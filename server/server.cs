using System.Net;
using System.Net.Sockets;
using System.Text;
using MySql.Data.MySqlClient;


namespace server
{
    class Program
    {
        static Dictionary<string, List<TcpClient>> gameRooms = new Dictionary<string, List<TcpClient>>();
        static List<string> playerInfo = new List<string>();  // 로그인한 플레이어 정보 저장
        static List<string> readyPlayer = new List<string>(); // ready한 플레이어 정보 저장

        static List<string> topics = new List<string> { "동물", "도시", "과일", "물건" }; // 주제 리스트
        static Dictionary<string, List<string>> keywords = new Dictionary<string, List<string>>()
        {
            { "동물", new List<string> { "호랑이", "코끼리", "강아지", "고양이", "판다", "거북이", "늑대", "오리", "고래" } },
            { "도시", new List<string> { "서울", "부산", "대구", "울산", "토론토", "라스베가스", "바르셀로나", "몬차", "모나코" } },
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
        private static void SendSignUpModetoClient(string mode, TcpClient client)
        {
            Console.WriteLine(mode);
            byte[] data = Encoding.UTF8.GetBytes(mode);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            Console.WriteLine("모드 보냄");
        }

        private static void SendNicknametoClient(string nickname, TcpClient client)
        {
            string response = string.IsNullOrEmpty(nickname) ? "login_failure" : $"login_success:{nickname}";
            byte[] data = Encoding.UTF8.GetBytes(response);
            //Console.WriteLine("메세지 보냄");
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }

        // gameRoom의 모든 클라인언트에게 메시지 전송
        private static void BroadcastMessage( string gameRoom, string message, TcpClient excludeClient)
        {
            if (gameRooms.ContainsKey(gameRoom))
            {
                byte[] data = Encoding.UTF8.GetBytes($"{message}");

                Console.WriteLine("gameRooms[gameRoom].Capacity: " + gameRooms[gameRoom].Capacity.ToString() + ": " + message);
                Console.WriteLine("gameRooms[gameRoom].Count : " + gameRooms[gameRoom].Count + ": " + message);
                Console.WriteLine("playerInfo : " + playerInfo.Count + ": " + message);
                Console.WriteLine("readyPlayer : " + readyPlayer.Count + ": " + message);

                foreach (var client in gameRooms[gameRoom])
                {
                    if (client != excludeClient)
                    {
                        Console.WriteLine($"메세지 보냄 {message}");
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
        }

        public static void SendMessageToClient(string gameRoom, string message, TcpClient targetClient)
        {
            if (gameRooms.ContainsKey(gameRoom))
            {
                byte[] data = Encoding.UTF8.GetBytes($"{message}");
                NetworkStream stream = targetClient.GetStream();
                Console.WriteLine($"메세지 보냄 {message}");
                stream.Write(data, 0, data.Length);
            }
        }

        private static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            string gameRoom = null;
            bool isLoggedIn = false;

            try
            {
                string playerID = null;
                string playerNick = null;

                while (true)
                {
                    // 메시지 수신
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    string[] messageParts = message.Split(':');
                    string action = messageParts[0];
                    string mode_signup = null;

                    if (action == "login" && messageParts.Length == 3)
                    {
                        playerID = messageParts[1];
                        string playerPW = messageParts[2];
                        playerNick = login(playerID, playerPW);
                        SendNicknametoClient(playerNick, client);
                        if (!string.IsNullOrEmpty(playerNick))
                        {
                            lock (playerInfo)
                            {
                                playerInfo.Add($"{playerID}:{playerNick}");
                                Console.WriteLine($"플레이어 로그인 - ID: {playerID}, 닉네임: {playerNick}");
                                isLoggedIn = true;
                            }
                        }
                    }else if (action == "signup")
                    {
                        Console.WriteLine("회원가입 모드");
                        playerID=messageParts[1];
                        string playerPW = messageParts[2];
                        playerNick = messageParts[3];
                        Console.WriteLine(playerID + " " + playerPW + " " + playerNick);
                        mode_signup = Signup(playerID, playerPW, playerNick);
                        SendSignUpModetoClient(mode_signup, client);
                    }

                    if (isLoggedIn)
                    {
                        switch (action)
                        {
                            case "join":
                                JoinGame(messageParts, client, ref gameRoom);
                                break;
                            case "chat":
                                HandleChat(messageParts, gameRoom, client);
                                break;
                            case "ready":
                                HandleReady(messageParts, gameRoom, client);
                                break;
                            default:
                                Console.WriteLine("잘못된 메시지 형식");
                                break;
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
                RemoveClient(client, gameRoom);
            }
        }

        private static void JoinGame(string[] messageParts, TcpClient client, ref string gameRoom)
        {
            string playerID = messageParts[1];
            string playerNick = messageParts[2];
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
                    SendPlayerInfo(gameRoom);
                }
                else
                {
                    //byte[] msg = Encoding.UTF8.GetBytes("방이 가득 찼습니다");
                    //stream.Write(msg, 0, msg.Length);
                    client.Close();
                    Console.WriteLine("456,CLOSE");
                }
            }
        }

        private static void HandleChat(string[] messageParts, string gameRoom, TcpClient client)
        {
            if (messageParts.Length != 3)
            {
                Console.WriteLine("잘못된 채팅 메시지 형식");
                return;
            }
            string nickname = messageParts[1];
            string chatMessage = messageParts[2];
            BroadcastMessage(gameRoom, $"chat:{nickname}:{chatMessage}", null);
        }

        private static void HandleReady(string[] messageParts, string gameRoom, TcpClient client)
        {
            string id = messageParts[1];
            string nickname = messageParts[2];
            Console.WriteLine($"{nickname} 준비 완료");

            if (messageParts.Length != 3)
            {
                Console.WriteLine("ready-잘못된 메시지 형식");
                return;
            }

            lock (readyPlayer)
            {
                if (!readyPlayer.Contains($"{id}:{nickname}"))
                {
                    readyPlayer.Add($"{id}:{nickname}");
                    BroadcastMessage(gameRoom, $"ready:{id}:{nickname}", null);
                }

                if (readyPlayer.Count == gameRooms[gameRoom].Count && gameRooms[gameRoom].Count >= 3)
                {
                    //BroadcastMessage(gameRoom, $"chat:server:모든 플레이어가 준비 완료, 게임 시작", null);
                    StartGame(gameRoom);
                }
                else
                {
                    BroadcastMessage(gameRoom, "chat:server:모든 player가 ready하면 게임이 시작됩니다.",null);
                }
            }
        }

        private static void StartGame(string gameRoom)
        {
            readyPlayer.Clear();
            Console.WriteLine("모든 플레이어가 준비 완료, 게임 시작");
            Random rand = new Random();
            string selectedTopic = topics[rand.Next(topics.Count)];
            List<string> topicKeywords = keywords[selectedTopic];
            string selectedKeyword = topicKeywords[rand.Next(topicKeywords.Count)];
            int liarIndex = rand.Next(readyPlayer.Count);

            //for (int i = 0; i < gameRooms[gameRoom].Count; i++)
            //{
                var client = gameRooms[gameRoom][liarIndex];
                BroadcastMessage(gameRoom, $"topic_keyword:{selectedTopic}:{selectedKeyword}", client);
                SendMessageToClient(gameRoom, $"topic_keyword:{selectedTopic}:Liar", client);

                //if (i == liarIndex)
                //{
                //    SendMessageToClient(gameRoom, $"topic_keyword:{selectedTopic}:Liar", client);
                //}
                //else
                //{
                //    SendMessageToClient(gameRoom, $"topic_keyword:{selectedTopic}:{selectedKeyword}", client);
                //}
            //}
        }

        private static void RemoveClient(TcpClient client, string gameRoom)
        {
            lock (gameRooms)
            {
                if (gameRoom != null && gameRooms.ContainsKey(gameRoom))
                {
                    gameRooms[gameRoom].Remove(client);
                    if (gameRooms[gameRoom].Count == 0)
                    {
                        gameRooms.Remove(gameRoom);
                    }
                }
            }
            Console.WriteLine("540,CLOSE");
            client.Close();
        }

        public static string Signup(string ID, string PW, string Nickname)
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            string mode_signup = null; // 0 : 회원가입 성공  1 : 회원가입 실패(중복된 ID) 2 : 회원가입 실패(중복된 닉네임)

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // DB 연결

                    // ID 중복 확인 (대소문자 구별)
                    string checkIdQuery = "SELECT * FROM user WHERE BINARY ID = @ID";
                    MySqlCommand checkIdCmd = new MySqlCommand(checkIdQuery, conn);
                    checkIdCmd.Parameters.AddWithValue("@ID", ID);
                    MySqlDataReader idReader = checkIdCmd.ExecuteReader();
                    if (idReader.Read())
                    {
                        idReader.Close();
                        Console.WriteLine("중복된 아이디입니다.");
                        mode_signup = "1";
                        return mode_signup;
                    }
                    idReader.Close();

                    // 닉네임 중복 확인 (대소문자 구별)
                    string checkNicknameQuery = "SELECT * FROM user WHERE BINARY NICKNAME = @NICKNAME";
                    MySqlCommand checkNicknameCmd = new MySqlCommand(checkNicknameQuery, conn);
                    checkNicknameCmd.Parameters.AddWithValue("@NICKNAME", Nickname);
                    MySqlDataReader nicknameReader = checkNicknameCmd.ExecuteReader();
                    if (nicknameReader.Read())
                    {
                        nicknameReader.Close();
                        Console.WriteLine("중복된 닉네임입니다.");
                        mode_signup = "2";
                        return mode_signup;
                    }
                    nicknameReader.Close();

                    // 새로운 user 삽입
                    string insertQuery = "INSERT INTO user (ID, PW, NICKNAME) VALUES (@ID, @PW, @NICKNAME)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@ID", ID);
                    insertCmd.Parameters.AddWithValue("@PW", PW);
                    insertCmd.Parameters.AddWithValue("@NICKNAME", Nickname);
                    insertCmd.ExecuteNonQuery();

                    Console.WriteLine("회원가입성공");
                    mode_signup = "0";
                    return mode_signup;
                    //start startForm = new start();
                    //startForm.Show();
                    //this.Hide();
                }
                catch { }

            }

            return mode_signup;
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
                    string query = "SELECT NICKNAME FROM user WHERE BINARY ID = @ID AND BINARY PW = @PW";
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
                        //CHECK.Text = "ID 또는 PW를 다시 입력해 주세요.";
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
        
        // gameRoom의 모든 클라이언트에게 플레이어 정보 전송
        private static void SendPlayerInfo(string gameRoom)
        {
            string playerListMessage = "player_info:";
            lock (playerInfo)
            {
                foreach (var p in playerInfo)
                {
                    playerListMessage += $"{p},";
                    Console.WriteLine($"{playerListMessage}");
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
                    //BroadcastMessage(gameRoom, $"{playerListMessage}", null);

                }
            }
        }
        
    }
}
