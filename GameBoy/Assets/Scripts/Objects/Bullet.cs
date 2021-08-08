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

}
