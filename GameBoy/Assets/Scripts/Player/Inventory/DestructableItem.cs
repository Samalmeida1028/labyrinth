using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableItem : MonoBehaviour
{
    public int health;
    public GameObject coin;
    public bool isDestroyed= false;

    void Start(){
        GetComponent<HittableStats>().health = 50;
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "AttackType"){
        Vector3 random = new Vector3(Random.Range(1,4),Random.Range(1,4),Random.Range(1,4));
        Vector3 position = transform.position;
        Instantiate(coin,position+random,Quaternion.identity);
        }
    }
}
