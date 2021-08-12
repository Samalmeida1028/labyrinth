using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{


    public GameObject rangedAttack;//ranged prefab
    public GameObject meleeAttack;//melee prefab
    public Transform attackPoint;//where prefabs spawn from
    public int totalDamage;//damage of base damage and item
    public float totalArmor;//base armor and item armor
    public float updateCounter;//used for attackSpeed
    public int attackSpeed;//amount of times you can attack per second or per .7 seconds for melee
    public int projectileSpeed;//speed of arrows
    public bool isRanged;
    public bool canAttack;
    public bool isPotion;
    int itemDamage;

    void Start()
    {
        canAttack = true;
        isRanged = false;
        ChangeDamage();
        ChangeArmor();

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (canAttack) Attack();
        }
        else if (isPotion)
        {
            if (Heal(GetComponent<PlayerInventory>().activeItem.GetComponent<Item>().healthAmount))//method for potion
            {
                Destroy(GetComponent<PlayerInventory>().activeItem);
            }
        }
    }

    void FixedUpdate(){
        updateCounter += Time.fixedDeltaTime;
    }
    public void ChangeDamage()//sets damage to the player base damage, item damage, and multiplier
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
        if (isRanged)//this is for the bow stuff, change the bullet 1 prefab if u wanna change that pprojectile
        {
            if (GetComponent<PlayerInventory>().ammo > 0 && updateCounter>(1/attackSpeed))
            {
                updateCounter = 0;
                int force = -projectileSpeed;
                GameObject bulletSpawn = Instantiate(rangedAttack, attackPoint.position, attackPoint.rotation);
                bulletSpawn.GetComponent<Bullet>().SetDamage(totalDamage);
                Rigidbody2D rb = bulletSpawn.GetComponent<Rigidbody2D>();
                rb.AddForce(attackPoint.up * force, ForceMode2D.Impulse);
                GetComponent<PlayerInventory>().AddAmmo(-1);
            }
        }
        else if (updateCounter>(.7/attackSpeed))//this is for melee
        {
            updateCounter = 0;
            GameObject melee = Instantiate(meleeAttack, attackPoint.position, attackPoint.rotation);
            melee.GetComponent<Bullet>().SetDamage(totalDamage);
            Destroy(melee, .07f);
        }
    }

    void TakeDamage(int damage)//normal takeDamage, if it is less than 0 then die;
    {
        totalArmor = GetComponent<PlayerStats>().armor;
        GetComponent<PlayerStats>().currentHealth -= (int)(damage / totalArmor);
        if (GetComponent<PlayerStats>().currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<PlayerStats>().moveSpeed = 0;
        canAttack=false;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            Debug.Log(other.gameObject.GetComponent<EnemyAttack>().damage);
            TakeDamage(other.gameObject.GetComponent<EnemyAttack>().damage);
        }
    }
    public bool Heal(int health)//heals player either to the potion amount or to max
    {
        int difference = 0;
        if (gameObject.GetComponent<PlayerStats>().currentHealth + health < gameObject.GetComponent<PlayerStats>().maxHealth)
        {
            gameObject.GetComponent<PlayerStats>().currentHealth += health;
            return true;
        }
        else if (gameObject.GetComponent<PlayerStats>().maxHealth - gameObject.GetComponent<PlayerStats>().currentHealth != 0)
        {
            difference = gameObject.GetComponent<PlayerStats>().maxHealth - gameObject.GetComponent<PlayerStats>().currentHealth;
            gameObject.GetComponent<PlayerStats>().currentHealth += difference;
            return true;
        }
        else
        {
            return false;
        }
    }

}
