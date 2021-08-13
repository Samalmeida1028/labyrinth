using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth=100;
    public int baseDamage;
    public int damageMult;
    public float armor;
    public int moveSpeed;
    public int maxSpeed;
    public GameObject deathScreen;
    public bool die;

    void Start(){    
        currentHealth = maxHealth;
        moveSpeed = maxSpeed;
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

    public void UpdateHealth(int dmg)
    {
        currentHealth-=dmg;
        gameObject.GetComponent<PlayerInventory>().inventoryUI.GetComponent<InventoryUI>().UpdateHealthBar(currentHealth);
    }

    public IEnumerator Die()//move this to PlayerCombat
    {
        gameObject.GetComponent<PlayerInventory>().inventoryUI.SetActive(false);
        gameObject.GetComponent<PlayerInventory>().Clear();
        gameObject.GetComponent<PlayerCombat>().dead = true;
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
        currentHealth=maxHealth;
        moveSpeed=maxSpeed;
        gameObject.GetComponent<PlayerInventory>().inventoryUI.GetComponent<InventoryUI>().ResetHealthBar();
        gameObject.GetComponent<PlayerCombat>().dead = false;
        SceneManager.LoadScene(1);

    }
}
