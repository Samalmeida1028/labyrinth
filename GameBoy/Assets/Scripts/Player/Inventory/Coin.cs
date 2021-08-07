using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int goldAmount;
    void Start()
    {
        goldAmount = Random.Range(0,10);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            other.GetComponent<PlayerInventory>().gold += goldAmount;
            Destroy(gameObject);
        }
    }
}
