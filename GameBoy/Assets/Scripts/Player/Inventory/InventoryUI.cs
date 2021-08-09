using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Experimental.Rendering.Universal;

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
        display[i].GetComponent<Light2D>().intensity = 0;

    }
    display[slot].GetComponent<SpriteRenderer>().color = Color.blue;
    display[slot].GetComponent<Light2D>().intensity = 3;

}

}
