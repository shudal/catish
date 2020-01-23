using System; 
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine; 
public class TcpClient : MonoBehaviour {

    IPEndPoint endp;
    Socket clientSocket;
    const int maxn = 1024;
    Queue<MyJson> msgs = new Queue<MyJson>();
    public void quitGame()
    {
        new Thread(() =>
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                SB.Append("quit");
                SB.Append("\n");
                byte[] buffer = Encoding.UTF8.GetBytes(SB.ToString());
                clientSocket.Send(buffer);
            }
            catch (Exception e)
            {

            }
        }).Start();
    }
    public void sendMsg(int type, string msg)
    {
        new Thread(() =>
        { 
            try
            {
                StringBuilder SB = new StringBuilder();
                SB.Append(MyPlayer.roomid);SB.Append("|");
                SB.Append(type);SB.Append("|");
                SB.Append(msg);   
                SB.Append("\n"); 
                byte[] buffer = Encoding.UTF8.GetBytes(SB.ToString());
                clientSocket.Send(buffer);
            }
            catch (Exception e)
            {

            }
        }).Start();
    }
    
    private void HandleMyJson(MyJson myjson)
    {
        MyPlayer.otherPlayerManager.HandleMsg(myjson);
    }
    private void ReceiveMsg()
    {
        while (true)
        {
            try
            {
                byte[] buffer = new byte[maxn * maxn];
                int n = clientSocket.Receive(buffer);
                if (n > 0)
                {
                    string msg = Encoding.UTF8.GetString(buffer, 0, n);
                    Debug.Log("@" + msg);
                    MyJson myjson = new MyJson(msg);
                    msgs.Enqueue(myjson);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
    private void Awake()
    {
        string domain = Config.SERVER_DOMAIN.Replace("http://", "").Replace("https://", "");
        IPHostEntry hostEntry = Dns.GetHostEntry(domain);
        endp = new IPEndPoint(hostEntry.AddressList[0], Config.SERVER_TCP_PORT); 
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(endp);
        Thread receiveThread = new Thread(ReceiveMsg);
        receiveThread.Start();
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        {
            sendMsg(CodeConfig.NEW_CLIENT_CAT, "");
        } else if (MyPlayer.playertype == Config.PLAYER_TYPE_FISH)
        {
            sendMsg(CodeConfig.NEW_CLIENT_FISH, "");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (msgs.Count > 0)
        { 
            HandleMyJson((msgs.Dequeue()));
        }
    }
    private void OnDestroy()
    {
        quitGame();
    }
}
