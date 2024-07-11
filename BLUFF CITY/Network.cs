using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BLUFF_CITY
{
    internal class Network
    {
        private bool disposed = false; // Dispose 메서드가 한 번만 호출되도록 하는 플래그
        public Network(int mode) 
        { 
            if (mode == 0)
            {
                ConnectToServer();
                m_mode = mode;
            }else if (mode == 1)
            {
                ConnectToServer();

                receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();
                m_mode = mode;
            }
        }
        private int m_mode;
        private static TcpClient client; // 서버와 TCP 연결을 나타내는 TcpCient 객체
        private static NetworkStream stream; // 네트워크 스트림
        private Thread receiveThread; // 메시지 수신 스레드
        public delegate void MessageReceivedHandler(string message);
        public event MessageReceivedHandler MessageReceived;

        // 서버 연결
        public void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 13000); // 서버 연결
                stream = client.GetStream(); // 네트워크 스트림 설정
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }


        //// 서버에 로그인 정보 전송
        //public void SendLoginInfo(string id, string nickname)
        //{
        //    try
        //    {
        //        if (client == null || !client.Connected)
        //        {
        //            ConnectToServer();
        //        }

        //        string message = $"login:{id}:{nickname}";
        //        byte[] data = Encoding.UTF8.GetBytes(message);
        //        stream.Write(data, 0, data.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception: {ex.Message}");
        //    }
        //}

        // 서버에 로그인 정보 전송
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
        // gameRoom==liar_game 참여
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
        /*
        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[256];
                int bytesRead;
                int mode;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    mode = message[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        */

        private void ReceiveMessages()
        {
            if (m_mode == 1)
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
                            OnMessageReceived(message); // 메시지를 받을 때 이벤트 발생
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            }
        }
        protected virtual void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }


        public string ReceiveNickname()
        {
            if (m_mode == 0)
            {
                try
                {
                    byte[] buffer = new byte[256];
                    int bytesRead;
                    int mode;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        return message;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 관리되는 자원 해제
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

                // 관리되지 않는 자원 해제
                // ...

                disposed = true;
            }
        }
        // 메시지 서버로 전송
        public void SendMessage(string playerNickname, string chat)
        {
            try
            {
                string message = $"chat:{playerNickname}:{chat}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // 메시지 서버로 전송
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

        // 소멸자
        ~Network()
        {
            Dispose(false);
        }
    }
}
