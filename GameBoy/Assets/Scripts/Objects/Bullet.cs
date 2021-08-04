using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hit;
    public float time = 0;
    public float lifeTime = 3f;
    void Update(){
        time += Time.deltaTime;
        if(time>lifeTime){
            Destroy(gameObject);
        }

    }

void OnCollisionEnter2D(Collision2D collision){
    Debug.Log("HIT");
    //GameObject effect = Instantiate(hit, transform.position, Quaternion.identity); 
    Destroy(gameObject);
    //Destroy(effect, 5f);
}
}
