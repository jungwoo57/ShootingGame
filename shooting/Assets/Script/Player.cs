using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxShotDelay;
    public float curShotDelay;

    public int power;
    public int life;
    public int score;
    public int maxPower;
    public int boom;
    public int maxBoom;
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;
    public bool isHit;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;


    public GameObject bulletA;
    public GameObject bulletB;
    public GameObject Ulteffect;
    public GameManager gameManager;
    public ObjectManager objManager;

    public GameObject[] followers;

    public bool isBoomTime;
    public bool isRespawnTime;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake() 
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 2);
    }
    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;
        if (isRespawnTime)
        { 
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            for (int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
            }
        }
        else {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            for (int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }
    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
        AddFollower();
    }

    public void JoyPanel(int type)
    {
        for (int index = 0; index < 9; index++) {
            joyControl[index] = index == type;
        }
    }
    
    public void JoyDown() 
    {
        isControl = true;     
    }
    public void JoyUp() 
    {
        isControl = false;
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v =0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }
        if ((h == -1 && isTouchLeft) || (h == 1 && isTouchRight) || !isControl)
        {
            h = 0;
        }
        if ((v == -1 && isTouchBottom) || (v == 1 && isTouchTop)|| !isControl)
        {
            v = 0;
        }
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || (Input.GetButtonUp("Horizontal")))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    public void ButtonADown() 
    {
        isButtonA = true;
    }
    public void ButtonAUp()
    {
        isButtonA = false;
    }

    public void ButtonBDown() 
    {
        isButtonB = true;
    }
    void Fire() 
    {

        if (curShotDelay < maxShotDelay)
           return;
        if (!isButtonA) return;

        if (isButtonA == true)
        {
            switch (power)
            {
                case 1:
                    GameObject bullet = objManager.MakeObj("PlayerBulletA");
                    bullet.transform.position = transform.position;
                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                    rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    break;
                case 2:
                    GameObject bulletR = objManager.MakeObj("PlayerBulletA");
                    GameObject bulletL = objManager.MakeObj("PlayerBulletA");
                    Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                    rigidR.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    rigidL.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    break;
                case 3:
                    GameObject bulletb = objManager.MakeObj("PlayerBulletB");
                    Rigidbody2D rigidB = bulletb.GetComponent<Rigidbody2D>();
                    rigidB.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    break;
                default:
                    GameObject bulletc = objManager.MakeObj("PlayerBulletB");
                    Rigidbody2D rigidc = bulletc.GetComponent<Rigidbody2D>();
                    rigidc.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    break;

            }
        }
        
       // else return;

        curShotDelay = 0;
    }

    void Reload() 
    {
        curShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy_bullet" || collision.gameObject.tag=="Enemy")
        {
            if (isRespawnTime)
                return;
            if (isHit) {
                return;
            }

            isHit = true;
            life--;
            gameManager.UpdateLifeIcon(life);
            gameManager.CallExplosion(transform.position, "p");
            if (life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.PlayerRespawnexe();
            }
            gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Item") {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type) 
            {
                case "Coin":
                    score += 20;
                    break;
                case "Boom":
                    if (boom < maxBoom)
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    else
                        score += 100;
                    break;
                case "Power":
                    if (power < maxPower)
                        power++;
                    else
                        score += 10;
                    break;
            }
            //Destroy(collision.gameObject);
            gameObject.SetActive(false);
          
        }
    }
    void OffUltEffect() {
        Ulteffect.SetActive(false);
        isBoomTime= false;
    }
    void Boom() {
        if (isButtonB == false)
            return;
        //if (Input.GetButton("Fire2"))
        if (isButtonB == true)
        {
            if (boom < 1)
                return;
            else
            {
                if (!isBoomTime)
                {
                    isBoomTime = true;
                    boom--;
                    gameManager.UpdateBoomIcon(boom);
                    Ulteffect.SetActive(true);
                    Invoke("OffUltEffect", 3f);
                    GameObject[] enemiesl = objManager.GetPool("enemy_1");
                    GameObject[] enemiesm = objManager.GetPool("enemy_2");
                    for (int index = 0; index < enemiesl.Length; index++)
                    { 
                        if (enemiesl[index].activeSelf)
                        {
                            Enemy enemyLogic = enemiesl[index].GetComponent<Enemy>();
                            enemyLogic.onHit(100);
                        }
                    }
                    for (int index = 0; index < enemiesm.Length; index++)
                    {
                        if (enemiesm[index].activeSelf)
                        {
                            Enemy enemyLogic = enemiesm[index].GetComponent<Enemy>();
                            enemyLogic.onHit(100);
                        }
                    }
                    GameObject[] bulletAs = objManager.GetPool("enemyBulletA");
                    GameObject[] bulletBs = objManager.GetPool("enemyBulletB");
                    for (int index = 0; index < bulletAs.Length; index++)
                    {
                        if (bulletAs[index].activeSelf)
                            bulletAs[index].SetActive(false);
                    }
                    for (int index = 0; index < bulletBs.Length; index++)
                    {
                        if (bulletBs[index].activeSelf)
                            bulletBs[index].SetActive(false);
                    }
                }
            }
        }

    }
    void AddFollower() 
    {
        if (power < 4)
            return;
        else if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        
    }
}
