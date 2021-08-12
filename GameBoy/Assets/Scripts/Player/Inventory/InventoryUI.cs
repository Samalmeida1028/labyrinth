using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Experimental.Rendering.Universal;

public class InventoryUI : MonoBehaviour
{
    public SpriteRenderer[] display = new SpriteRenderer[4];
    public GameObject player;

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void AddItemDisplay(Item temp){
    Debug.Log("adding ui item");

            display[temp.itemType].GetComponent<SpriteRenderer>().sprite = temp.GetComponent<SpriteRenderer>().sprite;
    }

    public void RefreshDisplay(int slot){
    for(int i = 0; i<display.Length; i++){
        if(display[i].GetComponent<SpriteRenderer>() != null){
        display[i].GetComponent<SpriteRenderer>().color = Color.white;
        }

    }
    display[slot].GetComponent<SpriteRenderer>().color = Color.blue;

    }

    public void clear(int i)
    {

        display[i].GetComponent<SpriteRenderer>().sprite = null;

    }
}
