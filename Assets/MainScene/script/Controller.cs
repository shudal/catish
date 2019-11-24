using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject up;
    public GameObject down;
    public GameObject skill;
    public GameObject fiskill;
    // Start is called before the first frame update
    public void myInit()
    { 
        if (MyPlayer.playertype == Config.PLAYER_TYPE_CAT)
        {
            up.SetActive(false);
            down.SetActive(false);
        } else if (MyPlayer.playertype == Config.PLAYER_TYPE_FISH)
        {
            skill.SetActive(false);
            fiskill.SetActive(true);
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
