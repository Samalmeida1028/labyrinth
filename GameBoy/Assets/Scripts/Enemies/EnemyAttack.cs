using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;

    public void SetDamage(int enemyDamage){
        damage = enemyDamage;
        Debug.Log(damage);
    }
    void OnCollisionEnter2D(Collision2D other){
        Destroy(gameObject);
    }

}
