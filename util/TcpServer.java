package moe.heing.server.util;

import java.lang.Integer;
import java.util.Iterator;
import java.io.IOException;
import java.net.InetAddress;
import java.io.*;
import java.net.*;
import java.util.ArrayList;

import moe.heing.server.config.UrlConfig;
import moe.heing.server.model.PlayerClient;

public class TcpServer {
    public ServerSocket serverSocket;
    public int connectPort;
    public int  maxn = 1024;
    public int threadCount = 0;
    public ThreadGroup clientThreadGroup;
    public ArrayList<ServerThread> clientThreads = new ArrayList<>();
    public TcpServer(int port) {
        connectPort = port;
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
                String threadName = (++threadCount) + "";
                ServerThread aThread = new ServerThread(clientThreadGroup, threadName, s);
                clientThreads.add(aThread);
                clientThreads.get(clientThreads.size()-1).start();

            }
	    } catch (Exception e) {
            e.printStackTrace();
        }
    }
    public void sendAll(String msg) {
        for (ServerThread aThread : clientThreads ) {
            aThread.sendMsgStr(msg);
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
            System.out.println("new client");
            try {
                in = new BufferedReader(new InputStreamReader(s.getInputStream()));
                out = new PrintWriter(s.getOutputStream());
                String msg;
                while (!(msg = in.readLine()).equals("quit")) {
                    System.out.println("#" + msg);
                    sendAll(msg);
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
