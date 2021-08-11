using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseMenuScript : MonoBehaviour
{
    public GameObject player;
    private int price=100; // PLACEHOLDER TO SEE IF FUNCTION WORKS
    PlayerStats stats;
    public GameObject Panel;

    public void UpgradeArmor() { 
        if (player.GetComponent<PlayerInventory>().gold>=price){
            stats.UpgradeArmor();
        }
        else{
            Debug.Log("Not Enough Dough");
        }
    }

    public void UpgradeHealth() { 
        if (player.GetComponent<PlayerInventory>().gold>=price){
            stats.UpgradeHealth();
        }
        else{
            Debug.Log("Not Enough Dough");
        }
    }

    public void UpgradeSpeed()
    { 
        if (player.GetComponent<PlayerInventory>().gold>=price){
            stats.UpgradeSpeed();
        }
        else{
            Debug.Log("Not Enough Dough");
        }
    }

    public void UpgradeDamage()
    { 
        if (player.GetComponent<PlayerInventory>().gold>=price){
            stats.UpgradeDamage();
        }
        else{
            Debug.Log("Not Enough Dough");
        }
    }
    public void UpgradeDamageMult()
    {
        if (player.GetComponent<PlayerInventory>().gold >= price)
        {
            stats.UpgradeDamageMult();
        }
        else
        {
            Debug.Log("Not Enough Dough");
        }
    }

    public void ExitMenu() {
        Panel.SetActive(false);
    }
}
