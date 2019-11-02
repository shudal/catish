package moe.heing.server.model;

import java.lang.Integer;
public class MyMsg {
    public int playerid;
    public int type;
    public String msg = "";
    public MyMsg(int _id, int _type, String _msg) {
        playerid = _id;
        type = _type;
        msg = _msg;
    }
    public MyMsg(String msg) {
        int i=0;
        for(; i<msg.length() && msg.charAt(i++) != ':';);
        String idStr= "";
        for (; i<msg.length(); i++) {
            if (msg.charAt(i) != ',') {
                idStr += msg.charAt(i);
            } else {
                break;
            }
        }

        for (; i<msg.length() && msg.charAt(i++) != ':';);
        String typeStr = "";
        for(; i<msg.length(); i++) {
            if (msg.charAt(i) != ',') {
                typeStr += msg.charAt(i);
            } else {
                break;
            }
        }

        for (; i<msg.length() && msg.charAt(i++) != ':';);
        String rmsg = "";
        for (i += 2; i<msg.length(); i++) {
            if (msg.charAt(i) != '\'') {
                rmsg += msg.charAt(i);
            } else {
                break;
            }
        }
        //System.out.println("playerid=" + idStr + ", tpe=" + typeStr + ",msg=" + rmsg);
        playerid = Integer.parseInt(idStr);
        type = Integer.parseInt(typeStr);
        this.msg = rmsg;
        System.out.println("MyMsg Constructor:" + this.toString());
    }

    public String toString() {
        return "{'playerid':" + playerid + ", 'type':" + type +",'msg': '" + msg + "' }";
    }
}
