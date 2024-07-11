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
using System.Runtime.InteropServices.ObjectiveC;

namespace server
{
    class Program
    {
        static Dictionary<string, List<TcpClient>> gameRooms = new Dictionary<string, List<TcpClient>>();
        static List<string> playerInfo = new List<string>();  // 로그인한 플레이어 정보 저장
        static List<string> readyPlayer = new List<string>(); // ready한 플레이어 정보 저장

        static List<string> topics = new List<string> { "동물", "음식", "도시" }; // 주제 리스트
        static Dictionary<string, List<string>> keywords = new Dictionary<string, List<string>>()
        {
            { "동물", new List<string> { "호랑이", "코끼리", "강아지", "고양이", "판다", "거북이", "늑대" } },
            { "음식", new List<string> { "피자", "햄버거", "소금빵", "치킨", "돼지국밥", "떡볶이", "파스타", "샌드위치" } },
            { "도시", new List<string> { "서울", "부산", "대구", "울산", "토론토", "라스베가스", "바르셀로나", "몬차", "모나코", "스필버그", "실버스톤" } },
            { "과일", new List<string> { "사과", "복숭아", "골든키위", "체리", "자두", "바나나", "딸기", "자몽", "망고" } },
            { "물건", new List<string> { "세탁기", "에어컨", "노트북", "연필", "충전기", "스마트폰", "시계", "가방", "책" } }
        };

        //static object lockObj = new object();

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

        private static void SendNicknametoClient(string nickname, TcpClient client)
        {
            byte[] data = Encoding.UTF8.GetBytes(nickname);
            //Console.WriteLine("메세지 보냄");
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }

        // gameRoom의 모든 클라인언트에게 메시지 전송
        private static void BroadcastMessage( string gameRoom, string message, TcpClient excludeClient)
        {
            if (gameRooms.ContainsKey(gameRoom))
            {
                byte[] data = Encoding.UTF8.GetBytes($"{gameRoom}:{message}");
                foreach (var client in gameRooms[gameRoom])
                {
                    if (client != excludeClient)
                    {
                        Console.WriteLine("메세지 보냄");
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
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
            //object obj = new object();

            try
            {
                string action = null;
                string playerID = null;
                string playerPW = null;
                string playerNick = null;


                // 클라이언트가 join 메시지를 보낼 때까지 대기
                while (true)
                {
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

                    if (action == "login")
                    {
                        playerID = messageParts[1];
                        playerPW = messageParts[2];
                        gameRoom = messageParts.Length > 3 ? messageParts[3] : null;
                        playerNick = login(playerID, playerPW);

                        SendNicknametoClient(playerNick, client);

                        playerInfo.Add($"{playerID}:{playerNick}"); // 플레이어 정보 저장

                        Console.WriteLine($"플레이어 로그인 - ID: {playerID}, 닉네임: {playerNick}");
                        isLoggedIn = true; // 로그인
                    }
                    else if (action != "chat" && action != "join")
                    {
                        Console.WriteLine("처음에 로그인 메시지가 와야합니다");
                        client.Close();
                        return;
                    }
                    /*
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // 클라이언트가 연결을 끊은 경우
                        break;
                    }

                    initialMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    messageParts = initialMessage.Split(':');
                    action = messageParts[0];
                    */
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

                                //SendPlayerInfo(gameRoom); // 모든 플레이어 정보 전송

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
                        if (messageParts.Length != 3)
                        {
                            Console.WriteLine("잘못된 채팅 메시지 형식");
                            continue;
                        }

                        string nickname = messageParts[1];
                        string chatMessage = messageParts[2];
                        BroadcastMessage(gameRoom, $"{nickname} : {chatMessage}", null);
                        // SaveMessageToDatabase(nickname, chatMessage); // 데이터베이스에 메시지 저장

                        // 모든 저장된 메시지를 클라이언트에 전송
                        //SendAllMessagesToClient(client, gameRoom);

                    }

                    else if (action == "ready")
                    {
                        string id = messageParts[1];
                        string nickname = messageParts[2];
                        Console.WriteLine($"{nickname} 준비 완료");

                        if (messageParts.Length != 3)
                        {
                            Console.WriteLine("잘못된 메시지 형식");
                            continue;
                        }

                        lock (readyPlayer)
                        {
                            if (!readyPlayer.Contains($"{id}:{nickname}"))
                            {
                                readyPlayer.Add($"{id}:{nickname}"); // ready 플레이어 정보 저장
                                BroadcastMessage(gameRoom, $"{nickname} : READY!", null);
                            }
                            if (readyPlayer.Count == playerInfo.Count && playerInfo.Count >= 3) // 최소 3명 이상이며 모두 ready했을 때 게임 진행 가능
                            {
                                Console.WriteLine("모든 플레이어가 준비 완료, 게임 시작");

                                // 랜덤 주제와 키워드 선택
                                Random rand = new Random();
                                string selectedTopic = topics[rand.Next(topics.Count)];
                                List<string> topicKeywords = keywords[selectedTopic];
                                string selectedKeyword = topicKeywords[rand.Next(topicKeywords.Count)];

                                // 라이어 선정
                                int liarIndex = rand.Next(readyPlayer.Count);

                                for (int i = 0; i < readyPlayer.Count; i++)
                                {
                                    TcpClient currentClient = gameRooms[gameRoom][i];
                                    NetworkStream currentStream = currentClient.GetStream();

                                    if (i == liarIndex)
                                    {
                                        byte[] liarData = Encoding.UTF8.GetBytes("topic_keyword:" + selectedTopic + ":Liar");
                                        currentStream.Write(liarData, 0, liarData.Length); // 라이어에게는 키워드를 보내지 않음
                                    }
                                    else
                                    {
                                        byte[] keywordData = Encoding.UTF8.GetBytes("topic_keyword:" + selectedTopic + ":" + selectedKeyword);
                                        currentStream.Write(keywordData, 0, keywordData.Length); // 라이어가 아닌 플레이어에게 주제와 키워드 전송
                                    }
                                }
                            }
                            else
                            {
                                byte[] msg = Encoding.UTF8.GetBytes("모든 player가 ready하면 게임이 시작됩니다.");
                                stream.Write(msg, 0, msg.Length);
                                client.Close();
                                return;
                            }
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
                if (isLoggedIn && gameRoom != null)
                {
                    lock (gameRooms)
                    {
                        gameRooms[gameRoom].Remove(client); // 클라이언트를 방에서 제거

                        // 방에 더 이상 클라이언트가 없으면 방 삭제
                        if (gameRooms[gameRoom].Count == 0)
                        {
                            gameRooms.Remove(gameRoom);
                        }
                    }
                }
                client.Close();
            }
        }
        public static string login(string ID, string PW)
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            string nickname = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // DB 연결
                    string query = "SELECT NICKNAME FROM user WHERE ID = @ID AND PW = @PW";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", ID); // ID 매개변수 설정
                    cmd.Parameters.AddWithValue("@PW", PW); // PW 매개변수 설정
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // 로그인 성공 시
                    {
                        nickname = reader["NICKNAME"].ToString();
                        return nickname;
                    }
                    else // 로그인 실패 시
                    {
                        Console.WriteLine("로그인 실패");
                        return nickname;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return nickname;
                }
            }
        }
        /*
        // gameRoom의 모든 클라이언트에게 플레이어 정보 전송
        private static void SendPlayerInfo(string gameRoom)
        {
            string playerListMessage = "player_info:";
            lock (playerInfo)
            {
                foreach (var p in playerInfo)
                {
                    playerListMessage += $"{p},";
                    Console.WriteLine($"{p},");

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
        */
    }
}
