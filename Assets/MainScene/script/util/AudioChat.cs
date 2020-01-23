using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine; 

public class AudioChat : MonoBehaviour
{
    public AudioSource aud;

    public AudioTcpClient audioTcpClient;

    public string micName;
    public int ti = 100;
    int lastSample;
    AudioClip c;
    int FREQUENCY = 44100;
    int theChan;

    // Start is called before the first frame update
    void Start()
    {
        audioTcpClient = GetComponent<AudioTcpClient>();
        aud = GetComponent<AudioSource>();
        string[] micArray = Microphone.devices; 
        if (micArray.Length == 0)
        {
            Debug.LogError("no mic device");
        } else
        {
            micName = micArray[0];
        }
        
       
        c = Microphone.Start(micName, true, ti, FREQUENCY);
        while (Microphone.GetPosition(micName) < 0) { }

        StartCoroutine(Rec());
    }
      
    IEnumerator Rec()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            int pos = Microphone.GetPosition(micName);
            int diff = pos - lastSample;
            if (diff > 0)
            {
                float[] samples = new float[diff * c.channels];
                c.GetData(samples, lastSample);
                byte[] ba = ToByteArray(samples);
                Send(ba, c.channels); ;
            }
            lastSample = pos;
        }
    } 

    public byte[] ToByteArray(float[] floatArray)
    {
        int len = floatArray.Length * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;
        foreach (float f in floatArray)
        {
            byte[] data = System.BitConverter.GetBytes(f);
            System.Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }
        return byteArray;
    }

    public float[] ToFloatArray(byte[] byteArray)
    {
        int len = byteArray.Length / 4;
        float[] floatArray = new float[len];
        for (int i = 0; i < byteArray.Length; i += 4)
        {
            try
            { 
                floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
            }
            catch
            {
            }
        }
        return floatArray;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void Send(byte[] ba, int chan)
    {
        theChan = chan;
        Debug.Log("theChan=" + theChan);
        // playByte(ba, chan); 
        new Thread(() =>
        {
            audioTcpClient.sendByte(ba);
        }).Start(); 
    }
    public void playByte(byte[] ba)
    { 
        float[] f = ToFloatArray(ba);
        aud.clip = AudioClip.Create("test", f.Length, theChan, FREQUENCY, false);
        aud.clip.SetData(f, 0);
        if (!aud.isPlaying) aud.Play(); 
    }
    public void playByte(byte[] ba, int chan)
    {
        float[] f = ToFloatArray(ba);
        aud.clip = AudioClip.Create("test", f.Length, chan, FREQUENCY, false);
        aud.clip.SetData(f, 0);
        if (!aud.isPlaying) aud.Play();
    }
    /*
    public void playByte2(byte[] ba, int chan)
    {
        StringBuilder SB = new StringBuilder();
        SB.Append(MyPlayer.roomid); SB.Append("|");
        SB.Append(CodeConfig.AUDIO); SB.Append("|");

        StringBuilder sb = new StringBuilder();
        sb.Append(chan + "|");
        sb.Append(Base32.ToBase32String(ba));

        SB.Append(sb.ToString());
        byte[] b1 = Encoding.UTF8.GetBytes(SB.ToString());
        string s2 = Encoding.UTF8.GetString(b1);
        MyJson myJson = new MyJson(s2);
        GameObject.FindGameObjectWithTag("OtherPlayerManagerGO").GetComponent<OtherPlayerManager>().HandleMsg(myJson);
      

    }
    
    public List<byte[]> SplitList(byte[] superbyte, int size)
    {
        List<byte[]> result = new List<byte[]>();
        int length = superbyte.Length;
        int count = length / size;
        int r = length % size;

        for (int i = 0; i < count; i++)
        {
            byte[] newbyte = new byte[size];
            newbyte = superbyte.Skip(size * i).Take(size).ToArray();// SplitArray(superbyte, size*i, size * i+ size);
            result.Add(newbyte);
        }


        if (r != 0)
        {
            byte[] newbyte = new byte[r];
            newbyte = superbyte.Skip(length - r).Take(r).ToArray();
            result.Add(newbyte);
        }

        return result;
    }
    */
}
