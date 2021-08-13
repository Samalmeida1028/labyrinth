using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableStats : MonoBehaviour
{
    public int health;


    public void TakeDamage(int damage){
        if (gameObject.layer == 10)
        {
            gameObject.GetComponent<EnemyScript>().isDamaged = true;
        }

        gameObject.GetComponent<HittableStats>().health -= damage;
        if(health<=0){
            if (gameObject.layer == 10)
            {
                GetComponent<EnemyScript>().isKilled = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
