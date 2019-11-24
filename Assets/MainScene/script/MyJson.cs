using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MyJson
{
    public string roomid;
    public int type;
    public string msg;
    public MyJson(MyJson _myJson)
    {
        roomid = _myJson.roomid;
        type = _myJson.type;
        msg = _myJson.msg;
    } 
    public MyJson(string j)
    {
        int i = 0;
        StringBuilder roomidSB = new StringBuilder();
        for (; j[i] != '|'; ++i)
        {
            roomidSB.Append(j[i]);
        }
        roomid = roomidSB.ToString();

        StringBuilder typeSB = new StringBuilder();
        for (++i; j[i] != '|'; ++i) {
            typeSB.Append(j[i]);
        } 
        type = Convert.ToInt32(typeSB.ToString());

        StringBuilder msgSB = new StringBuilder();
        for (++i; i<j.Length; ++i)
        {
            msgSB.Append(j[i]);
        }
        msg = msgSB.ToString(); 
        Debug.Log(this.ToString());
    } 

    public override
    string ToString()
    {
        return "roomid=" + roomid + ", type=" + type + ", msg=" + msg;
    }
}
