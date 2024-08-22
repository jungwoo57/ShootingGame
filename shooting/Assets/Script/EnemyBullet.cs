using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //public int dmg;
    public bool isRotate;

    private void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Player")
            gameObject.SetActive(false);

    }
}
