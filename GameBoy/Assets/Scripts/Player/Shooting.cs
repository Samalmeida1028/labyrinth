using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
public Transform firePoint;//this is an empty gameobject used for transform
public GameObject bullet;

public float force =20f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){   //Fire1 is M1
            Shoot();


        }
        
    }

    void Shoot(){
        GameObject bulletSpawn = Instantiate(bullet, firePoint.position, firePoint.rotation);//makes the prefab at the fire point, uses the rotation of the player to determine the shooting angle
        Rigidbody2D rb = bulletSpawn.GetComponent<Rigidbody2D>();//temp rigidbody for force
        rb.AddForce(firePoint.up*force, ForceMode2D.Impulse);//applies a force to bullet prefab so it go go (impulse means it is instantaneous not continuous)

    }
}
