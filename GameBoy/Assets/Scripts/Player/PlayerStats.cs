using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public int baseDamage;
    public int damageMult;
    public float armor;
    public int moveSpeed;

    void Start(){
        maxHealth = 100;
        currentHealth = maxHealth;
        baseDamage = 5;
        armor = 1;

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
}
