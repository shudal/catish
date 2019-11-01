package moe.heing.server;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;

import moe.heing.server.config.UrlConfig;
import moe.heing.server.util.*;
public class Main {
    public static int  maxn = 1024;
    public static void main(String[] args) {
        System.out.println("启动ing");
        TcpServer tcpServer = new TcpServer(UrlConfig.TCP_PORT);
    }
}
