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
    [Space(10)]


    [Header("Update for Attack Animation and Pathing")]
    [Space(5)]

    public float updateTime;
    public float waitTime;
    private float tempTime;
    [Space(10)]

    [Header("Enemy Stats")]
    [Space(5)]
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
    private State state;
    private NavMeshPath path;
    private Vector3 startingPosition;
    private Vector3 roamPos;
    public bool isAttacking;
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
        switch (state)
        {
            default:
            case State.Roaming:
                transform.rotation = Quaternion.identity;
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
                agent.SetDestination(player.position);
                PointAtPlayer();
                break;

            case State.Transition:
                roamPos = transform.position;
                Roaming();
                break;

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<Transform>();
            state = State.Chase;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            state = State.Transition;
        }


    }

    void Attack()
    {
        agent.enabled = false;
        tempTime = waitTime;
        while (tempTime > 0)
        {
            tempTime -= Time.deltaTime;
            Debug.Log(tempTime);
        }
        agent.enabled = true;
        isAttacking = false;
        tempTime = updateTime;


    }


    void PointAtPlayer()
    {
        Vector2 lookDir = player.position - transform.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        GetComponent<Rigidbody2D>().rotation = angle;
    }

    void Roaming()
    {
        state = State.Roaming;

    }

}

