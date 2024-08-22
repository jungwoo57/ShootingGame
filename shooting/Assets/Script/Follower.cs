using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;

    public ObjectManager objManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }
    void Update()
    {
        watch();
        follow();
        Fire();
        Reload();
    }
    void follow()
    {
        transform.position = followPos;
    }
    void Fire() {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay)
            return;
        GameObject bullet = objManager.MakeObj("followerBullet");
        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        curShotDelay = 0;
    } 
    void Reload() {
        curShotDelay += Time.deltaTime;
    }
    void watch() 
    {
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);
        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
        
    }
}