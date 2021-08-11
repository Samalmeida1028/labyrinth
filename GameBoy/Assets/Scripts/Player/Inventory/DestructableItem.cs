using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableItem : MonoBehaviour
{
    public int health;
    public GameObject coin;
    public bool isDestroyed= false;
    public ParticleSystem particle;

    void Start(){
        GetComponent<HittableStats>().health = 50;
    }
    void OnCollisionEnter2D(Collision2D other){
        Transform selfPos = gameObject.transform;
        if(other.gameObject.tag == "AttackType"){
        int randomAmount = 0;
        int spawnChance = Random.Range(1,10);
        if(spawnChance<=3)
        {   
            randomAmount=spawnChance;
        }   
        Vector3 position = transform.position;
        for(int i = 0; i<randomAmount; i++){
            Vector3 random = new Vector2(Random.Range(0,2),Random.Range(0,2));
            Instantiate(coin,transform.position+random,Quaternion.identity);
        }
        if(randomAmount>0) Instantiate(particle,transform.position,Quaternion.identity);
        Destroy(gameObject);
        }
    }
}
