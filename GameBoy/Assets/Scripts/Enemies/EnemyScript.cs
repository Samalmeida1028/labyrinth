using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyScript : MonoBehaviour
{
    public float speed = 100f;
    public Camera cam;
    public Rigidbody2D enemyrb;
    public int health = 100;
    [SerializeField] Transform target;
    NavMeshAgent agent;
    public float updateTime;
    public float tempTime;
    public float waitTime;
    public int attackRange;

    public int enemyType;

    public bool canShoot;
void Start(){
     agent = GetComponent<NavMeshAgent>();
     agent.updateRotation = false;
     agent.updateUpAxis = false;
     agent.angularSpeed = 100;
     agent.enabled = false;
     float tempUpdateTime = updateTime;
}

void Update(){
    tempTime -= Time.deltaTime;
    if(tempTime <= 0 && agent.enabled){
        
        PointAtPlayer();
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position,attackRange);
        foreach(Collider2D col in array){
            if(col.tag == "Player"){
                Attack();
            }
        }
        tempTime = updateTime;
    if(agent.enabled){
        agent.SetDestination(target.position);
    }
    }



}


void OnTriggerEnter2D(Collider2D other){
    Debug.Log(other.tag);
if(other.tag == "Player"){
    target = other.GetComponent<Transform>();
    agent.enabled = true;
}

}

void Attack(){
    agent.enabled = false;
    tempTime = waitTime;
    while(tempTime >0){
        tempTime-= Time.deltaTime;
        Debug.Log(tempTime);
    }
    agent.enabled = true;
    tempTime = updateTime;


}


void PointAtPlayer(){
    Vector2 lookDir = target.position - transform.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        GetComponent<Rigidbody2D>().rotation = angle;
}

void Move(){


    
}

}

