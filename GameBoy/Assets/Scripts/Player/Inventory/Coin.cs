using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int goldAmount;
    void Start()
    {
        goldAmount = Random.Range(0, 10);
        GetComponent<Rigidbody2D>().AddForce(UnityEngine.Random.insideUnitCircle/2,ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInventory>().AddGold(goldAmount);
            Destroy(gameObject);
        }
    }
}
