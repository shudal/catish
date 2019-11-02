package moe.heing.server.model;

public class GameMapStr {
    public long timestamp;
    public String mapStr;
    public GameMapStr(long timestamp, String mapStr) {
        this.timestamp = timestamp;
        this.mapStr = mapStr;
    }
}
