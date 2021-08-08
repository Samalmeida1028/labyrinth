using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public SpriteRenderer[] display = new SpriteRenderer[4];

    // Start is called before the first frame update
 void Start(){
     GameObject player = GameObject.FindGameObjectWithTag("Player");
 }


public void AddItemDisplay(Item temp){

    GameObject player = GameObject.FindGameObjectWithTag("Player");
         display[temp.itemType].GetComponent<SpriteRenderer>().sprite = temp.GetComponent<SpriteRenderer>().sprite;
}

public void RefreshDisplay(int slot){
    for(int i = 0; i<display.Length; i++){
        display[i].GetComponent<SpriteRenderer>().color = Color.white;
    }
    display[slot].GetComponent<SpriteRenderer>().color = Color.blue;
}

}
