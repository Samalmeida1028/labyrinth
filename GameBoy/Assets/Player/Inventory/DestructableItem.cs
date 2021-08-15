using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DestructableItem : MonoBehaviour
{
    public GameObject coin;
    public GameObject ammo;
    public ParticleSystem particleGold;
    public ParticleSystem particleNoGold;
    public AudioClip gold;
    public AudioClip noGold;
    public bool isEnemy;


    public void spawnDrops()
    {
        int randomAmount = 0;
        int spawnChance = Random.Range(1,10);
        if(spawnChance<=3)
        {   
            randomAmount=spawnChance;
        }   
        Vector3 position = transform.position;
        for(int i = 0; i<randomAmount; i++){
            Vector3 random = new Vector2(Random.Range(0,.5f),Random.Range(0,.5f));
            Instantiate(coin,transform.position+random,Quaternion.identity);
            if(i%2==0)
            {
                if(!isEnemy)
                    Instantiate(ammo,transform.position+random,Quaternion.identity);
            }
        }
        if(randomAmount>0){
            if(!isEnemy)
                FindObjectOfType<AudioManager>().Play("VaseBreak");
                //GetComponent<AudioSource>().PlayOneShot(gold);
            Instantiate(particleGold,transform.position,Quaternion.identity);
        }
        else {
            if(!isEnemy)
                FindObjectOfType<AudioManager>().Play("VaseBreak");
                //GetComponent<AudioSource>().PlayOneShot(noGold);
            Instantiate(particleNoGold,transform.position,Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "AttackType"){
        if (!isEnemy)
        {
           spawnDrops();
        }

        if(!isEnemy)
            Destroy(gameObject);
        }
    }
}
