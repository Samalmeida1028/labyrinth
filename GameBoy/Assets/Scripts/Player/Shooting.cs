using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
public Transform firePoint;
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
        GameObject bulletSpawn = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bulletSpawn.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up*force, ForceMode2D.Impulse);

    }
}
