using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth=100;
    public int baseDamage;
    public float damageMult;
    public float armor;
    public int moveSpeed;
    public int maxSpeed;
    public GameObject deathScreen;
    public bool die;

    public int healthLvl;
    public int armorLvl;
    public int moveSpeedLvl;
    public int dmgLvl;
    public int dmgMultLvl ;

    void Start(){    
        currentHealth = maxHealth;
        maxSpeed = 8;
        moveSpeed = maxSpeed;
        baseDamage = 5;
        armor = 1f;

        healthLvl = 1;
        armorLvl = 1;
        moveSpeedLvl = 1;
        dmgLvl = 1;
        dmgMultLvl = 1;
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
        armorLvl++;

        armor = 1+((armorLvl-1)*.2f);
        GetComponent<PlayerCombat>().ChangeArmor();
    }

    public void UpgradeHealth()
    {
        healthLvl++;

        maxHealth=100+((healthLvl-1)*20);
        currentHealth=maxHealth;
        gameObject.GetComponent<PlayerInventory>().inventoryUI.GetComponent<InventoryUI>().SetBarMax(maxHealth);
    }

    public void UpgradeSpeed()
    {
        moveSpeedLvl++;

        maxSpeed = (8+moveSpeedLvl-1);
        moveSpeed=maxSpeed;
    }

    public void UpgradeDamage()
    {
        dmgLvl++;

        baseDamage = (dmgLvl*5);

    }
    public void UpgradeDamageMult() 
    { 
        dmgMultLvl++;

        damageMult = 1+((dmgMultLvl-1)*.2f);
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
