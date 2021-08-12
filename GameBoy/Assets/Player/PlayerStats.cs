using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public int baseDamage;
    public int damageMult;
    public float armor;
    public int moveSpeed;
    public GameObject deathScreen;
    public bool die;

    void Start(){    
        maxHealth = 100;
        currentHealth = maxHealth;
        baseDamage = 5;
    }

    void Update()
    {
        if(currentHealth<=0)
        {
            StartCoroutine(Die());
        }
    }

    public void UpgradeArmor()
    {


    }

    public void UpgradeHealth()
    {

    }

    public void UpgradeSpeed()
    {


    }

    public void UpgradeDamage()
    {


    }
    public void UpgradeDamageMult() { 
    
    }

    public IEnumerator Die()//move this to PlayerCombat
    {
        currentHealth=maxHealth;
        gameObject.GetComponent<PlayerInventory>().Clear();
        Instantiate(deathScreen,gameObject.transform.position,Quaternion.identity);

        bool continueNext = false;

        while(continueNext == false)
        {
            yield return new WaitForSeconds(.05f);
            if(Input.GetKeyDown("e"))
            {
                continueNext=true;
            }
        }
        SceneManager.LoadScene(1);

    }
}
