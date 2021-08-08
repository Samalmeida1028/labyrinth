using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{


    public GameObject rangedAttack;
    public GameObject meleeAttack;
    public Transform attackPoint;
    public int totalDamage;
    public float totalArmor;
    public int attackSpeed;
    public int projectileSpeed;
    public bool isRanged;
    public bool canAttack;
    int itemDamage;

    void Start()
    {
        projectileSpeed = 10;
        ChangeDamage();
        ChangeArmor();

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (canAttack) Attack();
        }
    }
    public void ChangeDamage()
    {
        int baseDamage = GetComponent<PlayerStats>().baseDamage;
        int damageMult = GetComponent<PlayerStats>().damageMult;
        if (GetComponent<PlayerInventory>().activeItem != null)
        {
            itemDamage = GetComponent<PlayerInventory>().activeItem.damage;
        }
        else { itemDamage = 0; }
        totalDamage = (baseDamage + itemDamage) * damageMult;
    }
    public void ChangeArmor()
    {
        if (GetComponent<PlayerInventory>().inventory[3] != null)
        {
            totalArmor = GetComponent<PlayerStats>().armor + GetComponent<PlayerInventory>().inventory[3].armorVal;

        }
        else
        {
            totalArmor = GetComponent<PlayerStats>().armor;
        }
    }

    public void Attack()
    {
        if (isRanged)
        {
            int force = projectileSpeed;
            GameObject bulletSpawn = Instantiate(rangedAttack, attackPoint.position, attackPoint.rotation);
            Rigidbody2D rb = bulletSpawn.GetComponent<Rigidbody2D>();
            rb.AddForce(attackPoint.up * force, ForceMode2D.Impulse);
        }
        else
        {
            GameObject melee = Instantiate(meleeAttack, attackPoint.position, attackPoint.rotation);
            Destroy(melee, .1f);
        }
    }

    void TakeDamage(int damage)
    {
        totalArmor = GetComponent<PlayerStats>().armor;
        GetComponent<PlayerStats>().health -= (int)(damage / totalArmor);
        if (GetComponent<PlayerStats>().health <= 0)
        {
            Debug.Log("Ouch");
            Die();
        }
    }

    void Die()
    {
        GetComponent<PlayerStats>().moveSpeed = 0;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            TakeDamage(other.gameObject.GetComponent<EnemyDamage>().damage);
            Destroy(other.gameObject);
        }
    }

}
