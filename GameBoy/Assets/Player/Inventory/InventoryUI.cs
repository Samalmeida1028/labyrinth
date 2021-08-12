using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image[] itemSlots = new Image[4];
    public Image[] slots = new Image[3];
    public GameObject player;
    public Sprite selectedSlot;
    public Sprite unselectedSlot;

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void AddItemDisplay(Item temp){
    Debug.Log("adding ui item");

            itemSlots[temp.itemType].GetComponent<Image>().sprite = temp.GetComponent<SpriteRenderer>().sprite;
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
        slots[i].GetComponent<Image>().sprite = unselectedSlot;

    }
}
