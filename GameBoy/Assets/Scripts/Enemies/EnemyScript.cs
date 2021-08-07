using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.Utils;
public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Type")]
    [Space(5)]
    public int enemyType;
    public GameObject attackType;
    public Transform firePoint;
    [Space(10)]


    [Header("Update for Attack Animation and Pathing")]
    [Space(5)]

    public float updateTime;
    public float waitTime;
    private float tempTime;
    [Space(10)]

    [Header("Enemy Stats")]
    [Space(5)]
    public float attackSpeed;
    public int force;
    public int health = 100;
    public float speed = 100f;
    public int targetRange;
    public int attackRange;
    public bool canShoot;
    private enum State
    {

        Roaming,
        Transition,
        Chase,
        Attack

    }
    [Space(10)]
    [Header("References")]
    [Space(5)]
    public float counter=0;
    private State state;
    private NavMeshPath path;
    private Vector3 startingPosition;
    private Vector3 roamPos;
    public bool Attacked;
    public Rigidbody2D enemyrb;
    [SerializeField] Vector3 target;
    Transform player;
    [SerializeField] NavMeshAgent agent;
    public Camera cam;






    void Start()
    {
        path = new NavMeshPath();
        startingPosition = transform.position;
        roamPos = startingPosition;
        agent = GetComponent<NavMeshAgent>();
        agent.GetComponent<CircleCollider2D>().radius = targetRange;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.angularSpeed = 100;
        agent.enabled = true;
        float tempUpdateTime = updateTime;
        state = State.Roaming;
    }

    void Update()
    {
        counter += Time.deltaTime;
        switch (state)
        {
            default:
            case State.Roaming:
                transform.rotation = Quaternion.identity;
                if (CheckForPlayer()) state = State.Chase;
                target = roamPos;
                if (Vector3.Distance(transform.position, target) < 2f)
                {
                    roamPos = transform.position + UtilsClass.GetRandomDir() * Random.Range(1f, 7f);
                    if (NavMesh.CalculatePath(transform.position, roamPos, -1, path))
                    {
                        agent.SetDestination(roamPos);
                    }
                    else
                    {
                        roamPos = transform.position;
                    }
                }
                break;
            case State.Chase:
                tempTime += Time.deltaTime;
                if (tempTime > waitTime)
                {
                    agent.SetDestination(player.position);
                    Collider2D[] cast = Physics2D.OverlapCircleAll(transform.position, attackRange);
                    foreach (Collider2D col in cast)
                    {
                        if (col.tag == "Player")
                        {
                            PointAtPlayer();
                            state = State.Attack;
                        }
                    }
                }
                break;

            case State.Attack:
                    Attack();
                    state = State.Transition;
                break;

            case State.Transition:
                roamPos = transform.position;
                if (CheckForPlayer()) state = State.Chase;
                else state = State.Roaming;
                break;


        }
    }



    /*void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            state = State.Transition;
        }


    }*/

    void Attack()
    {
        agent.enabled = false;


        if (counter >= attackSpeed)
        {
            counter = 0;

            GameObject attack = Instantiate(attackType, firePoint.position, firePoint.rotation);
            Rigidbody2D attackHit = attack.GetComponent<Rigidbody2D>();
            attackHit.AddForce(firePoint.up * -force, ForceMode2D.Impulse);
        }
        agent.enabled = true;




    }


    void PointAtPlayer()
    {
        bool playerInRange = false;
        Vector2 lookDir = player.position - transform.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        GetComponent<Rigidbody2D>().rotation = angle;
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D col in array)
        {
            if (col.tag == "Player")
            {
                playerInRange = true;
            }
        }
        if (!playerInRange)
        {
            state = State.Transition;
        }
    }


    bool CheckForPlayer()
    {
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, targetRange);
        foreach (Collider2D col in array)
        {
            if (col.tag == "Player")
            {
                player = col.GetComponent<Transform>();
                return true;
            }
        }
        return false;
    }

}

