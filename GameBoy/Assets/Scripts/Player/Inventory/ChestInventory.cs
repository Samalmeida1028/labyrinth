using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : MonoBehaviour
{
    public Item[] itemInputInv = new Item[16];
    public Item[,] storage = new Item[4,4];
    // Start is called before the first frame update
    void Start()
    {
        for( int i = 0; i<16; i++){
            Debug.Log("Col"+(int)i/4 + "Row" + i%4);
            storage[(int)i/4,i%4]= itemInputInv[i];

        }
    }
}
