using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopActiveItem : MonoBehaviour
{
    public Item[,] chestPool = new Item[4, 4];
    public bool isShop;
    private int price;
    public Item activeItem;
    public bool canInteract;
    public bool hasInteract;
    public GameObject player;
    public int tier;
    public int itemInTier;
    public float tierVal;
    public float confirm = 3f;
    public float time = 0f;
    public Color interactedColor = Color.blue;

    public GameObject Chest;

    // Start is called before the first frame update
    void Start()
    {
        canInteract = false;
        hasInteract = false;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
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
        Instantiate(Chest);
        Instantiate(Chest);
        Instantiate(Chest);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasInteract) { 
            GetComponent<SpriteRenderer>().color = interactedColor;
        }
        if (canInteract && Input.GetKey("e")) {
            hasInteract = true;
            
        }
    }
}
