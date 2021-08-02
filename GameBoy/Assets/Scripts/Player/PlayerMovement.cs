using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public float moveSpeed;   
    Vector2 movement;
    public Rigidbody2D rb;
    public Camera cam;
    public bool canShoot;
    Vector2 mousePos;

    void Update(){
        checkInput();  
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);//conversion to find the location of an object in terms of the camera view, must use this in order for PointToMouse(); to work
    }

    private void FixedUpdate(){
        rb.velocity = movement*moveSpeed;
        PointToMouse();
 
    }
    
        void checkInput(){
        float movementX = Input.GetAxisRaw("Horizontal");
        float movementY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(movementX,movementY).normalized; //normalized just makes the max vector length 1 so diagonal movement isnt faster than vertical or horizontal
    }



     void PointToMouse(){
        Vector2 lookDir = mousePos - rb.position;   //Subtracts both vectors to find the vector pointing towards the mouse (can be used for any object jsut need to get the objects position and convert)
        float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f;    //finds angle from horizontal field to the vector pointing toward the mouse (90f just is base rotation you can tweak it)
        rb.rotation = angle;    //sets look angle to previously found angle

    }
}
