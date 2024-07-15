using MySqlX.XDevAPI.Common;
using System.Net.Sockets;
using System.Text;

namespace BLUFF_CITY
{
    internal class Network : IDisposable
    {
        private static Network instance;
        private static readonly object lockObj = new object();

        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        public delegate void MessageReceivedHandler(string message);
        public event MessageReceivedHandler MessageReceived;

        private Network()
        {
            ConnectToServer();
            StartReceiving();
        }

        public static Network Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Network();
                    }
                    return instance;
                }
            }
        }

        public void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 13000);
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void StartReceiving()
        {
            receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        OnMessageReceived(message);
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        protected virtual void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }
        public void SendSignupInfo(string id, string pw, string nickname)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    ConnectToServer();
                }

                string message = $"signup:{id}:{pw}:{nickname}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public void SendLoginInfo(string id, string pw)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    ConnectToServer();
                }

                string message = $"login:{id}:{pw}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public void Join(string playerID, string playerNickname)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes($"join:{playerID}:{playerNickname}:liar_game");
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string ReceiveNickname()
        {
            try
            {
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    return Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }

        public void SendMessage(string playerNickname, string chat)
        {
            try
            {
                string message = $"chat:{playerNickname}:{chat}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"{playerNickname}:{message}SENDMESSAGE 채팅 보냄");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SendReady(string playerID, string playerNickname)
        {
            try
            {
                string message = $"ready:{playerID}:{playerNickname}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SendVote(string playerID, string selectedPlayer, int voteState)
        {
            try
            {
                string message = $"vote:{playerID}:{selectedPlayer}:{voteState}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SendGuessMessage(string result)
        {
            try
            {
                string message = $"GuessKeyword:{result}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }

            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Abort();
                receiveThread = null;
            }
        }

        ~Network()
        {
            Dispose();
        }
    }
}
