using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public float interval = 3f;
    public Rigidbody2D spawner;
    public float spawn = 0f;
    public GameObject enemy;
    public float time = 0;

    // Update is called once per frame
 void FixedUpdate(){
     time += Time.deltaTime;
     if (time >= spawn){

        Instantiate(enemy,spawner.position, Quaternion.identity);
         spawn = time + interval;
     }



 }


}
