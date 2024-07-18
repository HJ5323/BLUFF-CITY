using System;
using System.Diagnostics.Metrics;
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
        static List<string> entryPlayer = new List<string>();  // gameRoom에 입장한 플레이어 정보 저장
        static List<string> readyPlayer = new List<string>(); // ready한 플레이어 정보 저장
        private static Dictionary<string, string> liarVotes = new Dictionary<string, string>();// liar투표결과 저장
        static string keyword;
        static bool gamestart;
        static bool isLoggedIn;
        static string liarPlayerNick = null;

        static List<string> topics = new List<string> { "동물", "도시", "과일", "물건" }; // 주제 리스트
        static Dictionary<string, List<string>> keywords = new Dictionary<string, List<string>>()
        {
            { "동물", new List<string> { "호랑이", "코끼리", "강아지", "고양이", "판다", "거북이", "늑대", "오리", "고래" } },
            { "도시", new List<string> { "서울", "부산", "대구", "울산", "토론토", "라스베가스", "바르셀로나", "몬차", "모나코" } },
            { "과일", new List<string> { "사과", "복숭아", "골든키위", "체리", "자두", "바나나", "딸기", "자몽", "망고" } },
            { "물건", new List<string> { "세탁기", "에어컨", "노트북", "연필", "충전기", "스마트폰", "시계", "가방", "책" } }
        };

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

        private static void SendSignUpModetoClient(string mode, TcpClient client)
        {
            Console.WriteLine(mode);
            byte[] data = Encoding.UTF8.GetBytes(mode);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            Console.WriteLine("모드 보냄");
        }

        private static void SendNicknametoClient(string mode_login, string ID, string nickname, TcpClient client)
        {
            string response = $"{mode_login}:{ID}:{nickname}";
            byte[] data = Encoding.UTF8.GetBytes(response);
            //Console.WriteLine("메세지 보냄");
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            Console.WriteLine(response);
        }

        // gameRoom의 모든 클라인언트에게 메시지 전송
        private static void BroadcastMessage(string gameRoom, string message, TcpClient excludeClient)
        {
            if (gameRooms.ContainsKey(gameRoom))
            {
                byte[] data = Encoding.UTF8.GetBytes($"{message}");

                //Console.WriteLine("gameRooms[gameRoom].Capacity: " + gameRooms[gameRoom].Capacity.ToString() + ": " + message);
                //Console.WriteLine("gameRooms[gameRoom].Count : " + gameRooms[gameRoom].Count + ": " + message);
                //Console.WriteLine("playerInfo : " + playerInfo.Count + ": " + message);
                //Console.WriteLine("entryPlayer : " + entryPlayer.Count + ": " + message);
                //Console.WriteLine("readyPlayer : " + readyPlayer.Count + ": " + message);

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

        // Game 시작 여부에 따라 게임 입장 제어
        private static void CreateRoomMessage(string message, TcpClient client)
        {

            byte[] data = Encoding.UTF8.GetBytes($"{message}");
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            Console.WriteLine($"message: {message}");
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
            isLoggedIn = false;

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
                    Console.WriteLine($"1111--{message}");
                    string[] messageParts = message.Split(':');
                    string action = messageParts[0];
                    string mode_signup = null;
                    string mode_login = null;
                    if (action == "login" && messageParts.Length == 3)
                    {
                        playerID = messageParts[1];
                        string playerPW = messageParts[2];
                        playerNick = login(playerID, playerPW);
                        for (int i = 0; i < playerInfo.Count; i++)
                        {
                            string[] infoParts = playerInfo[i].Split(':');
                            if (playerNick != null)
                            {
                                if (infoParts[1] == playerNick)
                                {
                                    Console.WriteLine("중복 로그인");
                                    mode_login = "1";
                                    playerNick = null;
                                }
                            }
                            else
                            {
                                //playerNick == null 인 상태 > database에 아이디 없음
                                mode_login = "2";
                            }
                        }
                        Console.WriteLine($"2222--{mode_login}");
                        if (!string.IsNullOrEmpty(playerNick))
                        {
                            lock (playerInfo)
                            {
                                playerInfo.Add($"{playerID}:{playerNick}");
                                Console.WriteLine($"플레이어 로그인 - ID: {playerID}, 닉네임: {playerNick}");
                                mode_login = "0";
                                isLoggedIn = true;
                            }
                        }
                        SendNicknametoClient(mode_login, playerID, playerNick, client);

                    }
                    else if (action == "signup")
                    {
                        Console.WriteLine("회원가입 모드");
                        playerID = messageParts[1];
                        string playerPW = messageParts[2];
                        playerNick = messageParts[3];
                        Console.WriteLine(playerID + " " + playerPW + " " + playerNick);
                        mode_signup = Signup(playerID, playerPW, playerNick);
                        SendSignUpModetoClient(mode_signup, client);
                    }
                    if (isLoggedIn)
                    {
                        Console.WriteLine(message);

                        switch (action)
                        {
                            case "CreatRoom":
                                if (gamestart == true)
                                {
                                    CreateRoomMessage("createRoom:failure:이미 게임이 실행되었습니다. 게임이 끝날때까지 기다려주세요.", client);
                                    break;
                                }
                                else
                                {
                                    CreateRoomMessage("createRoom:success:liarGame 생성", client);
                                    break;
                                }
                            case "join":
                                JoinGame(messageParts, client, ref gameRoom);
                                break;
                            case "chat":
                                HandleChat(messageParts, gameRoom, client);
                                break;
                            case "ready":
                                HandleReady(messageParts, gameRoom, client);
                                break;
                            case "vote":
                                HandleVoteMessage(messageParts, gameRoom);
                                break;
                            case "GuessKeyword":
                                HandleGuessKeyword(messageParts, gameRoom);
                                break;
                            case "logout":
                                HandleLogout(messageParts);
                                break;
                            case "exitGameroom":
                                HandleExitGameroom(messageParts, gameRoom, client);
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
                    gameRooms[gameRoom].Capacity = 8;
                    Console.WriteLine($"JoinGame :gameRooms[gameRoom] : {gameRooms[gameRoom]}");
                }

                if (gameRooms[gameRoom].Count < 8)
                {
                    gameRooms[gameRoom].Add(client);
                    entryPlayer.Add($"{playerID}:{playerNick}:{gameRoom}:{null}");

                    Console.WriteLine($"{playerNick}가 방에 입장했습니다: {gameRoom}");
                    BroadcastMessage(gameRoom, $";{playerNick}님이 게임에 참가했습니다!", client);
                    SendEntryPlayerInfo(gameRoom);
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
            BroadcastMessage(gameRoom, $";chat:{nickname}:{chatMessage}", null);
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
                if (readyPlayer.Contains($"{id}:{nickname}"))
                {
                    // 이미 준비된 상태이면, readyPlayer 목록에서 제거
                    readyPlayer.Remove($"{id}:{nickname}");
                    BroadcastMessage(gameRoom, $";cancel_ready:{id}:{nickname}", null);
                    Console.WriteLine($"{nickname} 준비 취소");
                }
                else
                {
                    // 준비되지 않은 상태이면, readyPlayer 목록에 추가
                    readyPlayer.Add($"{id}:{nickname}");
                    BroadcastMessage(gameRoom, $";ready:{id}:{nickname}", null);
                    Console.WriteLine($"{nickname} 준비 완료");

                    if (readyPlayer.Count == gameRooms[gameRoom].Count && gameRooms[gameRoom].Count >= 3)
                    {
                        StartGame(gameRoom);
                    }
                    else
                    {
                        BroadcastMessage(gameRoom, ";chat:server:모든 player가 ready하면 게임이 시작됩니다.", null);
                    }
                }
            }
        }

        // 게임 시작
        private static void StartGame(string gameRoom)
        {
            gamestart = true;
            readyPlayer.Clear();
            Console.WriteLine("모든 플레이어 준비 완료, 게임 시작");
            Random rand = new Random();
            string selectedTopic = topics[rand.Next(topics.Count)];
            List<string> topicKeywords = keywords[selectedTopic];
            string selectedKeyword = topicKeywords[rand.Next(topicKeywords.Count)];
            int liarIndex = rand.Next(entryPlayer.Count);
            keyword = selectedKeyword;

            // liar 지목된 플레이어 설정
            //for (int i = 0; i < entryPlayer.Count; i++)
            //{
            //    string entryInfo = entryPlayer[i];
            //    if (i == liarIndex)
            //    {
            //        entryPlayer[i] = $"{entryInfo}:liar";
            //    }
            //    else
            //    {
            //        entryPlayer[i] = $"{entryInfo}:no";
            //    }
            //}
            for (int i = 0; i < entryPlayer.Count; i++)
            {
                string[] playerInfo = entryPlayer[i].Split(':');
                if (i == liarIndex && playerInfo.Length > 3)
                {
                    playerInfo[3] = "liar";
                }
                else if (playerInfo.Length > 3)
                {
                    playerInfo[3] = "no";
                }
                entryPlayer[i] = string.Join(":", playerInfo);
            }
            Console.WriteLine($"entryPlayer[0] : {entryPlayer[0]}");
            Console.WriteLine($"entryPlayer[1] : {entryPlayer[1]}");
            Console.WriteLine($"entryPlayer[2] : {entryPlayer[2]}");


            var liarClient = gameRooms[gameRoom][liarIndex];
            BroadcastMessage(gameRoom, $";topic_keyword:{selectedTopic}:{selectedKeyword}", liarClient);
            SendMessageToClient(gameRoom, $";topic_keyword:{selectedTopic}:Liar", liarClient);

            BroadcastMessage(gameRoom, ";disable_chat:server", null);

            // 첫 번째 플레이어의 발언 순서 시작
            if (entryPlayer.Count > 0)
            {
                string firstPlayerID = entryPlayer[0].Split(':')[0];
                string firstPlayerNick = entryPlayer[0].Split(':')[1];
                HandlePlayerTurn(firstPlayerID, firstPlayerNick, gameRoom);
            }
        }
        private static void HandlePlayerTurn(string playerID, string playerNick, string gameRoom)
        {
            // 발언 순서의 플레이어만 chat 활성화
            TcpClient targetClient = GetTcpClientFromPlayerID(playerNick, gameRoom);
            SendMessageToClient(gameRoom, $";enable_chat:{playerNick}", targetClient);
            BroadcastMessage(gameRoom, $";chat:server:{playerNick} 차례입니다.", null);

            if (targetClient == null)
            {
                Console.WriteLine($"Player with ID {playerID} not found in game room {gameRoom}.");
                return; // 대상 클라이언트를 찾지 못한 경우 처리
            }

            // 각 플레이어의 순서가 시작될 때마다 타이머 시작
            int timeLeft = 3;
            System.Timers.Timer timer = new System.Timers.Timer(1000); // 1초마다 실행

            timer.Elapsed += (sender, e) =>
            {
                if (timeLeft >= 0)
                {
                    BroadcastMessage(gameRoom, $";timer:{playerNick}:{timeLeft}", null);
                    timeLeft--;
                }
                else
                {
                    timer.Stop();
                    BroadcastMessage(gameRoom, $";chat:server:{playerNick} - time out!", null);
                    SendMessageToClient(gameRoom, ";disable_chat:server", targetClient);

                    // 다음 플레이어로 넘어가기
                    MoveToNextPlayer(playerNick, gameRoom);

                }
            };
            timer.Start();
        }

        private static void MoveToNextPlayer(string currentPlayerNick, string gameRoom)
        {
            int currentPlayerIndex = GetCurrentPlayerIndex(currentPlayerNick);
            int nextPlayerIndex = (currentPlayerIndex + 1) % entryPlayer.Count;

            if (nextPlayerIndex == 0)
            {
                HandleGroupSpeech(currentPlayerNick, gameRoom);
            }
            else
            {
                string nextPlayerID = entryPlayer[nextPlayerIndex].Split(':')[0];
                string nextPlayerNick = entryPlayer[nextPlayerIndex].Split(':')[1];
                HandlePlayerTurn(nextPlayerID, nextPlayerNick, gameRoom);
            }
        }

        // playerID를 사용하여 gameRooms에서 해당 TcpClient를 찾아 반환하는 함수
        private static TcpClient GetTcpClientFromPlayerID(string playerNick, string gameRoom)
        {
            // 현재 플레이어의 인덱스를 가져옴
            int playerIndex = GetCurrentPlayerIndex(playerNick);

            if (playerIndex != -1)
            {
                return gameRooms[gameRoom][playerIndex];
            }
            return null;
        }

        // 현재 플레이어의 인덱스를 반환하는 함수
        private static int GetCurrentPlayerIndex(string playerNick)
        {
            for (int i = 0; i < entryPlayer.Count; i++)
            {
                if (entryPlayer[i].Split(':')[1] == playerNick)
                {
                    return i;
                }
            }
            return -1; // 찾지 못한 경우
        }

        private static void HandleGroupSpeech(string playerNick, string gameRoom)
        {
            //각 플레이어 발언 후 모든 플레이어 자유 발언(1분)
            BroadcastMessage(gameRoom, ";chat:server:모든 플레이어는 자유롭게 발언해 주세요.", null);
            BroadcastMessage(gameRoom, $";enable_chat:{playerNick}", null);

            // 모든 플레이어가 자유발언
            int timeLeft = 5;
            System.Timers.Timer timer = new System.Timers.Timer(1000); // 1초마다 실행

            timer.Elapsed += (sender, e) =>
            {
                if (timeLeft >= 0)
                {
                    BroadcastMessage(gameRoom, $";timer:server:{timeLeft}", null);
                    timeLeft--;
                }
                else
                {
                    timer.Stop();
                    BroadcastMessage(gameRoom, $";chat:server:time out!", null);
                    BroadcastMessage(gameRoom, ";disable_chat:server", null);

                    // 의심되는 Liar 지목 요청
                    BroadcastMessage(gameRoom, ";chat:server:의심되는 Liar를 지목해주세요", null);
                    BroadcastMessage(gameRoom, ";start_voting:server", null); // 투표 시작 메시지
                }
            };
            timer.Start(); // 타이머 시작
        }

        private static void HandleVoteMessage(string[] messageParts, string gameRoom)
        {
            string voter = messageParts[1];
            string votee = messageParts[2];
            int voteState = int.Parse(messageParts[3]);

            if (voteState == 1)
            {
                HandleVote(voter, votee, gameRoom);
            }
            else if (voteState == 0)
            {
                // 투표 취소 처리
                if (liarVotes.ContainsKey(voter) && liarVotes[voter] == votee)
                {
                    liarVotes.Remove(voter);
                    Console.WriteLine("Remove:liarVotes.count : " + liarVotes.Count);
                    foreach (var kvp in liarVotes)
                    {
                        Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
                    }
                }
            }
        }

        private static void HandleVote(string voter, string votee, string gameRoom)
        {
            lock (liarVotes)
            {
                lock (liarVotes)
                {
                    liarVotes[voter] = votee;
                    Console.WriteLine("liarVotes.count : " + liarVotes.Count);
                    foreach (var kvp in liarVotes)
                    {
                        Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
                    }
                }

                // 모든 플레이어 투표 완료
                if (liarVotes.Count == entryPlayer.Count)
                {
                    BroadcastVotingResult(gameRoom); // 결과 방송
                }
            }
        }

        private static void BroadcastVotingResult(string gameRoom)
        {
            lock (liarVotes)
            {
                // votee 별 득표수 계산
                var voteCounts = new Dictionary<string, int>();
                foreach (var votee in liarVotes.Values)
                {
                    if (!voteCounts.ContainsKey(votee))
                    {
                        voteCounts[votee] = 0;
                    }
                    voteCounts[votee]++;
                }

                // 최다 득표 votee 찾기
                string liar = "";
                int maxVotes = 0;
                bool isTie = false;
                foreach (var voteCount in voteCounts)
                {
                    if (voteCount.Value > maxVotes)
                    {
                        maxVotes = voteCount.Value;
                        liar = voteCount.Key;
                        isTie = false;
                    }
                    else if (voteCount.Value == maxVotes)
                    {
                        isTie = true;
                    }
                }

                // 동점인 경우
                if (isTie)
                {
                    // 동점이 발생했음을 방송
                    BroadcastMessage(gameRoom, ";start_voting:server", null); // 투표 시작 메시지
                    BroadcastMessage(gameRoom, $";chat:server:투표 결과 동점입니다. 다시 투표해주세요.", null);
                    liarVotes.Clear();
                    return;
                }
                else
                {
                    // 투표 결과 방송
                    BroadcastMessage(gameRoom, $";maxVotes:server:{liar}:{liar}이 Liar로 지목되었습니다", null);

                    // entryPlayer에서 liar 여부 확인하여 추가 작업 수행
                    foreach (var player in entryPlayer)
                    {
                        string[] playerParts = player.Split(':');
                        string playerNick = playerParts[1];
                        string isLiar = playerParts[3];

                        if (isLiar == "liar")
                        {
                            liarPlayerNick = playerNick; // liar인 사람의 playerNick을 변수에 할당
                            break; // liar를 찾았으면 더 이상 반복할 필요 없음
                        }
                    }

                    if (liarPlayerNick == liar)
                    {
                        TcpClient liarClient = GetTcpClientFromPlayerID(liarPlayerNick, gameRoom);

                        // liar가 최다 득표
                        SendMessageToClient(gameRoom, $";liar:server:{keyword}", liarClient);
                        BroadcastMessage(gameRoom, $";chat:server:{liarPlayerNick}가 라이어 맞습니다.", null);
                        BroadcastMessage(gameRoom, $";chat:server:키워드 고르는 중...", null);
                    }
                    else
                    {
                        // 시민 최다득표, liar 승리
                        BroadcastMessage(gameRoom, $";chat:server:{liar}는 라이어가 아닙니다.", null);
                        BroadcastMessage(gameRoom, $";noliar:server:라이어 {liarPlayerNick}가 승리하였습니다.", null);
                        // 라이어 승점 +1
                        HandlePoints("liar", 1, gameRoom);
                    }
                }
            }
        }


        private static void HandleGuessKeyword(string[] messageParts, string gameRoom)
        {
            string result = messageParts[1];

            if (result == "right")
            {
                BroadcastMessage(gameRoom, $";chat:server:라이어 {liarPlayerNick}가 키워드를 맞추었습니다.", null);
                BroadcastMessage(gameRoom, $";chat:server:라이어 {liarPlayerNick}가 승리하였습니다.", null);
                // 라이어 승점 +1
                HandlePoints("liar", 1, gameRoom);
            }
            else if (result == "wrong")
            {
                BroadcastMessage(gameRoom, $";chat:server:라이어 {liarPlayerNick}가 키워드를 맞추지 못하였습니다.", null);
                BroadcastMessage(gameRoom, $";chat:server:시민이 승리하였습니다.", null);
                // 시민 승점 +1
                HandlePoints("no", 1, gameRoom);
            }
        }

        private static void HandlePoints(string targetRole, int point, string gameRoom)
        {
            foreach (var player in entryPlayer)
            {
                string[] playerParts = player.Split(':');
                string playerNick = playerParts[1];
                string isliar = playerParts[3];

                if (isliar == targetRole)
                {
                    SendPoint(playerNick, point, gameRoom);
                }
            }
        }

        private static void HandleLogout(string[] messageParts)
        {
            Console.WriteLine("---------------LOGOUT----------");
            string id = messageParts[1];
            string nickname = messageParts[2];
            if (playerInfo.Contains($"{id}:{nickname}"))
            {
                playerInfo.Remove($"{id}:{nickname}");
                Console.WriteLine($"{nickname} 로그아웃");
                isLoggedIn = false;
            }
        }

        private static void EndGame(string playerNick, string gameRoom)
        {
            BroadcastMessage(gameRoom, $";close:server:게임이 곧 재설정됩니다.", null);

            lock (liarVotes)
            {
                liarVotes.Clear();
            }

            //Console.WriteLine("EndGame : gameRooms[gameRoom].Count " + gameRooms[gameRoom].Count);
            //Console.WriteLine("EndGame : playerInfo : " + playerInfo.Count);
            //Console.WriteLine("EndGame : entryPlayer : " + entryPlayer.Count);
            //Console.WriteLine("EndGame : readyPlayer : " + readyPlayer.Count);
            //Console.WriteLine("EndGame : gameRoom  : " + gameRoom);
            //Console.WriteLine("EndGame : liarVotes  : " + liarVotes);

            // 게임 상태 초기화
            gamestart = false;
        }

       
        private static void RemoveClient(TcpClient client, string gameRoom)
        {
            // gameRooms 컬렉션 동기화
            lock (gameRooms)
            {
                if (gameRoom != null && gameRooms.ContainsKey(gameRoom))
                {
                    // 게임 방에서 클라이언트 제거
                    if (gameRooms[gameRoom].Contains(client))
                    {
                        gameRooms[gameRoom].Remove(client);
                    }

                    // 게임 방에 남아 있는 클라이언트 수 확인
                    if (gameRooms[gameRoom].Count == 0)
                    {
                        //gameRooms.Remove(gameRoom);
                    }
                }
            }
        }
    
        private static void HandleExitGameroom(string[] messageParts, string gameRoom, TcpClient client)
        {
            // 메시지 파트 검증
            if (messageParts.Length != 4)
            {
                Console.WriteLine("HandleExitGameroom-잘못된 메시지 형식");
                return;
            }

            string id = messageParts[1];
            string nickname = messageParts[2];
            string gameroom = messageParts[3];
            Console.WriteLine($"{nickname} HandleExitGameroom");

            lock (gameRooms)
            {
                //readyPlayer에서 제거
                lock (readyPlayer)
                {
                    if (readyPlayer.Contains($"{id}:{nickname}"))
                    {
                        readyPlayer.Remove($"{id}:{nickname}");
                        Console.WriteLine($"{nickname} - readyPlayer.Remove");
                    }
                }

                // entryPlayer에서 부분 일치 항목 제거
                lock (entryPlayer)
                {
                    for (int i = entryPlayer.Count - 1; i >= 0; i--)
                    {
                        string[] parts = entryPlayer[i].Split(':');
                        string entryId = parts[0];
                        string entryNickname = parts[1];

                        // ID와 nickname이 일치하는 경우 제거
                        if (entryId == id && entryNickname == nickname)
                        {
                            Console.WriteLine($"{entryPlayer[i]} - entryPlayer.Remove");
                            entryPlayer.RemoveAt(i);
                            BroadcastMessage(gameRoom, $";chat:server:{nickname}이 게임을 나갔습니다.", null);
                        }
                    }
                }

                // gameRooms에서 클라이언트 제거
                if (gameRooms.ContainsKey(gameroom))
                {
                    lock (gameRooms[gameroom])
                    {
                        if (gameRooms[gameroom].Contains(client))
                        {
                            gameRooms[gameroom].Remove(client);
                            Console.WriteLine($"{nickname} - gameRooms[{gameroom}].Remove");
                        }

                        // gameRooms[gameroom]이 더 이상 클라이언트를 포함하지 않으면 제거
                        if (gameRooms[gameroom].Count == 0)
                        {
                            //gameRooms.Remove(gameroom);
                            Console.WriteLine($"Removed game room: {gameroom}");
                        }
                    }
                }

                //Console.WriteLine("HandleExitGameroom : gameRooms[gameRoom].Capacity: " + gameRooms[gameRoom].Capacity.ToString());
                //Console.WriteLine("HandleExitGameroom : gameRooms[gameRoom].Count : " + gameRooms[gameRoom].Count);
                //Console.WriteLine("HandleExitGameroom : playerInfo : " + playerInfo.Count);
                //Console.WriteLine("HandleExitGameroom : entryPlayer : " + entryPlayer.Count);
                //Console.WriteLine("HandleExitGameroom : readyPlayer : " + readyPlayer.Count);
            }

            // 게임 시작 상태에 따라 entryPlayer 정보 업데이트
            if (!gamestart && gameRooms[gameroom].Count != 0)
            {
                SendEntryPlayerInfo(gameroom);
                Console.WriteLine("entryPlayer update");
            }
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

        public static void SendPoint(string playerNick, int point, string gameRoom)
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // DB 연결

                    // 기존 포인트 조회
                    string selectQuery = "SELECT point FROM user WHERE NICKNAME = @nickname";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                    selectCmd.Parameters.AddWithValue("@nickname", playerNick);

                    // 기존 포인트 추출
                    object currentPointObj = selectCmd.ExecuteScalar();
                    int currentPoint = currentPointObj != null ? Convert.ToInt32(currentPointObj) : 0;

                    // 포인트 추가하여 업데이트
                    int newPoint = currentPoint + point;
                    string updateQuery = "UPDATE user SET point = @newPoint WHERE NICKNAME = @nickname";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@newPoint", newPoint);
                    updateCmd.Parameters.AddWithValue("@nickname", playerNick);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"{point} points to {playerNick}에 {point} 추가 완료. total point: {newPoint}");
                    }
                    else
                    {
                        Console.WriteLine($"포인트 업데이트 실패, {playerNick}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            // 게임 종료
            EndGame(playerNick, gameRoom);
        }

        // gameRoom의 모든 클라이언트에게 플레이어 정보 전송
        private static void SendEntryPlayerInfo(string gameRoom)
        {
            string entryPlayerListMessage = "entry_player_info:";
            lock (entryPlayer)
            {
                foreach (var p in entryPlayer)
                {
                    entryPlayerListMessage += $"{p},";
                    Console.WriteLine($"{entryPlayerListMessage}");
                }
            }
            entryPlayerListMessage = entryPlayerListMessage.TrimEnd(',');

            lock (gameRooms)
            {
                foreach (var client in gameRooms[gameRoom])
                {
                    NetworkStream stream = client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(entryPlayerListMessage);
                    stream.Write(data, 0, data.Length);
                }
            }
        }

    }
}
