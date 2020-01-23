using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class FishPlayer : MonoBehaviour
{
    public int horDirection = 0;
    public int lastHorDir = 0;
    private const int m_times = 6;

    public int verDirection = 0;
    public GameObject catBottom;
    public GameObject gameOverGO;
    public Animator animator;

    public LayerMask bottomLayerMask;
    public float myDistance = 1F;

    public GameObject bubbleGO;
    public GameObject fishwin;
     
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
        bubbleGO.transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
        bubbleGO.SetActive(true);
    }
    public void moveVer(int _direction)
    {
        verDirection = _direction; 
    }
     
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    void animControll()
    {
        if (horDirection != 0 || verDirection != 0)
        {
            animator.SetBool("moving", true);
        } else
        {
            animator.SetBool("moving", false);
        }
        if (lastHorDir > 0)
        {
            if (transform.localScale.x > 0) { 
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        } else
        {
            if (transform.localScale.x < 0)
            { 
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void moveControll()
    {
        // 限制不移出边界
        Vector2 s = new Vector2(transform.position.x, transform.position.y);

        bool castDown = Physics2D.Linecast(s, new Vector2(s.x, s.y - myDistance), bottomLayerMask);
        bool castUp = Physics2D.Linecast(s, new Vector2(s.x, s.y + myDistance), bottomLayerMask);
        bool castLeft = Physics2D.Linecast(s, new Vector2(s.x - myDistance, s.y), bottomLayerMask);
        bool castRight = Physics2D.Linecast(s, new Vector2(s.x + myDistance, s.y), bottomLayerMask);

        if (horDirection != 0)
        {
            if (horDirection > 0)
            {
                if (!castRight)
                {
                    transform.Translate(horDirection * m_times * Time.deltaTime, 0, 0);
                }
            }
            else
            {
                if (!castLeft)
                {
                    transform.Translate(horDirection * m_times * Time.deltaTime, 0, 0);
                }
            }
        }
        if (verDirection != 0)
        { 
            if (verDirection > 0)
            {
                if (!castUp)
                {
                    transform.Translate(0, verDirection * m_times * Time.deltaTime, 0);
                }
            }
            else
            {
                if (!castDown)
                { 
                    transform.Translate(0, verDirection * m_times * Time.deltaTime, 0);
                }
            }
        } 
    }
    // Update is called once per frame
    void Update()
    {
        moveControll(); 

        if (transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        animControll();

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "fork")
        {
            Debug.Log("crash");
            gameOverGO.SetActive(true);
            MyPlayer.tcpClient.sendMsg(CodeConfig.GAME_OVER, ""); 
            MyPlayer.tcpClient.quitGame();
        }
    }
}
