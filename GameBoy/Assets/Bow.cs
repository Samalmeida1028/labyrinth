using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{

    public SpriteRenderer Sprite;

    public GameObject d;

    private Animator Animator;

    public bool flipX;

    // Other shit
    private bool isShooting;
    private bool shot;
    private string currentState;

    // Animaton References
    const string SHOOT_F = "Shoot_Forward";
    const string SHOOT_B = "Shoot_Backward";
    const string IDLE = "Idle";


    void Start()
    {
        Animator = GetComponent<Animator>();

        transform.position = d.transform.position;
    }


    void Update()
    {

        if (!isShooting)
        {
            Sprite.enabled = false;
            ChangeAnimationState(IDLE);
        }

        //Flip Bow
        if (flipX)
        {
            transform.position = d.transform.position + new Vector3(-0.2f, -0.16f);
            Sprite.flipX = true;
        }
        else
        {
            gameObject.transform.position = d.transform.position + new Vector3(0.2f, -0.16f);
            Sprite.flipX = false;
        }

        //Animate bow shooting
        if (shot)
        {
            shot = false;

            if (!isShooting)
            {
                isShooting = true;

                Sprite.enabled = true;

                //ChangeAnimationState(SHOOT);
            }

            Invoke("stopShooting", 0.6f);
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

    void stopShooting()
    {
        isShooting = false;
    }

    public void Shoot()
    {
        shot = true;
    }

}
