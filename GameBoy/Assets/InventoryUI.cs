using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public SpriteRenderer[] display = new SpriteRenderer[4];

    // Start is called before the first frame update
 void Start(){
     GameObject player = GameObject.FindGameObjectWithTag("Player");

     for(int i = 0; i<4; i++){
         display[i].sprite = player.GetComponent<PlayerInventory>().inventory[i].GetComponent<SpriteRenderer>().sprite;
         
     }
 }


public void updateDisplay(Item temp){
    Debug.Log(temp.itemType);
    GameObject player = GameObject.FindGameObjectWithTag("Player");
         display[temp.itemType].GetComponent<SpriteRenderer>().sprite = temp.GetComponent<SpriteRenderer>().sprite;
}

}
