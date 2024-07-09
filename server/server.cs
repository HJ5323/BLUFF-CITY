using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1.X509;

namespace server
{
    class Program
    {
        static Dictionary<string, List<TcpClient>> gameRooms = new Dictionary<string, List<TcpClient>>();
        static List<string> playerInfo = new List<string>();  // 로그인한 플레이어 정보 저장

        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {

                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1"); // 로컬 IP

                server = new TcpListener(localAddr, port); // 서버 생성
                server.Start();

                Console.WriteLine("Server started...");


                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient(); // 클라이언트 연결 대기
                    Console.WriteLine("Connected!");

                    Thread clientThread = new Thread(() => HandleClient(client)); // 새로운 스레드 생성 -> 클라이언트 처리(HandleClient 호출)
                    clientThread.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private static void ClearDB()
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // DB 연결
                    string query = "DROP TABLE mafia_chats;" +

                    "DROP TABLE liar_chats;" +

                    "CREATE TABLE `mafia_chats` (" +
                        "`nickname` varchar(10) NOT NULL," +
                        "`mafia_chat` text NOT NULL," +
                        "`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP," +
                        "KEY `timestamp` (`timestamp`)," +
                        "CONSTRAINT `mafia_chats_ibfk_1` FOREIGN KEY(`nickname`) REFERENCES `user` (`NICKNAME`) ON DELETE CASCADE ON UPDATE CASCADE" +
                        ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;" +

                    "CREATE TABLE `liar_chats` (" +
                        "`nickname` varchar(10) NOT NULL," +
                        "`liar_chat` text NOT NULL," +
                        "`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP," +
                        "KEY `timestamp` (`timestamp`)," +
                        "CONSTRAINT `liar_chats_ibfk_1` FOREIGN KEY(`nickname`) REFERENCES `user` (`NICKNAME`) ON DELETE CASCADE ON UPDATE CASCADE" +
                        ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb3";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // gameRoom의 모든 클라인언트에게 메시지 전송
        private static void BroadcastMessage(string gameRoom, string message, TcpClient excludeClient)
        {
            lock (gameRooms)
            {
                if (gameRooms.ContainsKey(gameRoom))
                {
                    byte[] data = Encoding.UTF8.GetBytes($"{gameRoom}:{message}");
                    Console.WriteLine("메세지 보냄");

                    foreach (var client in gameRooms[gameRoom])
                    {
                        if (client != excludeClient)
                        {
                            NetworkStream stream = client.GetStream();
                            stream.Write(data, 0, data.Length);
                        }
                    }
                }
            }
        }

        private static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            string gameRoom = null;
            bool isLoggedIn = false; // 클라이언트 로그인 여부
            object obj = new object();

            try
            {
                string action = null;
                string playerID = null;
                string playerNick = null;

                // 초기 메시지 읽기
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string initialMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"수신된 초기 메시지: {initialMessage}");

                string[] messageParts = initialMessage.Split(':');
                if (messageParts.Length < 3)
                {
                    Console.WriteLine("잘못된 메시지 형식");
                    client.Close();
                    return;
                }

                action = messageParts[0];
                playerID = messageParts[1];
                playerNick = messageParts[2];
                gameRoom = messageParts.Length > 3 ? messageParts[3] : null;

                if (action == "login")
                {
                    lock (playerInfo)
                    {
                        playerInfo.Add($"{playerID}:{playerNick}"); // 플레이어 정보 저장
                    }
                    Console.WriteLine($"플레이어 로그인 - ID: {playerID}, 닉네임: {playerNick}");
                    isLoggedIn = true; // 로그인
                }
                else
                {
                    Console.WriteLine("처음에 로그인 메시지가 와야합니다");
                    client.Close();
                    return;
                }

                // 클라이언트가 join 메시지를 보낼 때까지 대기
                while (isLoggedIn)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // 클라이언트가 연결을 끊은 경우
                        break;
                    }

                    initialMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    messageParts = initialMessage.Split(':');
                    action = messageParts[0];

