using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public ParticleSystem onShot;
    public void SetDamage(int totalDamage){
        damage = totalDamage;

        Debug.Log(totalDamage);

    }
    void Start(){
        Instantiate(onShot,transform.position,Quaternion.identity);
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<HittableStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag != "Coin"){
            Destroy(gameObject);
        }

    }

}
