using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    //Public Variables
    public Rigidbody2D RB;
    public GameObject playerDisplay;

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

    //Other Variables
    private SpriteRenderer playerSprite;
    private Animator animator;
    private Vector2 movement;

    private bool isAttackPressed;
    private bool isAttacking;

    private string currentState;

    void Start()    
    {
        animator = playerDisplay.GetComponent<Animator>();
        playerSprite = playerDisplay.GetComponent<SpriteRenderer>();
    }  

     void Update()
     {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        //Check for player keyboard input
        checkInput();  
    }

    void FixedUpdate()
    {
        RB.velocity = movement*GetComponent<PlayerStats>().moveSpeed;
    }
    
    void checkInput()
    {
        // Movement
        movement = new Vector2(xAxis,yAxis).normalized; 

        //Check what axis the player is pointing towards
        if (xAxis < 0 ) //Moving Left
        {
            if (yAxis > 0)//Moving Up
            {
            playerSprite.flipX = true;
            ChangeAnimationState(PLAYER_WALK_B);
            }
            else
            {
            playerSprite.flipX = true;
            ChangeAnimationState(PLAYER_WALK_F);
            }

        }
        else if (xAxis >0 ) //Moving Right
        {
            if (yAxis > 0)//Moving Up
            {
            playerSprite.flipX = false;
            ChangeAnimationState(PLAYER_WALK_B);
            }
            else
            {
            playerSprite.flipX = false;
            ChangeAnimationState(PLAYER_WALK_F);
            }
        }
        else if (yAxis > 0) //Moving Up
        {
            playerSprite.flipX = false;
            ChangeAnimationState(PLAYER_WALK_B);
        }
        else if (yAxis < 0) //Moving Down
        {
            playerSprite.flipX = false;
            ChangeAnimationState(PLAYER_WALK_F);
        }
        else //Not Moving
        {
            ChangeAnimationState(PLAYER_IDLE_F);
        }
    }

    void ChangeAnimationState(string newState)
    {
        //Stop the same animation from fucking itself
        if (currentState == newState) return;

        //pLAY THAT MF
        animator.Play(newState);
    }


}
