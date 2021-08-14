using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{

    public SpriteRenderer Sprite;
    public GameObject Player;

    private Animator Animator;

    // Other shit
    private bool isShooting;
    private bool shot;
    private string currentState;

    private PlayerMovement MovementInformation;
    private PlayerCombat Combat;

    // Animaton References
    const string SHOOT_F = "Shoot_Forward";
    const string SHOOT_B = "Shoot_Backwards";
    const string IDLE = "Idle";

    // For rotating the bow
    private float currentYAxis = -0.16f;
    private Quaternion baseRotation;

    void Start()
    {
        // Initialization
        MovementInformation = Player.GetComponent<PlayerMovement>();
        Combat = Player.GetComponent<PlayerCombat>();
        Animator = GetComponent<Animator>();

        // Resets the bows position and saves its base rotation
        transform.position = Player.transform.position;
        baseRotation = transform.rotation;
    }


    void Update()
    {
        // If the player isnt in the middle of shooting then disable the sprite and reset the animation
        if (!isShooting)
        {
            Sprite.enabled = false;
            ChangeAnimationState(IDLE);
        }

        //Flip Bow
        if (!MovementInformation.isFacingRight)
        {
            transform.position = Player.transform.position + new Vector3(-0.2f, currentYAxis);
            Sprite.flipX = true;
        }
        else
        {
            gameObject.transform.position = Player.transform.position + new Vector3(0.2f, currentYAxis);
            Sprite.flipX = false;
        }

        //Animate bow shooting
        if (shot)
        {
            shot = false;
            rotateBow();

            if (!isShooting)
            {
                isShooting = true;
                Sprite.enabled = true;

  
                ChangeAnimationState(SHOOT_F); 
                Invoke("shootBullet", 0.4f);
            }

            Invoke("stopShooting", 0.6f);
        }


    }

    // Shoot Bullet
    void shootBullet()
    {
        Combat.Attack();
    }

    // Allows the shooting animation to be played again after already shooting
    void stopShooting()
    {
        transform.rotation = baseRotation;
        isShooting = false;
    }

    // Shoot the bow(Play the animation)
    public void Shoot()
    {
        shot = true;
    }

    // Rotate Bow
    // All hard-coded in I dont care we have a day left this took 5 minutes and was mad easy
    void rotateBow()
    {
        if (MovementInformation.angle == -45)
        {
            currentYAxis = 0.05f;
            transform.Rotate(0.0f, 0.0f, 20f, Space.Self);

        }
        else if (MovementInformation.angle == -90)
        {
            currentYAxis = -0.16f;
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (MovementInformation.angle == -135)
        {
            currentYAxis = -0.3f;
            transform.Rotate(0.0f, 0.0f, -21f, Space.Self);
        }
        else if (MovementInformation.angle == 45)
        {
            currentYAxis = 0.05f;
            transform.Rotate(0.0f, 0.0f, -20f, Space.Self);
        }
        else if (MovementInformation.angle == 90 || MovementInformation.angle == -270)
        {
            currentYAxis = -0.16f;
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (MovementInformation.angle == -225)
        {
            currentYAxis = -0.3f;
            transform.Rotate(0.0f, 0.0f, 21f, Space.Self);
        }
    }

    void ChangeAnimationState(string newState)
    {
        //Stop the same animation from fucking itself
        if (currentState == newState) return;

        currentState = newState;
        //pLAY THAT MF
        Animator.Play(newState);
    }

}
