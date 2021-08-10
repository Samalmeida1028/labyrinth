using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
 
    Vector2 movement;
    public float counter;
    private float rotationSpeed = 1000;
    public Rigidbody2D rb;
    public Camera cam;
    Vector2 mousePos;

    public int lookDir;
    public int angle;





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



     void PointToMouse(){
        Vector2 lookDir = mousePos - rb.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        angle = (int)Mathf.Round((Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f)/45)*45;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        rb.rotation = angle;    //sets look angle to previously found angle
        if(counter>.1){
            counter = 0;
        FacingDirection(angle);
        }


    }

    void PointInDirection(){
         if(movement != Vector2.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(Vector3.forward, movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotate,rotationSpeed*Time.deltaTime);
        }
    }

    void FacingDirection(float angle){
        if(angle==0){
            lookDir = 0; 
        }
        if(angle == -180){
            lookDir = 1;
        }
        if(angle == -90){
            lookDir = 2;
        }
        if(angle == 90){
            lookDir = 3;
        }


    }
}
