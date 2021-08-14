using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] inventory = new Item[4];
    public Item activeItem;//item in player's hand
    public Item starterItem;
    public GameObject inventoryUI;
    public bool askToAdd = false;//used for shop to determine when to take money from player
    public int gold = 0;
    public int ammo = 10;

    void Start()
    {
        inventoryUI.GetComponent<InventoryUI>().RefreshGoldDisplay(gold);
        inventoryUI.GetComponent<InventoryUI>().RefreshAmmoDisplay(ammo);
        AddItem(starterItem);
    }

    void Update(){
        //sets the current slot for items
        if(Input.GetKeyDown("1")) ChangeActiveItem(0);
        if(Input.GetKeyDown("2")) ChangeActiveItem(1);
        if(Input.GetKeyDown("3")) ChangeActiveItem(2);
    }
    public bool AddItem(Item item)
    {
        Item temp = item;
        if(inventory[temp.itemType]==null || (temp.rarity>inventory[temp.itemType].rarity))
        {
            inventory[temp.itemType] = temp;
            inventoryUI.GetComponent<InventoryUI>().AddItemDisplay(temp);
            if(temp.itemType==3)
            {
                GetComponent<PlayerCombat>().ChangeArmor();
            }
        }else{
            return true;
        }
        return false;
    }

    public void AddGold(int goldAmount)
    {
        gold += goldAmount;
        inventoryUI.GetComponent<InventoryUI>().RefreshGoldDisplay(gold);
    }

    public void AddAmmo(int ammoAmount)
    {
        ammo += ammoAmount;
        inventoryUI.GetComponent<InventoryUI>().RefreshAmmoDisplay(ammo);
    }

    public void ChangeActiveItem(int slot){

        if(slot!=3){
            if(inventory[slot]!=null){
            inventoryUI.GetComponent<InventoryUI>().RefreshDisplay(slot);
            activeItem = inventory[slot];
            GetComponent<PlayerCombat>().ChangeDamage();
            //these if statements just basically check the itemType, you could set it to that as well, 1 = 0, 2 = 1 etc for slot to itemType
            if(slot == 1){
            GetComponent<PlayerCombat>().canAttack = true;
            GetComponent<PlayerCombat>().isRanged = true;
            }
            else if(slot == 0){
                GetComponent<PlayerCombat>().canAttack = true;
                GetComponent<PlayerCombat>().isRanged = false;
            }
            else if(slot == 2){
                if(inventory[0]!=null || inventory[1]!=null){
                GetComponent<PlayerCombat>().canAttack = false;
                }
                GetComponent<PlayerCombat>().isPotion = true;
            }
            }
        }

    }
    public void Clear()
    {
        ammo=10;
        AddAmmo(0);
        for(int i=0; i<inventory.Length;i++)
        {
            inventory[i] = null;
            inventoryUI.GetComponent<InventoryUI>().clear(i);
        }
        activeItem=null;
        AddItem(starterItem);
    }

    public void clearSlot(int slot)
    {
        if(activeItem.itemType==slot)
        {
            activeItem=null;
        }

        inventory[slot]=null;
        inventoryUI.GetComponent<InventoryUI>().clear(slot);

    }

    public bool hasItem(Item item)
    {
         if(item!=null&&inventory[item.itemType]!=null)
         {
            return (inventory[item.itemType] == item); 
         }
        return false;
        
    }
}
