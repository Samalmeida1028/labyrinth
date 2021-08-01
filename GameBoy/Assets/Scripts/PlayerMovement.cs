using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public float moveSpeed;   
    Vector2 movement;
    public Rigidbody2D rb;
    public Camera cam;

    Vector2 mousePos;

    void Update(){
        checkInput();  
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate(){
        rb.velocity = movement*moveSpeed;
        PointToMouse();
 
    }
    
        void checkInput(){
        float movementX = Input.GetAxisRaw("Horizontal");
        float movementY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(movementX,movementY).normalized;
    }

     void PointToMouse(){
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg +90f;
        rb.rotation = angle;

    }
}
