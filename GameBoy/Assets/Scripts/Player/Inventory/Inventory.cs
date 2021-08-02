using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] inventory = new Item[4];
    public GameObject inventoryUI;

    public bool AddItem(Item item){
        Item temp = item;
        if(inventory[temp.itemType]==null){
            inventory[item.itemType] = item;
            inventoryUI.GetComponent<InventoryUI>().updateDisplay();

            return true;
        }
        else{
            Debug.Log("for later");
            return false;

        }

    }
}
