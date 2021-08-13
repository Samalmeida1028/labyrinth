using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableStats : MonoBehaviour
{
    public int health;



    public void TakeDamage(int damage){
        GetComponent<EnemyScript>().isDamaged = true;

        gameObject.GetComponent<HittableStats>().health -= damage;
        if(health<=0){
            Destroy(gameObject);
        }
    }
}
