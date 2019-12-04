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
    public SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
    public TcpServer(int port) {
        connectPort = port;
        long nowTimeLong = System.currentTimeMillis() / 1000;
        init();
        listen();
    }
    public void init() {
        try {
            serverSocket = new ServerSocket(connectPort);
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
                printStatus();
            }
	    } catch (Exception e) {
            e.printStackTrace();
        }
    }
    public void myRemove(String threadName) {
        // System.out.println("myRemove(" + threadName);
        new Thread(()->{
            System.out.println(threadName);
            Iterator<ServerThread> it = clientThreads.iterator();
            while (it.hasNext()) {
                ServerThread sh = it.next();
                if (sh.threadName.equals(threadName)) {
                    it.remove();
                    System.out.println("移除threadName=" + threadName);
                    System.out.println("        ,roomid=" + sh.roomid);
                    printStatus();
                    break;
                }
            }
        }).start();
    }
    public void sendAll(String roomid, String threadName, String msg) {
        new Thread(()-> {
            for (ServerThread aThread : clientThreads) {
                if (roomid.equals(aThread.roomid) && !threadName.equals(aThread.threadName)) {
                    aThread.sendMsgStr(msg);
                }
            }
        }).start();
    }
    private void printStatus() {
        System.out.println("    总连接线程数：" + clientThreads.size());
    }
    protected class ServerThread extends Thread {
        Socket s;
        public String roomid;
        public BufferedReader in;
        public PrintWriter out;
        public String threadName;
        public ServerThread(ThreadGroup _threadGroup, String _threadName, Socket _s) {
            super(_threadGroup, _threadName);
            s = _s;
            roomid = null;
            threadName = _threadName;
        }
        public void sendMsgStr(String msgStr) {
            out.println(msgStr);
            out.flush();
        }
        public void myclose() {
            // System.out.println("调用myclose方法");
            try {
                in.close();
                out.close();
                s.close();
                myRemove(threadName);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        public void run() {
            try {
                in = new BufferedReader(new InputStreamReader(s.getInputStream()));
                out = new PrintWriter(s.getOutputStream());
                String msg;
                while (true) {
                    msg = in.readLine();
                    if (msg.equals("quit")) {
                        System.out.println("收到退出信号");
                        myclose();
                        break;
                    }
                    String nowTimeStr = df.format(new Date());
                    System.out.println(nowTimeStr + "#\n    :" + msg);
                    if (roomid == null) {
                        int l = msg.length();
                        StringBuilder sb = new StringBuilder();
                        for (int i=0; msg.charAt(i) != '|'; ++i) {
                            sb.append(msg.charAt(i));
                        }
                        roomid = sb.toString();
                    }
                    sendAll(roomid, threadName, msg);
                }
                myclose();
            } catch (Exception e) {
                e.printStackTrace();
                myclose();
            }
        }
    }
}
