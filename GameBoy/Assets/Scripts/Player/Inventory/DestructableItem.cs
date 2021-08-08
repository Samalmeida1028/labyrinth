using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableItem : MonoBehaviour
{
    public int health;
    public GameObject coin;
    public bool isDestroyed= false;

    void Update(){
        if(health<=0){
            isDestroyed = true;
            Destroy(gameObject);
            Instantiate(coin,transform.position,Quaternion.identity);
        }
    }
}
