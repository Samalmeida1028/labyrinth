using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 3f;
    private Transform target;
    public Camera cam;
    public Rigidbody2D enemyrb;
    public int health = 100;



private void Update(){
    if(health<=0){
        Destroy(gameObject);
    }

}
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("Hello");
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("Bye");
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

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

