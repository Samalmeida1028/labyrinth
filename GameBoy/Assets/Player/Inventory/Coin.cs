using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int goldAmount = 1;
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(UnityEngine.Random.insideUnitCircle/2,ForceMode2D.Impulse);
        goldAmount = 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInventory>().AddGold(goldAmount);
            FindObjectOfType<AudioManager>().Play("CoinPickup");
            Destroy(gameObject);

        }
    }
}
