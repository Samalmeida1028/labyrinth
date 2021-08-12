using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
<<<<<<< Updated upstream:GameBoy/Assets/Scripts/Player/PlayerMovement.cs
 
    Vector2 movement;
    public float counter;
    private float rotationSpeed = 1000;
    public Rigidbody2D rb;
    public Camera cam;
    Vector2 mousePos;

    public int lookDir;
    public int angle;
=======
    //Public Variables
    public Rigidbody2D RB;
    public GameObject playerDisplay;

    //Axis Info
    private float xAxis;
    private float yAxis;
>>>>>>> Stashed changes:GameBoy/Assets/Player/PlayerMovement.cs

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

<<<<<<< Updated upstream:GameBoy/Assets/Scripts/Player/PlayerMovement.cs
    void Start(){
    }   
     void Update(){
        checkInput();  
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);//conversion to find the location of an object in terms of the camera view, must use this in order for PointToMouse(); to work
    }

    private void FixedUpdate(){
        counter += Time.fixedDeltaTime;
        rb.velocity = movement*GetComponent<PlayerStats>().moveSpeed;

 
    }
    
        void checkInput(){
        float movementX = Input.GetAxisRaw("Horizontal");
        float movementY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(movementX,movementY).normalized; //normalized just makes the max vector length 1 so diagonal movement isnt faster than vertical or horizontal
        PointToMouse();

    }
=======
    private bool isAttackPressed;
    private bool isAttacking;
>>>>>>> Stashed changes:GameBoy/Assets/Player/PlayerMovement.cs

    private string currentState;

    void Start()    
    {
        animator = playerDisplay.GetComponent<Animator>();
        playerSprite = playerDisplay.GetComponent<SpriteRenderer>();
    }  

<<<<<<< Updated upstream:GameBoy/Assets/Scripts/Player/PlayerMovement.cs
     void PointToMouse(){
        Vector2 lookDir = mousePos - rb.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        angle = (int)Mathf.Round((Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f)/45)*45;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        rb.rotation = angle;    //sets look angle to previously found angle
        if(counter>.1){
            counter = 0;
        FacingDirection(angle);
        }
=======
     void Update()
     {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
>>>>>>> Stashed changes:GameBoy/Assets/Player/PlayerMovement.cs

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