                    if (action == "join")
                    {

                        playerID = messageParts[1];
                        playerNick = messageParts[2];
                        gameRoom = messageParts[3];

                        Console.WriteLine($"플레이어 게임 입장 - ID: {playerID}, 닉네임: {playerNick}, 게임: {gameRoom}");

                        lock (gameRooms)
                        {
                            if (!gameRooms.ContainsKey(gameRoom))
                            {
                                gameRooms[gameRoom] = new List<TcpClient>();
                            }

                            if (gameRooms[gameRoom].Count < 8)
                            {
                                gameRooms[gameRoom].Add(client);
                                if (gameRooms[gameRoom].Count == 1)
                                {
                                    ClearDB();
                                    Console.WriteLine($"ClearDB");

                                }
                                Console.WriteLine($"{playerNick}가 방에 입장했습니다: {gameRoom}");

                                BroadcastMessage(gameRoom, $"{playerNick}님이 게임에 참가했습니다!", client);

                                SendPlayerInfo(gameRoom); // 모든 플레이어 정보 전송

                            }
                            else
                            {
                                byte[] msg = Encoding.UTF8.GetBytes("방이 가득 찼습니다");
                                stream.Write(msg, 0, msg.Length);
                                client.Close();
                                return;
                            }
                        }
                    }

                    else if (action == "chat")
                    {
                        Console.WriteLine(messageParts[1]);
                        Console.WriteLine(messageParts[2]);

                        if (messageParts.Length != 3)
                        {
                            Console.WriteLine("잘못된 채팅 메시지 형식");
                            continue;
                        }

                        string nickname = messageParts[1];
                        string chatMessage = messageParts[2];

                        SaveMessageToDatabase(nickname, chatMessage); // 데이터베이스에 메시지 저장

                        lock(obj)
                        {
                            // 모든 저장된 메시지를 클라이언트에 전송
                            SendAllMessagesToClient(client, gameRoom);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (gameRoom != null)
                {
                    lock (gameRooms)
                    {
                        gameRooms[gameRoom].Remove(client);
                        BroadcastMessage(gameRoom, $"{gameRoom}: 플레이어가 게임에서 나갔습니다!", client);
                    }
                }
                client.Close();
            }
        }

        private static void SendAllMessagesToClient(TcpClient client, string gameRoom)
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nickname, mafia_chat FROM mafia_chats ORDER BY timestamp";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder allMessages = new StringBuilder();
                        while (reader.Read())
                        {
                            string nickname = reader.GetString("nickname");
                            string message = reader.GetString("mafia_chat");
                            allMessages.AppendLine($"\n[{nickname}] {message}");
                        }
                        byte[] data = Encoding.UTF8.GetBytes(allMessages.ToString());
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
            }
        }

        // gameRoom의 모든 클라이언트에게 플레이어 정보 전송
        private static void SendPlayerInfo(string gameRoom)
        {
            string playerListMessage = "player_info:";
            lock (playerInfo)
            {
                foreach (var p in playerInfo)
                {
                    playerListMessage += $"{p},";
                }
            }
            playerListMessage = playerListMessage.TrimEnd(',');

            lock (gameRooms)
            {
                foreach (var client in gameRooms[gameRoom])
                {
                    NetworkStream stream = client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(playerListMessage);
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        // 메시지 데이터베이스에 저장
        private static void SaveMessageToDatabase(string nickname, string message)
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO mafia_chats (nickname, mafia_chat) VALUES (@nickname, @message)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nickname", nickname);
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.ExecuteNonQuery();

                    // 데이터베이스에 메시지가 저장된 후에 로드하여 클라이언트에 전송
                    LoadMessagesFromDatabase();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
            }
        }

        // 데이터베이스에서 메시지를 로드하여 모든 클라이언트에게 브로드캐스트
        private static void LoadMessagesFromDatabase()
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nickname, mafia_chat FROM mafia_chats ORDER BY timestamp";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nickname = reader.GetString("nickname");
                            string message = reader.GetString("mafia_chat");
                            BroadcastMessage("mafia_game", $"{nickname}: {message}", null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
            }
        }

    }
}
