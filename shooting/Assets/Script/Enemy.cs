using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;

    public float speed;
    public float HP;
    public float maxShotDelay;
    public float curShotDelay;

    public int enemyScore;
    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    public Sprite[] sprites;

   // public GameObject bulletA;
    //public GameObject bulletB;
    public GameObject player;
    public GameManager gameManager;
   // public GameObject itemBoom;
    //public GameObject itemCoin;
    //public GameObject itemPower;
    
    public ObjectManager objManager;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;

    private void Awake()
    {
        if (enemyName == "B")
            anim = GetComponent<Animator>();
        else
            spriteRenderer = GetComponent<SpriteRenderer>();
       
    }
    void OnEnable() 
    {
        switch (enemyName) {

            case "L":
                HP = 4;
                break;
            case "S":
                HP = 1;
                break;
            case "B": 
                HP = 1000;
                Invoke("Stop",2);
                break;
        }
    }

    void Update()
    {
        if (enemyName == "B")
        {
            return;

        }
        else
        {
            Fire();
            Reload();
        }
    }
   
    public void onHit(int dmg)
    {
        if (HP <= 0)
            return;
        HP -= dmg;
        if (enemyName == "B") 
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }
        if (HP <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            gameManager.CallExplosion(transform.position, enemyName);
                //랜덤 아이템 드랍
            int ran = enemyName == "B" ? 1 :Random.Range(0, 100);
            if (ran % 7 == 0)
            {
                CreateItem(ran);
             }
             else Debug.Log("아이템이 드롭되지않았습니다.");
            if (enemyName == "B") {
                gameManager.StageEnd();    
            }
        }

        
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder" && enemyName != "B")
        {
            gameObject.SetActive(false) ;
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "Player_bullet") {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            onHit(bullet.dmg);

            collision.gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "S") {
            GameObject bulletA = objManager.MakeObj("enemyBulletA");
                bulletA.transform.position = player.transform.position - transform.position;
                Rigidbody2D rigid = bulletA.GetComponent<Rigidbody2D>();
                Vector3 dirVec = player.transform.position - transform.position;
                rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
            }
        else if (enemyName == "L") {
                GameObject bulletR = objManager.MakeObj("enemyBulletB");
                GameObject bulletL = objManager.MakeObj("enemyBulletB");
                bulletR.transform.position = player.transform.transform.position + Vector3.right * 0.3f;
                bulletL.transform.position = player.transform.transform.position + Vector3.left * 0.3f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                Vector3 dirVecR = player.transform.position - (transform.position - Vector3.right*0.3f);
                Vector3 dirVecL = player.transform.position - (transform.position- Vector3.left * 0.3f);
                rigidR.AddForce(dirVecR.normalized * 3, ForceMode2D.Impulse);
                rigidL.AddForce(dirVecL.normalized* 3, ForceMode2D.Impulse);
              }
        curShotDelay = 0;
        
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void CreateItem(int ran)

    {
        switch (ran % 5)
        {
            case 4:
                GameObject ItemPower = objManager.MakeObj("Item_Power");
                ItemPower.transform.position = transform.position;
                Rigidbody2D rigid = ItemPower.GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.down * 3;
                break;
            case 3:
                GameObject ItemBoom = objManager.MakeObj("Item_Boom");
                ItemBoom.transform.position = transform.position;
                Rigidbody2D rigid2 = ItemBoom.GetComponent<Rigidbody2D>();
                rigid2.velocity = Vector2.down * 3;
                break;
            default:
                GameObject ItemCoin = objManager.MakeObj("Item_Coin");
                ItemCoin.transform.position = transform.position;
                Rigidbody2D rigid3 = ItemCoin.GetComponent<Rigidbody2D>();
                rigid3.velocity = Vector2.down * 3;
                break;
        }
    }
    void Stop()
    {
        if (!gameObject.activeSelf)
            return;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        Invoke("Think" , 5);
    }

    void Think() 
    {
        if (HP <= 0)
            return;
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;
        switch (patternIndex) 
        {
            case 0:
                BossPattern0();
                break;
            case 1:
                BossPattern1();
                break;
            case 2:
                BossPattern2();
                break;
            case 3:
                BossPattern3();
                break;
        
        }
    }
    void BossPattern0() 
    {
        GameObject bulletR = objManager.MakeObj("BossBullet");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objManager.MakeObj("BossBullet");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletL = objManager.MakeObj("BossBullet");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objManager.MakeObj("BossBullet");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletL.GetComponent<Rigidbody2D>();
       
        rigidR.AddForce(Vector2.down * 3, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 3, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 3, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 3, ForceMode2D.Impulse);


        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("BossPattern0", 2.0f);
        }
        else
            Invoke("Think", 2.0f);
    }

    void BossPattern1()
    {
        curPatternCount++;
        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = objManager.MakeObj("BossBullet");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirvec = player.transform.position - transform.position;
            Vector2 ranvec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirvec += ranvec;
            rigid.AddForce(dirvec.normalized * 3, ForceMode2D.Impulse);
        }

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("BossPattern1", 2.0f);
        }
        else
            Invoke("Think", 2.0f);
    }

    void BossPattern2()
    {
        GameObject bullet = objManager.MakeObj("BossBullet");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI*5*curPatternCount / maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("BossPattern2", 2.0f);
            
        }
        else
            Invoke("Think", 2.0f);
    }

    void BossPattern3() {
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;
        for (int index = 0; index < roundNum; index++) 
        {
            GameObject bullet = objManager.MakeObj("BossBullet");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 5 * index / roundNum), Mathf.Cos(Mathf.PI * 5 * index / roundNum));
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
            Vector3 rotVec = Vector3.forward * 360 * index / roundNum+Vector3.forward*90;
            bullet.transform.Rotate(rotVec); 
            
           
        }

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("BossPattern3", 2.0f);
        }
        else
            Invoke("Think", 2.0f);
    }

}
