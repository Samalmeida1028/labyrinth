using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = this.gameObject.transform.position;
        player.GetComponent<PlayerInventory>().inventoryUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
