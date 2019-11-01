package moe.heing.server.model;

import moe.heing.serber.config.CodeConfig;
public class PlayerClient {
    public String ip;
    public int status;
    public PlayerClient(String _ip) {
        ip = _ip;
        status = CodeConfig.STATUS_PLAYER_CLIENT_NORMAL;
    }
}
