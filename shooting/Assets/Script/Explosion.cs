using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void OnEnable()
    {
        Invoke("OnDisable", 2.0f);
    }
    public void OnDisable()
    {
        gameObject.SetActive(false);
    }
    public void StartExplosion(string target) 
    {
        anim.SetTrigger("Explosion");
        switch (target) {
            case "S":
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "L":
            case "P":
                transform.localScale = Vector3.one * 1.0f;
                break;
            case "B":
                transform.localScale = Vector3.one * 3.0f;
                break;
        
        }
    }
}
