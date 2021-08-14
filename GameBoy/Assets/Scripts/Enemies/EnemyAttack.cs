using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public bool isSpell;

    private string currentState;

    //Animation States
    const string IMPACT = "Impact";
    const string HOLDING = "Throwing";

    private bool isImpacted;

    public void SetDamage(int enemyDamage){
        damage = enemyDamage;
    }

    void Update()
    {
        if (!isImpacted)
        {
           ChangeAnimationState(HOLDING); 
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Player" || other.gameObject.layer == 3 || other.gameObject.layer == 9)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

            if (!isSpell)
            {
                destroyProjectile();
            }
            else
            {
                isImpacted = true;
                Debug.Log("Impact");
                
                ChangeAnimationState(IMPACT);
                Invoke("destroyProjectile", 0.35f);
            }
        }
       
    }


    // Changes the Monsters's current animation state
    public void ChangeAnimationState(string newState)
    {
        //Stop the same animation from fucking itself
        if (currentState == newState) return;

        //pLAY THAT MF
       gameObject.transform.GetChild(0).GetComponent<Animator>().Play(newState);
    }

    void destroyProjectile()
    {
        Destroy(gameObject);
    }

}
