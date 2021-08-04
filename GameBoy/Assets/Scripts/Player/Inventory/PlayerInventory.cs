using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] inventory = new Item[4];
    public GameObject inventoryUI;
    public bool askToAdd = false;

    public bool AddItem(Item item){
        Item temp = item;
        if(inventory[temp.itemType]==null||askToAdd){
            askToAdd = false;
            inventory[temp.itemType] = temp;
            inventoryUI.GetComponent<InventoryUI>().updateDisplay(temp);

            return true;
        }
        else if(!askToAdd){
            Debug.Log("Can't Add!");
            
            return false;

        }
        else{
            return false;
        }

    }
}
