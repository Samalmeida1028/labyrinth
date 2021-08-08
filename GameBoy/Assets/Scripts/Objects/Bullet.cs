using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public void SetDamage(int totalDamage){
        damage = totalDamage;

        Debug.Log(totalDamage);

    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<HittableStats>().TakeDamage(damage);
            Destroy(gameObject);
        }

    }

}
