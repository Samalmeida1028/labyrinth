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
    public Color interactedColor = Color.blue;


    private void OnTriggerEnter2D(Collider2D other)
    {
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
            else if (tier > 3)
            {
                tier = 3;

            }
            itemInTier = Random.Range(0, 4);
            hasInteract = false;
            canInteract = false;
            activeItem = chestPool[tier, itemInTier];
            if (isShop) price = activeItem.value;
            else price = 0;

        }
        if (other.gameObject.tag == "Player")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            canInteract = true;
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
            GetComponent<SpriteRenderer>().color = interactedColor;
        }

        if (canInteract && Input.GetKey("e") && player.GetComponent<PlayerInventory>().gold >= price)
        {

            if (player.GetComponent<PlayerInventory>().AddItem(activeItem))
            {
                hasInteract = true;
            }
            else if (!hasInteract)
            {
                Debug.Log("Do you want to add item?");
                time += Time.deltaTime;
                if (time > confirm)
                {
                    player.GetComponent<PlayerInventory>().askToAdd = true;
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
