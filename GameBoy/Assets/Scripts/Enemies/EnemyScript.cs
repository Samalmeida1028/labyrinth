using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 3f;
    private Transform target;
    public Camera cam;
    public Rigidbody2D enemyrb;
    Rigidbody2D player = null;
    public int health = 100;


private void Update(){
    if(health<=0){
        Destroy(gameObject);
    }

}
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            GetComponent<AIDestinationSetter>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            canFollow = false;
            GetComponent<AIDestinationSetter>().enabled = false;
        }
    }

    public void PointToTarget(Rigidbody2D rb){
        Vector2 lookDir = rb.position - enemyrb.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg;
        enemyrb.rotation = angle;
    }

    public void OnCollisionEnter2D(Collision2D other){

        if(other.gameObject.tag == "Bullet"){
            health-=20;
        }
    }

}
