using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Pathfinding;

public class MinotaurScript : MonoBehaviour
{


    private enum State
    {
        Roaming,
        Transition,
        Chase,
        Attack
    }

    [Header("Enemy Type")]
    [Space(5)]
    public GameObject FirepointAxis;
    public Transform firePoint;
    public GameObject baseAttack;
    public GameObject jumpAttack;
    [Space(10)]
    public GameObject lvlChange;

    [Header("Update for Attack Animation and Pathing")]
    [Space(5)]

    //public float updateTime;
    //public float waitTime;
    private float tempTime;
    [Space(10)]

    [Header("Enemy Stats")]
    [Space(5)]
    public float enemyDamage = 10;
    public float jumpDamage = 20;

    public float projectileLife = .5f;
    public int maxHealth = 1000;
    public float attackSpeed = .8f;
    public float attackSpeedStageTwo= .4f;
    public int force;
    public int targetRange;
    public float attackRange1;
    public float attackRange2;

    [Space(10)]
    [Header("References")]
    [Space(5)]
    public float counter = 0;
    float updateCounter;
    private State state;
    private Vector3 targPosition;
    public float radius = 10;

    [SerializeField] Vector3 target;
    private Vector3 roamPos;
    Transform player;

    bool chasing = false;
    public bool stageTwo = false;

    //Public stuff

    //Other Variables
    private SpriteRenderer enemySprite;
    private Animator animator;


    private bool isFacingBack;
    private bool isFacingRight;

    public bool isAttacking;
    private bool isAttackPressed;

    private bool killdb = false;

    public bool isDamaged;
    public bool isKilled;

    private string currentState;
    public IAstarAI ai;
    float lastPathed = 0;

    //Animation States
    const string MONSTER_WALK_F = "Walk_Forward";
    const string MONSTER_WALK_B = "Walk_Backward";

    const string MONSTER_ATTACK_F = "Attack_Forward";
    const string MONSTER_ATTACK_B = "Attack_Backward";

    const string MONSTER_JUMP_ATTACK = "Jump_Attack";

    const string MONSTER_DAMAGED_F = "Damaged_Front";
    const string MONSTER_DAMAGED_B = "Damaged_Back";

    const string DEAD = "Death";

    Vector3 PickRandomPoint() {
        var point = Random.insideUnitSphere * radius;

        //point.y = 0;
        point += transform.position;
        return point;
    }

    void Start()
    {
        AstarPath.active.Scan();
        GetComponent<Rigidbody2D>().freezeRotation = true;

        //Get Animator
        animator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();

        //Get Enemy Stats
        gameObject.GetComponent<HittableStats>().health = (int)(maxHealth);
        ai = GetComponent<IAstarAI>();

        //float tempUpdateTime = updateTime;
        state = State.Roaming;
    }

