using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class AudioTcpClient : MonoBehaviour
{
    AudioChat audioChat;
    IPEndPoint endp;
    Socket clientSocket;
    const int maxn = 1024;
    Queue<byte[]> msgs = new Queue<byte[]>(); 
    public void sendMsg(int type, string msg)
    {
        new Thread(() =>
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                SB.Append(MyPlayer.roomid); SB.Append("|");
                SB.Append(type); SB.Append("|");
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
    public void sendByte(byte[] ba)
    {
        new Thread(() =>
        {
            try {
                // clientSocket.Send(ba);
                clientSocket.Send(Encoding.UTF8.GetBytes(Convert.ToBase64String(ba)));
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }).Start();
    }
    private void HandleMyJson(MyJson myjson)
    {
        MyPlayer.otherPlayerManager.HandleMsg(myjson);
    }
    private void HandleMyBytes(byte[] ba)
    {
        audioChat.playByte(ba);
    }
    private void ReceiveMsg()
    {
        while (true)
        {
            try
            {
                byte[] buffer = new byte[maxn];
                byte[] ba = new byte[maxn];
                int n = clientSocket.Receive(buffer);
                if (n > 0)
                { 
                    ba = Convert.FromBase64String(Encoding.UTF8.GetString(buffer, 0, n));
                    
                    msgs.Enqueue(ba);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
    private void ReceiveMsg2()
    {
        while (true)
        {
            try
            { 
                byte[] buffer = new byte[maxn];
                int n = clientSocket.Receive(buffer);
                if (n > 0)
                {
                    /*
                    ba = Convert.FromBase64String(Encoding.UTF8.GetString(buffer, 0, n));
                    */
                     
                    msgs.Enqueue(buffer); 
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
        endp = new IPEndPoint(hostEntry.AddressList[0], Config.AUDIO_TCP_PROT);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(endp);
        Thread receiveThread = new Thread(ReceiveMsg);
        receiveThread.Start();
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        {
            sendMsg(CodeConfig.NEW_AUDIO, "");
        }
        else if (MyPlayer.playertype == Config.PLAYER_TYPE_FISH)
        {
            sendMsg(CodeConfig.NEW_AUDIO, "");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioChat = GetComponent<AudioChat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (msgs.Count > 0)
        {
            HandleMyBytes((msgs.Dequeue()));
        }
    }
    private void OnDestroy()
    {

    } 
}
