package moe.heing.server.util;

import java.lang.Integer;
import java.util.Iterator;
import java.io.IOException;
import java.net.InetAddress;
import java.io.*;
import java.net.*;
import java.util.*;
import java.text.SimpleDateFormat;
import java.util.ArrayList;


import moe.heing.server.config.CodeConfig;
import moe.heing.server.config.UrlConfig;
import moe.heing.server.model.MyMsg;
import moe.heing.server.model.GameMapStr;

public class TcpServer {
    public ServerSocket serverSocket;
    public int connectPort;
    public int  maxn = 1024;
    public int threadCount = 0;
    public ThreadGroup clientThreadGroup;
    public ArrayList<ServerThread> clientThreads = new ArrayList<>();
    public static GameMapStr gameMapStr;
    public SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
    public TcpServer(int port) {
        connectPort = port;
        long nowTimeLong = System.currentTimeMillis() / 1000;
        gameMapStr = new GameMapStr(nowTimeLong, "2,3.333002,1.74068,358.1939|1,-11.54934,-2.688283,3.479636|");
        init();
        listen();
    }
    public void init() {
        try {
            serverSocket = new ServerSocket(connectPort);
            clientThreadGroup = new ThreadGroup("clients");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    private  void listen() {
        try {
            while (true) {
                Socket s = serverSocket.accept();
                clientThreads.add(new ServerThread(clientThreadGroup, (++threadCount) + "", s));
                clientThreads.get(clientThreads.size()-1).start();

            }
	    } catch (Exception e) {
            e.printStackTrace();
        }
    }
    public void sendAll(String msg) {
        new Thread(()-> {
            for (ServerThread aThread : clientThreads) {
                aThread.sendMsgStr(msg);
            }
        }).start();
    }
    public void handleMsg(MyMsg mymsg) {
        if (mymsg.type == CodeConfig.TYPE_UPLOAD_MY_MAP) {
            long nowTimeLong = System.currentTimeMillis() / 1000;
            if (nowTimeLong > gameMapStr.timestamp) {
                gameMapStr.timestamp = nowTimeLong;
                gameMapStr.mapStr = mymsg.msg;
                System.out.println("存储game map");
            }
        } else if (mymsg.type == CodeConfig.TYPE_NEW_CLIENT) {
            sendAll(new MyMsg(CodeConfig.SERVER_PLAYER_ID, CodeConfig.TYPE_UPDATE_MAP, gameMapStr.mapStr).toString());
        }
    }
    protected class ServerThread extends Thread {
        Socket s;
        public BufferedReader in;
        public PrintWriter out;
        public ServerThread(ThreadGroup _threadGroup, String _threadName, Socket _s) {
            super(_threadGroup, _threadName);
            s = _s;
        }
        public void sendMsgStr(String msgStr) {
            out.println(msgStr);
            out.flush();
        }
        public void run() {
            try {
                in = new BufferedReader(new InputStreamReader(s.getInputStream()));
                out = new PrintWriter(s.getOutputStream());
                String msg;
                while (!(msg = in.readLine()).equals("quit")) {
                    String nowTimeStr = df.format(new Date());
                    System.out.println(nowTimeStr + "#\n    " + msg);
                    sendAll(msg);
                    if (msg.charAt(0) != '{') {
                        handleMsg(new MyMsg(msg));
                    }
                }
                in.close();
                out.close();
                s.close();

            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }
}
