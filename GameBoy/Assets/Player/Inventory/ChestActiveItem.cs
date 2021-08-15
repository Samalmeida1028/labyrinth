using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float tierVal;
    public float confirm = 3f;
    public float time = 0f;
    public Sprite interacted;
    public Image itemImage;
    public ParticleSystem itemParticles;

    public GameObject coin;
    public GameObject ammo;
    public ParticleSystem particleGold;
    public AudioClip gold;
    public GameObject canvas;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        var main = itemParticles.main;
        ParticleSystem.SetActive(false);
        tier = (int)(tierVal - Random.Range(0f, 4f));
        if(tier <1)
        {
            main.startColor = Color.white;
        }
        else if(tier<2)
        {
            main.startColor = Color.blue;
        }
        else if(tier<3)
        {
            main.startColor = Color.yellow;
        }
        else if(tier>=3)
        {
            main.startColor = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            gameObject.GetComponent<ChestInventory>().AddItems();
        if (!hasEntered)
        {
            hasEntered = true;
            Random.InitState(Random.Range(0, 1000));
            chestPool = GetComponent<ChestInventory>().storage;
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
            if (isShop)
            {
                price = activeItem.value;
                itemImage.sprite = activeItem.GetComponent<SpriteRenderer>().sprite;
                canvas.SetActive(true);
            } 
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

        if(canInteract&&hasInteract==false&& player.GetComponent<PlayerInventory>().hasItem(activeItem)&& Input.GetKey("e"))
        {
            player.GetComponent<PlayerInventory>().AddItem(activeItem);
            hasInteract = true;
            spawnCoins(activeItem.value/4);
            FindObjectOfType<AudioManager>().Play("ChestOpen");
        }
        else if (canInteract&&hasInteract==false && Input.GetKey("e") && player.GetComponent<PlayerInventory>().gold >= price)
        {

            player.GetComponent<PlayerInventory>().AddItem(activeItem);
            player.GetComponent<PlayerInventory>().AddGold(-price);
            hasInteract = true;
        }
        else if (canInteract && hasInteract==false && Input.GetKey("e") && player.GetComponent<PlayerInventory>().gold < price)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void spawnCoins(int coins)
    {
        Vector3 position = transform.position;
        coins = (int)Random.Range((coins*.5f),coins+1);
        for(int i = 0; i<coins; i++){
            Vector3 random = new Vector2(Random.Range(0,2),Random.Range(0,2));
            Instantiate(coin,transform.position+random,Quaternion.identity);
            if(i%2==0)
            {
                Instantiate(ammo,transform.position+random,Quaternion.identity);
            }
        }
        Instantiate(particleGold,transform.position,Quaternion.identity);
    
    }

}
