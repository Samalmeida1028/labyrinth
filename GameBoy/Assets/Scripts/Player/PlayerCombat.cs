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
    public bool isPotion;
    int itemDamage;

    void Start()
    {
        canAttack = true;
        isRanged = false;
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
        else if (isPotion){
           if(Heal(GetComponent<PlayerInventory>().activeItem.GetComponent<Item>().healthAmount))
           {
               Destroy(GetComponent<PlayerInventory>().activeItem);
           }
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
            totalArmor = GetComponent<PlayerStats>().armor * GetComponent<PlayerInventory>().inventory[3].armorVal;

        }
        else
        {
            totalArmor = GetComponent<PlayerStats>().armor;
        }
    }

    public void Attack()
    {
        Debug.Log("AAAH");
        if (isRanged)
        {
            if(GetComponent<PlayerInventory>().ammo >0){
            int force = -projectileSpeed;
            GameObject bulletSpawn = Instantiate(rangedAttack, attackPoint.position, attackPoint.rotation);
            bulletSpawn.GetComponent<Bullet>().SetDamage(totalDamage);
            Rigidbody2D rb = bulletSpawn.GetComponent<Rigidbody2D>();
            rb.AddForce(attackPoint.up * force, ForceMode2D.Impulse);
            GetComponent<PlayerInventory>().ammo -=1;
        }
        else{}
        }
        else
        {
            GameObject melee = Instantiate(meleeAttack, attackPoint.position, attackPoint.rotation);
            melee.GetComponent<Bullet>().SetDamage(totalDamage);
            Destroy(melee, .1f);
        }
    }

    void TakeDamage(int damage)
    {
        totalArmor = GetComponent<PlayerStats>().armor;
        GetComponent<PlayerStats>().currentHealth -= (int)(damage / totalArmor);
        if (GetComponent<PlayerStats>().currentHealth <= 0)
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
            Debug.Log(other.gameObject.GetComponent<EnemyAttack>().damage);
            TakeDamage(other.gameObject.GetComponent<EnemyAttack>().damage);
        }
    }
    public bool Heal(int health){
        int difference = 0;
        if(gameObject.GetComponent<PlayerStats>().currentHealth + health < gameObject.GetComponent<PlayerStats>().maxHealth){
            gameObject.GetComponent<PlayerStats>().currentHealth += health;
            return true;
        }
        else if(gameObject.GetComponent<PlayerStats>().maxHealth - gameObject.GetComponent<PlayerStats>().currentHealth != 0){
            difference = gameObject.GetComponent<PlayerStats>().maxHealth - gameObject.GetComponent<PlayerStats>().currentHealth;
             gameObject.GetComponent<PlayerStats>().currentHealth += difference;
             return true;
        }
        else{
            Debug.Log("AKLJHLKDFJH");
            return false;
        }
    }

}
