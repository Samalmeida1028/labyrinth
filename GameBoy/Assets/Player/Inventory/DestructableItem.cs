using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DestructableItem : MonoBehaviour
{
    public int health;
    public GameObject coin;
    public GameObject ammo;
    public ParticleSystem particleGold;
    public ParticleSystem particleNoGold;
    public AudioClip gold;
    public AudioClip noGold;


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
            Vector3 random = new Vector2(Random.Range(0,.5f),Random.Range(0,.5f));
            Instantiate(coin,transform.position+random,Quaternion.identity);
            if(i%2==0)
            {
                Instantiate(ammo,transform.position+random,Quaternion.identity);
            }
        }
        if(randomAmount>0){
            FindObjectOfType<AudioManager>().Play("VaseBreak");
            Instantiate(particleGold,transform.position,Quaternion.identity);
            GetComponent<AudioSource>().PlayOneShot(gold);
        }
        else {
            FindObjectOfType<AudioManager>().Play("VaseBreak");
            Instantiate(particleNoGold,transform.position,Quaternion.identity);
            GetComponent<AudioSource>().PlayOneShot(noGold);
        }
        Destroy(gameObject);
        //AstarPath.active.Scan();
        }
    }
}
