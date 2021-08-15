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
        }else if(gameObject.layer == 12)
        {
            gameObject.GetComponent<MinotaurScript>().isDamaged = true;
            if(gameObject.GetComponent<MinotaurScript>().stageTwo&&!gameObject.GetComponent<MinotaurScript>().isAttacking)
            {
                gameObject.GetComponent<MinotaurScript>().isDamaged = false;
            }
        }

        gameObject.GetComponent<HittableStats>().health -= damage;
        
        if(health<=0){
            if (gameObject.layer == 10)
            {
                GetComponent<EnemyScript>().isKilled = true;
            }else if(gameObject.layer == 12)
            {
                GetComponent<MinotaurScript>().isKilled = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
