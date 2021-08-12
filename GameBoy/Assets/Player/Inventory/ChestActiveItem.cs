using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestActiveItem : MonoBehaviour
{
    public Item[,] chestPool = new Item[4, 4];
    public bool isShop;
    private int price;
    public Item activeItem;
    public bool canInteract;
    public bool hasInteract;
    public GameObject player;
    public bool hasEntered;
    public int tier;
    public int itemInTier;
    public float tierVal= 1.2f;
    public float confirm = 3f;
    public float time = 0f;
    public Sprite interacted;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            gameObject.GetComponent<ChestInventory>().AddItems();
        if (!hasEntered)
        {
            hasEntered = true;
            Random.InitState(Random.Range(0, 1000));
            chestPool = GetComponent<ChestInventory>().storage;
            tier = (int)(tierVal - Random.Range(0f, 4f));
            if (tier < 0)
            {
                tier = 0;
            }
            else if (tier >= 3)
            {
                tier = 3;

            }
            itemInTier = Random.Range(0, 4);
            hasInteract = false;
            canInteract = false;
            activeItem = chestPool[tier, itemInTier];
            Debug.Log(activeItem);
            if (isShop) price = activeItem.value;
            else price = 0;

        }
        if (other.gameObject.tag == "Player")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            canInteract = true;
        }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        time = 0;
        canInteract = false;
        if (hasInteract) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasInteract)
        {
            GetComponent<SpriteRenderer>().sprite = interacted;
        }

        if (canInteract && Input.GetKey("e") && player.GetComponent<PlayerInventory>().gold >= price)
        {

            if (player.GetComponent<PlayerInventory>().AddItem(activeItem))//checks to see if player added item to take money
            {
                player.GetComponent<PlayerInventory>().gold -= price;
                hasInteract = true;
            }
            else if (!hasInteract)
            {
                Debug.Log("Do you want to add item?");
                time += Time.deltaTime;
                if (time > confirm)
                {
                    player.GetComponent<PlayerInventory>().askToAdd = true;//sets askToAdd true so the AddItem metlhod will trigger
                    player.GetComponent<PlayerInventory>().AddItem(activeItem);
                    hasInteract = true;
                }
                else if (!Input.GetKey("e"))
                {
                    time = 0;
                }
            }
        }
        else if (canInteract && Input.GetKey("e") && player.GetComponent<PlayerInventory>().gold <= price) Debug.Log("Not Enough Dough");
    }
}
