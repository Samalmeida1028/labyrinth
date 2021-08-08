using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableItem : MonoBehaviour
{
    public int health;
    public GameObject coin;
    public bool isDestroyed= false;

    
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "AttackType"){
            TakeDamage(other.gameObject.GetComponent<Bullet>().damage);
        }

    }

    public void TakeDamage(int damage){
        Debug.Log("oww");
        health -= damage;
        if(health<=0){
            Instantiate(coin,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
