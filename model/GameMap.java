package moe.heing.server.model;

import java.util.*;
import java.io.*;

public class GameMap {
    public long timestamp;
    public ArrayList<PlayerMap> playerMapList = new ArrayList<>();
    public void addPlayerMap()
    private class PlayerMap {
        public int playerid;
        public String x;
        public String y;
        public PlayerMap() {
            playerid = -1;
            x = "";
            y = "";
        }
        public PlayerMap(int _id, String _x, String _y) {
            playerid = _id;
            x = _x;
            y = _y;
        }
    }
}