    void Update()
    {
        // Check for stages
         if (gameObject.GetComponent<HittableStats>().health <= maxHealth/2)
         {
             stageTwo = true;
         }

        //Keep Firepoint Axis on Enemy
        FirepointAxis.transform.position = transform.position;

        //Change Sprite Direction/Animation
        if (isFacingRight && !isKilled)
        {
            enemySprite.flipX = false;
        }
        else
        {
            enemySprite.flipX = true;
        }

        if (!isAttacking && !isDamaged && !isKilled)
        {
            if (isFacingRight) //If the monster is facing right
            {
               if (isFacingBack)
               {
                   ChangeAnimationState(MONSTER_WALK_B);
               }
               else
               {
                    ChangeAnimationState(MONSTER_WALK_F);
               }
            }
            else //If the monster is facing left
            {
                if (isFacingBack)
               {
                   ChangeAnimationState(MONSTER_WALK_B);
               }
               else
               {
                    ChangeAnimationState(MONSTER_WALK_F);
               }
            }
        }

        // Player Monster Attack Animation
        if (isAttackPressed && !isDamaged && !isKilled)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;

                if (!stageTwo)   
                {
                    if (isFacingBack)
                    {
                        ChangeAnimationState(MONSTER_ATTACK_B);
                    }
                    else
                    {
                        ChangeAnimationState(MONSTER_ATTACK_F);
                    }

                    Invoke("AttackComplete", 0.3f);
                }
                else //STAGE TWO 
                {
                    ChangeAnimationState(MONSTER_JUMP_ATTACK);

                    Invoke("AttackComplete", 0.75f);
                }
            }

            

        }  


        // Damaged Animation
        if (isDamaged && !isKilled)
        {
            if (isFacingBack)
            {
                ChangeAnimationState(MONSTER_DAMAGED_B);
            }
            else
            {
                ChangeAnimationState(MONSTER_DAMAGED_F);
            }

            Invoke("DamagedComplete", 0.2f);
        }
        

        // MF DEAD
        if (isKilled)
        {

            if (!killdb)
            {
                killdb = true;

                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                Component[] CircleCollider2Ds; //nuts
                CircleCollider2Ds = GetComponents(typeof(CircleCollider2D));

                foreach (CircleCollider2D f in CircleCollider2Ds)
                    f.enabled = false;

                if (isFacingBack)
                {
                    ChangeAnimationState(MONSTER_DAMAGED_B);
                }
                else
                {
                    ChangeAnimationState(MONSTER_DAMAGED_F);
                }

                Invoke("deathAnimation", 0.2f);
            }
        }
    }

    void deathAnimation()
    {
        ChangeAnimationState(DEAD);
        Invoke("kill", 0.8f);
    }

    void kill()
    {
        Destroy(gameObject);
        lvlChange.GetComponent<LevelChangeScript>().open=true;
    }

    void DamagedComplete()
    {
        isDamaged = false;
    }

    void AttackComplete()
    {
        isAttacking = false;
    }

    void FixedUpdate()
    {
        counter += Time.deltaTime;
        updateCounter += Time.deltaTime;
        switch (state)
        {
            default:
            case State.Roaming:
                if (updateCounter > .2)
                {
                    updateCounter = 0;
                    Roam();
                }

                break;
            case State.Chase:

                if (updateCounter < .2)
                {
                    updateCounter = 0;
                    Chase();
                }
                break;

            case State.Attack:
                Attack();
                state = State.Transition;
                break;

            case State.Transition:
                roamPos = transform.position;
                chasing = false;
                updateCounter = 0;
                if (CheckForPlayer()) state = State.Chase;
                else state = State.Roaming;
                break;
        }
    }



    void Attack()
    {
        if (!isKilled)
        {
            ai.destination = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            ai.SetPath(null);

            PointAtPlayer();
            if (!stageTwo)
            {
                if (counter >= 1 / attackSpeed)
                {
                    isAttackPressed = true;
                    counter = 0;

                    GameObject attack = Instantiate(baseAttack, firePoint.position, firePoint.rotation);

                    attack.GetComponent<EnemyAttack>().SetDamage((int)(enemyDamage));
                    Rigidbody2D attackHit = attack.GetComponent<Rigidbody2D>();
                    FindObjectOfType<AudioManager>().Play("Thud");
                    Destroy(attack, projectileLife);
                }
            }
            else //STAGE TWO ATTACKS
            {
                 if (counter >= 1 / attackSpeedStageTwo)
                {
                    isAttackPressed = true;
                    counter = 0;
                    Invoke("JumpAttack", 0.4f);
                }
            }
        }

    }

    // Jump Attack Call
    void JumpAttack()
    {
        GameObject attack = Instantiate(jumpAttack, transform.position, transform.rotation);

        attack.GetComponent<EnemyAttack>().SetDamage((int)(jumpDamage));
        Rigidbody2D attackHit = attack.GetComponent<Rigidbody2D>();
        Destroy(attack, projectileLife);
    }


    void PointAtPlayer()
    {
        Vector2 lookDir = player.position - transform.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        FacingDirection(angle);
        FirepointAxis.GetComponent<Rigidbody2D>().rotation = angle;
    }
    
    void PointAtTargPos()
    {
        Vector2 lookDir = targPosition - transform.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        FacingDirection(angle);
        FirepointAxis.GetComponent<Rigidbody2D>().rotation = angle;
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

    void Roam()
    {
        if (CheckForPlayer()) state = State.Chase;
        {
            if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
            {
                lastPathed = Time.fixedTime;
                ai.destination = PickRandomPoint();

                targPosition = ai.destination;
                PointAtTargPos();

                ai.SearchPath();
            }
            else if ((Time.fixedTime - lastPathed) > 4)
            {
                lastPathed = Time.fixedTime;
                ai.destination = PickRandomPoint();

                targPosition = ai.destination;
                PointAtTargPos();

                ai.SearchPath();
            }
        }
    }

    void Chase()
    {
        if (CheckForPlayer() && !isKilled)
        {
            if (chasing == false)
            {
                ai.destination = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
                ai.SetPath(null);
                chasing = true;
            }
                lastPathed = Time.fixedTime;
                ai.destination = player.transform.position;
                ai.SearchPath();
  
            Collider2D[] cast;
            PointAtPlayer();
            if(stageTwo)
            {
                cast = Physics2D.OverlapCircleAll(transform.position, attackRange2);
            }else{
                cast = Physics2D.OverlapCircleAll(transform.position, attackRange1);
            }
            foreach (Collider2D col in cast)
            {
                if (col.tag == "Player")
                {
                    state = State.Attack;
                    return;
                }
            }
        }
        else
        {
            state = State.Transition;
        }


    }

    // Changes the Monsters's current animation state
    public void ChangeAnimationState(string newState)
    {
        //Stop the same animation from fucking itself
        if (currentState == newState) return;

        //pLAY THAT MF
        animator.Play(newState);
    }

    void FacingDirection(float angle)
    {
        if (angle <= -90)
        {
            isFacingBack = false;
        }
        else if (angle > -90 )
        {
            isFacingBack = true;
        }

        if (angle <= 0 && angle > -180)
        {
            isFacingRight = true;
        }
        else if (angle < -180 || angle > 0)
        {
            isFacingRight = false;
        }
    }
}

