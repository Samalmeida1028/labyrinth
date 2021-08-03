using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] inventory = new Item[4];
    public GameObject inventoryUI;

    public bool AddItem(Item item){
        Item temp = item;
        if(inventory[temp.itemType]==null){
            inventory[temp.itemType] = temp;
            inventoryUI.GetComponent<InventoryUI>().updateDisplay(temp);

            return true;
        }
        else{
            Debug.Log("for later");
            
            return false;

        }

    }
}
