using JetBrains.Annotations;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy_1Prefab;
    public GameObject enemy_2Prefab;
    //public GameObject enemy_3Prefab;
    public GameObject enemyBulletAPrefab;
    public GameObject enemyBulletBPrefab;
    public GameObject Item_BoomPrefab;
    public GameObject Item_CoinPrefab;
    public GameObject Item_PowerPrefab;
    public GameObject PlayerBulletAPrefab;
    public GameObject PlayerBulletBPrefab;
    public GameObject FollowerBulletPrefab;
    public GameObject BossBulletPrefab;
    public GameObject EnemyBossPrefab;
    public GameObject ExplosionPrefab;



    GameObject[] enemy_1;
    GameObject[] enemy_2;
    //GameObject[] enemy_3;
    GameObject[] enemyBulletA;
    GameObject[] enemyBulletB;
    GameObject[] Item_Boom;
    GameObject[] Item_Coin;
    GameObject[] Item_Power;
    GameObject[] PlayerBulletA;
    GameObject[] PlayerBulletB;
    GameObject[] targetPool;
    GameObject[] followerBullet;
    GameObject[] BossBullet;
    GameObject[] EnemyBoss;
    GameObject[] Explosion;
    private void Awake()
    {
        enemy_1 = new GameObject[20];
        enemy_2 = new GameObject[20];
        //enemy_3 = new GameObject[20];
        enemyBulletA = new GameObject[40];
        enemyBulletB = new GameObject[40];
        Item_Boom = new GameObject[20];
        Item_Coin = new GameObject[20];
        Item_Power = new GameObject[20];
        PlayerBulletA = new GameObject[100];
        PlayerBulletB = new GameObject[100];
        followerBullet = new GameObject[100];
        BossBullet = new GameObject[500];
        EnemyBoss = new GameObject[5];
        Explosion = new GameObject[30];
        Generate();

    }
    void Generate()
    {
        for (int index = 0; index < enemy_1.Length; index++)
        {
            enemy_1[index] = Instantiate(enemy_1Prefab);
            enemy_1[index].SetActive(false);
        }
        for (int index = 0; index < enemy_2.Length; index++)
        {
            enemy_2[index] = Instantiate(enemy_2Prefab);
            enemy_2[index].SetActive(false);
        }
        for (int index = 0; index < enemyBulletA.Length; index++)
        {
            enemyBulletA[index] = Instantiate(enemyBulletAPrefab);
            enemyBulletA[index].SetActive(false);
        }
        for (int index = 0; index < enemyBulletB.Length; index++)
        {
            enemyBulletB[index] = Instantiate(enemyBulletBPrefab);
            enemyBulletB[index].SetActive(false);
        }
        for (int index = 0; index < Item_Boom.Length; index++)
        {
            Item_Boom[index] = Instantiate(Item_BoomPrefab);
            Item_Boom[index].SetActive(false);
        }
        for (int index = 0; index < Item_Coin.Length; index++)
        {
            Item_Coin[index] = Instantiate(Item_CoinPrefab);
            Item_Coin[index].SetActive(false);
        }
        for (int index = 0; index < Item_Power.Length; index++)
        {
            Item_Power[index] = Instantiate(Item_PowerPrefab);
            Item_Power[index].SetActive(false);
        }
        for (int index = 0; index < PlayerBulletA.Length; index++)
        {
            PlayerBulletA[index] = Instantiate(PlayerBulletAPrefab);
            PlayerBulletA[index].SetActive(false);
        }
        for (int index = 0; index < PlayerBulletB.Length; index++)
        {
            PlayerBulletB[index] = Instantiate(PlayerBulletBPrefab);
            PlayerBulletB[index].SetActive(false);
        }
        for (int index = 0; index < followerBullet.Length; index++) {
            followerBullet[index] = Instantiate(PlayerBulletBPrefab);
            followerBullet[index].SetActive(false);
        }
        for (int index = 0; index < BossBullet.Length; index++) {
            BossBullet[index] = Instantiate(BossBulletPrefab);
            BossBullet[index].SetActive(false);
        }
        for (int index = 0; index < EnemyBoss.Length; index++){
            EnemyBoss[index] = Instantiate(EnemyBossPrefab);
            EnemyBoss[index].SetActive(false);
        }
        for (int index = 0; index < Explosion.Length; index++)
        {
            Explosion[index] = Instantiate(ExplosionPrefab);
            Explosion[index].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "enemy_1":
                targetPool = enemy_1;
                break;
            case "enemy_2":
                targetPool = enemy_2;
                break;
            case "enemyBulletA":
                targetPool = enemyBulletA;
                break;
            case "enemyBulletB":
                targetPool = enemyBulletB;
                break;
            case "Item_Boom":
                targetPool = Item_Boom;
                break;
            case "Item_Power":
                targetPool = Item_Power;
                break;
            case "Item_Coin":
                targetPool = Item_Coin;
                break;
            case "PlayerBulletA":
                targetPool = PlayerBulletA;
                break;
            case "PlayerBulletB":
                targetPool = PlayerBulletB;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "BossBullet":
                targetPool = BossBullet;
                break;
            case "EnemyBoss":
                targetPool = EnemyBoss;
                break;
            case "Explosion":
                targetPool = Explosion;
                break;
        }
        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }
        return null;
    }
    public GameObject[] GetPool(string type) {

        switch (type)
        {
            case "enemy_1":
                targetPool = enemy_1;
                break;
            case "enemy_2":
                targetPool = enemy_2;
                break;
            case "enemyBulletA":
                targetPool = enemyBulletA;
                break;
            case "enemyBulletB":
                targetPool = enemyBulletB;
                break;
            case "Item_Boom":
                targetPool = Item_Boom;
                break;
            case "Item_Power":
                targetPool = Item_Power;
                break;
            case "Item_Coin":
                targetPool = Item_Coin;
                break;
            case "PlayerBulletA":
                targetPool = PlayerBulletA;
                break;
            case "PlayerBulletB":
                targetPool = PlayerBulletB;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "BossBullet":
                targetPool = BossBullet;
                break;
            case "EnemyBoss":
                targetPool = EnemyBoss;
                break;
            case "Explosion":
                targetPool = Explosion;
                break;
        }
        return targetPool;
    }
}

