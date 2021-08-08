using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] inventory = new Item[4];
    public Item activeItem;
    public GameObject inventoryUI;
    public bool askToAdd = false;
    public int gold;

    void Update(){
        if(Input.GetKeyDown("1")) {
            Debug.Log("Hello");
            ChangeActiveItem(0);}
        if(Input.GetKeyDown("2")) ChangeActiveItem(1);
        if(Input.GetKeyDown("3")) ChangeActiveItem(2);
        if(Input.GetKeyDown("4")) ChangeActiveItem(3);
    }
    public bool AddItem(Item item)
    {
        Item temp = item;
        if (inventory[temp.itemType] == null || askToAdd)
        {
            Debug.Log("Added!");
            askToAdd = false;
            inventory[temp.itemType] = temp;
            inventoryUI.GetComponent<InventoryUI>().updateDisplay(temp);

            return true;
        }
        else if (!askToAdd)
        {
            Debug.Log("Can't Add!");

            return false;

        }
        else
        {
            return false;
        }


    }
    public void AddGold(int goldAmount)
    {
        gold += goldAmount;
    }


    public void ChangeActiveItem(int slot){
        if(inventory[slot]!=null){
            Debug.Log(slot);
            activeItem = inventory[slot];
            GetComponent<PlayerCombat>().ChangeDamage();
            if(slot == 1){
            GetComponent<PlayerCombat>().canAttack = true;
            GetComponent<PlayerCombat>().isRanged = true;
        }
        else if(slot == 0){
            GetComponent<PlayerCombat>().canAttack = true;
            GetComponent<PlayerCombat>().isRanged = false;
        }
        else GetComponent<PlayerCombat>().canAttack = false;
        }
        else{
            Debug.Log("Can't switch!");
        }


    }
}
