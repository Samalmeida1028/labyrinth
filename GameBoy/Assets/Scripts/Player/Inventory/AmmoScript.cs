using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    public int ammoAmount = 1;
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(UnityEngine.Random.insideUnitCircle/2,ForceMode2D.Impulse);
        ammoAmount = 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInventory>().AddAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
