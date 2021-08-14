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


    [Header("Update for Attack Animation and Pathing")]
    [Space(5)]

    //public float updateTime;
    //public float waitTime;
    private float tempTime;
    [Space(10)]

    [Header("Enemy Stats")]
    [Space(5)]
    public float enemyDamage;
    public float jumpDamage;
    public float moveSpeed;
    public float maxSpeed = 5f;
    public float projectileLife = .5f;
    public int maxHealth = 1000;
    public float attackSpeed;
    public int force;
    public int targetRange;
    public int attackRange;

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
    bool stageTwo = false;

    //Public stuff

    //Other Variables
    private SpriteRenderer enemySprite;
    private Animator animator;

    private bool isFacingBack;
    private bool isFacingRight;

    private bool isAttacking;
    private bool isAttackPressed;

    private bool killdb = false;

    public bool isDamaged;
    public bool isKilled;

    private string currentState;
    public IAstarAI ai;
    float lastPathed = 0;

    public Slider healthBar;
    //Animation States
    const string MONSTER_WALK_F = "Walk_Forward";
    const string MONSTER_WALK_B = "Walk_Backward";

    const string MONSTER_ATTACK_F = "Attack_Forward";
    const string MONSTER_ATTACK_B = "Attack_Backward";

    const string MONSTER_JUMP_ATTACK_F = "Jump_Attack_Forward";
    const string MONSTER_JUMP_ATTACK_B = "Jump_Attack_Backward";

    const string MONSTER_DAMAGED_F = "Enemy_Damaged_Forward";
    const string MONSTER_DAMAGED_B = "Enemy_Damaged_Backward";

    const string DEAD = "Death";

    Vector3 PickRandomPoint() {
        var point = Random.insideUnitSphere * radius;

        //point.y = 0;
        point += transform.position;
        return point;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().freezeRotation = true;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
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
                
                if (isFacingBack)
                {
                    ChangeAnimationState(MONSTER_ATTACK_B);
                }
                else
                {
                    ChangeAnimationState(MONSTER_ATTACK_F);
                }
            }

            Invoke("AttackComplete", 0.3f);

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
            if (counter >= 1 / attackSpeed)
            {
                isAttackPressed = true;
                counter = 0;

                GameObject attack = Instantiate(baseAttack, firePoint.position, firePoint.rotation);

                attack.GetComponent<EnemyAttack>().SetDamage((int)(enemyDamage));
                Rigidbody2D attackHit = attack.GetComponent<Rigidbody2D>();
                Destroy(attack, projectileLife);

                //attackHit.AddForce(firePoint.up * -force, ForceMode2D.Impulse);
            }
        }

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


            if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
            {
                lastPathed = Time.fixedTime;
                ai.destination = player.transform.position;
                ai.SearchPath();
            }   
            PointAtPlayer();
            Collider2D[] cast = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D col in cast)
            {
                if (col.tag == "Player")
                {
                    state = State.Attack;
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

    public void UpdateHealthBar(int cHealth)
    {
        healthBar.value = (cHealth);
    }
}

