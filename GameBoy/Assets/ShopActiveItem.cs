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
    private Vector3 chestpos;

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
        chestpos = transform.position;
        time = 0;
        canInteract = false;
        if (hasInteract) {
            Chest.GetComponent<ChestActiveItem>().isShop = true;
            Instantiate(Chest, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.rotation);
            Instantiate(Chest, new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), transform.rotation);
            Instantiate(Chest, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasInteract) { 
            GetComponent<SpriteRenderer>().color = interactedColor;
        }
        if (canInteract && Input.GetKey("e"))
        {
            hasInteract = true;
        }
    }
}
