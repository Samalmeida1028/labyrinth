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
            Debug.Log("Player went to next level");
        }
    }

    private void onTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            canInteract = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetKey("e")) {
            ChestActiveItem Chest1 = new ChestActiveItem();
            ChestActiveItem Chest2 = new ChestActiveItem();
            ChestActiveItem Chest3 = new ChestActiveItem();

        }
    }
}
