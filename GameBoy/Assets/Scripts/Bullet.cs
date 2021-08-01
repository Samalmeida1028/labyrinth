using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hit;

void OnCollisionEnter2D(Collision2D collision){
    Debug.Log("HIT");
    GameObject effect = Instantiate(hit, transform.position, Quaternion.identity);
    Destroy(gameObject);
    Destroy(effect, 5f);
}
}
