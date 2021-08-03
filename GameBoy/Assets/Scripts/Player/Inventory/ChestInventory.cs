using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : MonoBehaviour
{
    public Item[] chestPool = new Item[15];
    public Item activeItem;
    public bool canInteract;
    public bool hasInteract;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        hasInteract = false;
        canInteract = false;
        activeItem = chestPool[0];
    }

    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("hello");

        if(other.gameObject.tag == "Player" && !hasInteract)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            canInteract = true;

        }
    }
    private void OnTriggerExit2D(Collider2D other){
        canInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e")){
            if(canInteract){
                hasInteract = true;
            player.GetComponent<PlayerInventory>().AddItem(activeItem);
            }
            if(canInteract && hasInteract )
            {
                Destroy(gameObject);
            }

        }
        
    }
}
