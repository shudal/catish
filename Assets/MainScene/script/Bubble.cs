using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{ 
    public GameObject gameOver;
    public GameObject bubble;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "cat")
        {
            Debug.Log("bubble crash cat");
            GameObject.FindGameObjectWithTag("fish").GetComponent<FishPlayer>().fishwin.SetActive(true);
            MyPlayer.tcpClient.sendMsg(CodeConfig.FISH_WIN, "");
        }
        if (collision.collider.name == "wateraround")
        {
            Debug.Log("crash");
            bubble.SetActive(false);
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
