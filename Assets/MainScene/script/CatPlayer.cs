using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayer : MonoBehaviour
{
    public int horDirection = 0;
    public int lastHorDir = 0;
    private const int m_times = 6;
    public GameObject forkGO;
    public float forkDown = 0.3F;
     
    public void moveHor(int _direction)
    {
        horDirection = _direction;
        if (_direction != 0)
        {
            lastHorDir = _direction;
        }
    }
    public void skill()
    {

        MyPlayer.playerGO.GetComponent<Animator>().Play("fork");
        forkGO.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        forkGO.transform.position = new Vector3(transform.position.x + 0.2F, transform.position.y - 1.8F, transform.position.z);
        /*
        if (forkCount < maxForkCount)
        {
            GameObject aFork = (GameObject)Resources.Load("prefab/fork");
            aFork.transform.position = new Vector3(forkGO.transform.position.x, forkGO.transform.position.y - forkDown, 0);
            aFork.AddComponent<Rigidbody2D>();
            aFork.name = "fork" + (++forkCount);
            Instantiate(aFork);
        } else
        {
            for (int i=1; i<=maxForkCount; ++i)
            {
                if (i == forkIndex)
                {
                    GameObject aFork = GameObject.Find("fork" + forkIndex + "(Clone)");
                    aFork.transform.position = new Vector3(forkGO.transform.position.x, forkGO.transform.position.y - forkDown, 0);
                    if ((++forkIndex) == (maxForkCount + 1))
                    {
                        forkIndex = 1;
                    }
                    break;
                }
            }
        }
        //MyPlayer.playerGO.GetComponent<Animator>().SetBool("fork", false);
        */
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (horDirection != 0)
        {
            transform.Translate(horDirection * m_times * Time.deltaTime, 0, 0);
        }
        if (!(transform.rotation.eulerAngles.z == 0))
        { 
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
