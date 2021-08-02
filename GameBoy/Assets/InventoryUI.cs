using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public SpriteRenderer[] display = new SpriteRenderer[4];

    // Start is called before the first frame update
 void Start(){
     GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

     for(int i = 0; i<4; i++){
         display[i].sprite = player[0].GetComponent<Inventory>().inventory[i].GetComponent<SpriteRenderer>().sprite;
         
     }

 }


public void updateDisplay(){
    GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

     for(int i = 0; i<4; i++){
         display[i].sprite = player[0].GetComponent<Inventory>().inventory[i].GetComponent<SpriteRenderer>().sprite;
         
     }
}
}
