using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using System.IO;
using UnityEditor.Build;

public class GameManager : MonoBehaviour
{
    public int stage;

    public string[] enemyObjs;
    public Transform[] spawnPoints;
    public Transform playerPos;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public Text scoreText;
    public Image[] BoomImage;
    public Image[] LifeImage;
    public GameObject gameOverSet;
    public GameObject player;
    public ObjectManager objManager;

    public Animator startAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
 
    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;
    void Awake() {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "enemy_1", "enemy_2","EnemyBoss" };
        StageStart();
    }

    public void StageStart()
    {
        //Stage UI Load
        player.transform.position = playerPos.position;
        startAnim.SetTrigger("On");
        startAnim.GetComponent<Text>().text = "stage" + stage + "\nStart";
        clearAnim.GetComponent<Text>().text = "stage" + stage + "\nclear";
        fadeAnim.SetTrigger("In");

        ReadSpawnFile();

        //#Fade I
    }
    public void StageEnd()
    {
        //stage clear UI
        clearAnim.SetTrigger("On");
        //Fade Out
        fadeAnim.SetTrigger("Out");
        stage++;
        if (stage > 2)
            GameOver();
        else
            Invoke("StageStart",5);
    }
    void ReadSpawnFile() {

        //#1 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //#2 리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage" + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);
        
        while(stringReader != null) 
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);
            if (line == null)
                break;
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }
        stringReader.Close();
        nextSpawnDelay = spawnList[0].delay;

    }
    void Update()
    {

        curSpawnDelay += Time.deltaTime;
        if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {

        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type) 
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "B":
                enemyIndex = 2;
                break;
        }
        /* 랜덤소환
         int ranEnemy = Random.Range(0, 2);
         int ranPoint = Random.Range(0, 8);
        */
        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objManager = objManager;
        enemyLogic.gameManager = this;
        if (enemyPoint == 5 || enemyPoint == 7)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (1), -1);
        }
        else if (enemyPoint == 6 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));

        //리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count) {
            spawnEnd = true;
            return;
        }
        //다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life) {//life init
        for (int index = 0; index < 3; index++)
        {
            LifeImage[index].color = new Color(1, 1, 1, 0);
        }
        // life able
        for (int index = 0; index < life; index++)
        {
            LifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom) 
    {
        for (int index = 0; index < 3; index++)
        {
            BoomImage[index].color = new Color(1, 1, 1, 0);
        }
        // life able
        for (int index = 0; index < boom; index++)
        {
            BoomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void PlayerRespawnexe() {
        Invoke("RespawnPlayer", 2f);
    }

    void RespawnPlayer()
    {
        player.transform.position = Vector3.down * 4f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }
    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }
    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
    public void CallExplosion(Vector3 pos, string type) 
    {
        GameObject explosion = objManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }
}
