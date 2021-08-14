using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseMenuScript : MonoBehaviour
{
    public GameObject player;
    private int price=20; // PLACEHOLDER TO SEE IF FUNCTION WORKS
    public GameObject PlayerInventoryUI;
    PlayerStats stats;

    public GameObject Panel;
    public GameObject healthButton;
    public GameObject armorButton;
    public GameObject moveSpeedButton;
    public GameObject dmgButton;
    public GameObject dmgMultButton;
    public GameObject prevButton;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI dmgMultText;



    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        PlayerInventoryUI = player.GetComponent<PlayerInventory>().inventoryUI;
        stats = player.GetComponent<PlayerStats>();
        prevButton = healthButton;
        ReloadCost();

    }

    public void UpgradeArmor() 
    {
        FindObjectOfType<AudioManager>().Play("UI_Click");
        prevButton.GetComponent<Image>().color = Color.white; 
        if (player.GetComponent<PlayerInventory>().gold>=(stats.armorLvl*price)&&stats.armorLvl<=5){
            armorButton.GetComponent<Image>().color = Color.green;
            player.GetComponent<PlayerInventory>().AddGold(-(stats.armorLvl*price));
            stats.UpgradeArmor();
            if(stats.armorLvl==6)
            {
                armorText.text = "Max";
            }
            else
            {
                armorText.text = ""+stats.armorLvl*price;
            }
        }
        else{
            armorButton.GetComponent<Image>().color = Color.red;
        }
        prevButton = armorButton;
    }

    public void UpgradeHealth() 
    {
        FindObjectOfType<AudioManager>().Play("UI_Click");
        prevButton.GetComponent<Image>().color = Color.white; 
        if (player.GetComponent<PlayerInventory>().gold>=(stats.healthLvl*price)&&stats.healthLvl<=5){
            healthButton.GetComponent<Image>().color = Color.green;
            player.GetComponent<PlayerInventory>().AddGold(-(stats.healthLvl*price));
            stats.UpgradeHealth();
            if(stats.healthLvl==6)
            {
                healthText.text = "Max";
            }
            else
            {
                healthText.text = ""+stats.healthLvl*price;
            }
        }
        else{
            healthButton.GetComponent<Image>().color = Color.red;
        }
        prevButton = healthButton;
    }

    public void UpgradeSpeed()
    {
        FindObjectOfType<AudioManager>().Play("UI_Click");
        prevButton.GetComponent<Image>().color = Color.white;
        if (player.GetComponent<PlayerInventory>().gold>=(stats.moveSpeedLvl*price)&&stats.moveSpeedLvl<=5){
            moveSpeedButton.GetComponent<Image>().color = Color.green;
            player.GetComponent<PlayerInventory>().AddGold(-(stats.moveSpeedLvl*price));
            stats.UpgradeSpeed();
            if(stats.moveSpeedLvl==6)
            {
                moveSpeedText.text = "Max";
            }
            else
            {
                moveSpeedText.text = ""+stats.moveSpeedLvl*price;
            }
        }
        else{
            moveSpeedButton.GetComponent<Image>().color = Color.red;
        }
        prevButton = moveSpeedButton;
    }

    public void UpgradeDamage()
    {
        FindObjectOfType<AudioManager>().Play("UI_Click");
        prevButton.GetComponent<Image>().color = Color.white;
        if (player.GetComponent<PlayerInventory>().gold>=(stats.dmgLvl*price)&&stats.dmgLvl<=5){
            dmgButton.GetComponent<Image>().color = Color.green;
            player.GetComponent<PlayerInventory>().AddGold(-(stats.dmgLvl*price));
            stats.UpgradeDamage();
            if(stats.dmgLvl==6)
            {
                dmgText.text = "Max";
            }
            else
            {
                dmgText.text = ""+stats.dmgLvl*price;
            }
        }
        else{
            dmgButton.GetComponent<Image>().color = Color.red;
        }
        prevButton = dmgButton;
    }
    public void UpgradeDamageMult()
    {
        FindObjectOfType<AudioManager>().Play("UI_Click");
        prevButton.GetComponent<Image>().color = Color.white;
        if (player.GetComponent<PlayerInventory>().gold >= (stats.dmgMultLvl*price)&&stats.dmgMultLvl<=5)
        {
            dmgMultButton.GetComponent<Image>().color = Color.green;
            player.GetComponent<PlayerInventory>().AddGold(-(stats.dmgMultLvl*price));
            stats.UpgradeDamageMult();
            if(stats.dmgMultLvl==6)
            {
                dmgMultText.text = "Max";
            }
            else
            {
                dmgMultText.text = ""+stats.dmgMultLvl*price;
            }
        }
        else
        {
            dmgMultButton.GetComponent<Image>().color = Color.red;
        }
        prevButton = dmgMultButton;
    }

    public void ExitMenu() {

        FindObjectOfType<AudioManager>().Play("UI_Click");
        prevButton.GetComponent<Image>().color = Color.white;
        Panel.SetActive(false);
        PlayerInventoryUI.SetActive(true);
    }

    public void ReloadCost()
    {
        if(stats.armorLvl==6)
        {
            armorText.text = "max";
        }
        else
        {
            armorText.text = ""+stats.armorLvl*price;
        }

        if(stats.healthLvl==6)
        {
            healthText.text = "Max";
        }
        else
        {
            healthText.text = ""+stats.healthLvl*price;
        }

        if(stats.moveSpeed==6)
        {
            moveSpeedText.text = "Max";
        }
        else
        {
            moveSpeedText.text = ""+stats.moveSpeedLvl*price;
        }

        if(stats.dmgLvl==6)
        {
            dmgText.text = "Max";
        }
        else
        {
            dmgText.text = ""+stats.dmgLvl*price;
        }
        if(stats.dmgMultLvl==6)
        {
            dmgMultText.text = "Max";
        }
        else
        {
            dmgMultText.text = ""+stats.dmgMultLvl*price;
        }
    }

}
