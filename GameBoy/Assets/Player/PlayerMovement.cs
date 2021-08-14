using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    //Public Variables
    public Rigidbody2D RB;
    public GameObject playerDisplay;

    public GameObject BowObj;

    //Axis Info
    private float xAxis;
    private float yAxis;

    //Current
    private string currentAnimation;

    //Animation States
    const string PLAYER_IDLE_F = "Idle_Front";
    const string PLAYER_IDLE_B = "Idle_Back";

    const string PLAYER_WALK_F = "Walk_Forward";
    const string PLAYER_WALK_B = "Walk_Backward";

    const string PLAYER_ATTACK_F = "Attack_Melee_F";
    const string PLAYER_ATTACK_B = "Attack_Melee_B";

    const string PLAYER_BOW_ATTACK_F = "Bow_Shoot_Front";
    const string PLAYER_BOW_ATTACK_B = "Bow_Shoot_Back";

    const string PLAYER_DAMAGED_F = "Damaged_F";
    const string PLAYER_DAMAGED_B = "Damaged_B";

    //Other Variables
    private SpriteRenderer playerSprite;
    private Animator animator;
    private Vector2 movement;

    public bool isFacingBack;
    public bool isFacingRight;

    public PlayerCombat playerCombatInfo;

    private bool isAttackPressed;
    private bool isAttacking;

    public bool isHit;
    public bool hitDB = false;

    private bool isShootingBow;

    private string currentState;

    private bool isMoving;
    private Bow bow;

    //Sams Shit
    private float rotationSpeed = 1000;//how fast player moves to mouse
    public Camera cam;
    private Vector2 mousePos;

    public int lookDir;//used for animator to set the player look direction
    public int angle;//full angle

    void Start()    
    {
        bow = BowObj.GetComponent<Bow>();
        animator = playerDisplay.GetComponent<Animator>();
        playerSprite = playerDisplay.GetComponent<SpriteRenderer>();
    }  

     void Update()
     {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        //Check for player keyboard input
        checkInput();  

        //conversion to find the location of an object in terms of the camera view, must use this in order for PointToMouse(); to work
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Check if player is moving
        if (yAxis != 0 || xAxis != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isFacingRight && !hitDB)
        {
            playerSprite.flipX = false;

        }
        else
        {
            playerSprite.flipX = true;
        }


        //Change Sprite Direction/Animation
        if (isMoving && !isAttacking && !hitDB)
        {
            if (isFacingRight) //If the player is facing right
            {
               if (isFacingBack)
               {
                   ChangeAnimationState(PLAYER_WALK_B);
               }
               else
               {
                    ChangeAnimationState(PLAYER_WALK_F);
               }
            }
            else //If the player is facing left
            {
                if (isFacingBack)
               {
                   ChangeAnimationState(PLAYER_WALK_B);
               }
               else
               {
                    ChangeAnimationState(PLAYER_WALK_F);
               }
            }
        }
        else if (!hitDB)
        {
            if (isFacingBack && !isAttacking)
            {
                ChangeAnimationState(PLAYER_IDLE_B);
            }
            else if (!isAttacking)
            {
              ChangeAnimationState(PLAYER_IDLE_F);  
            }
        }

        //
        if (isShootingBow)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }

        //Check if player attacks
        if (Input.GetButtonDown("Fire1") && !hitDB)
        {
            isAttackPressed = true;
        };

        if (isAttackPressed && !hitDB)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;
                
                if (!playerCombatInfo.isRanged)
                {
                    if (isFacingBack)
                    {
                        ChangeAnimationState(PLAYER_ATTACK_B);
                    }
                    else
                    {
                        ChangeAnimationState(PLAYER_ATTACK_F);
                    }

                    Invoke("AttackComplete", 0.3f);
                }
                else
                {
                    if (angle != 0 && angle != -180)
                    {
                        isShootingBow = true;

                        bow.Shoot();
                        ChangeAnimationState(PLAYER_BOW_ATTACK_F);

                        Invoke("AttackComplete", 0.7f);
                    }
                    else
                    {
                        isAttacking = false;
                    }
                }
            }
        }

        if (isHit)
        {
            isHit = false;

            if (!hitDB)
            {
                hitDB = true;

                if (isFacingBack)
                {
                    ChangeAnimationState(PLAYER_DAMAGED_B);
                }
                else    
                {
                    ChangeAnimationState(PLAYER_DAMAGED_F);
                }

                Invoke("hitComplete", 0.3f);
            }
        }
    }

    void hitComplete()
    {
        hitDB = false;
    }

    public void TakeDamage()
    {
        isHit = true;
    }

    void AttackComplete()
    {
        isAttacking = false;

        if (isShootingBow)
            isShootingBow = false;

    }

    void FixedUpdate()
    {
        RB.velocity = movement * GetComponent<PlayerStats>().moveSpeed;
    }
    
    void checkInput()
    {
        // Normalized just makes the max vector length 1 so diagonal movement isnt faster than vertical or horizontal
        movement = new Vector2(xAxis,yAxis).normalized; 
        PointToMouse();
    }

    // Changes the Player's current animation state
    void ChangeAnimationState(string newState)
    {
        //Stop the same animation from fucking itself
        if (currentState == newState) return;

        //pLAY THAT MF
        animator.Play(newState);
    }

    void PointToMouse()
    {
        //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        Vector2 lookDir = mousePos - RB.position; 

        //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        angle = (int)Mathf.Round((Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f)/45)*45;
        
        //sets look angle to previously found angle
        RB.rotation = angle; 
        FacingDirection(angle);
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

        if (angle <= 0 && angle != -225 && angle != -270)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

}
