using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConditions : MonoBehaviour
{
    public int lookDir;
    public GameObject player;
    public Animator animator;

    void FixedUpdate(){ 
        Debug.Log(player.GetComponent<PlayerMovement>().lookDir);
        animator.SetInteger("lookDir", player.GetComponent<PlayerMovement>().lookDir);
       }
}
