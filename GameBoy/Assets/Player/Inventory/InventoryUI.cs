using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Image[] itemSlots = new Image[4];
    public Image[] slots = new Image[3];
    public GameObject player;
    public Slider healthBar;
    public Sprite selectedSlot;
    public Sprite unselectedSlot;
    public TextMeshProUGUI goldCount;
    public TextMeshProUGUI ammoCount;

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        healthBar.wholeNumbers = true;
        healthBar.minValue = 0;
        SetBarMax(player.GetComponent<PlayerStats>().maxHealth);

    }


    public void AddItemDisplay(Item temp){
    Debug.Log("adding ui item");

            itemSlots[temp.itemType].GetComponent<Image>().sprite = temp.GetComponent<SpriteRenderer>().sprite;
            itemSlots[temp.itemType].GetComponent<Image>().enabled = true;
    }

    public void RefreshDisplay(int slot){
        for(int i = 0; i<slots.Length; i++){
            if(slots[i].GetComponent<Image>().sprite == selectedSlot)
            {
                slots[i].GetComponent<Image>().sprite = unselectedSlot;
            }

        }
        slots[slot].GetComponent<Image>().sprite = selectedSlot;

    }

    public void clear(int i)
    {

        itemSlots[i].GetComponent<Image>().sprite = null;
        itemSlots[i].GetComponent<Image>().enabled = false;
        if(i<3)
        {
            slots[i].GetComponent<Image>().sprite = unselectedSlot;
        }

    }

    public void UpdateHealthBar(int cHealth)
    {
        healthBar.value = (cHealth);
    }

    public void SetBarMax(int max)
    {
        healthBar.maxValue = max;
        ResetHealthBar();
    }

    public void ResetHealthBar()
    {
        healthBar.value = healthBar.maxValue;
    }

    public void RefreshGoldDisplay(int gCount)
    {
        goldCount.text = " x " + gCount;
    }

    public void RefreshAmmoDisplay(int aCount)
    {
        ammoCount.text = " x " + aCount;
    }
}
