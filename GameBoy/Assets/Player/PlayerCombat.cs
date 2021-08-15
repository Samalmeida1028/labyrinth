using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    private AudioManager Audio;

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
    public bool dead;
    int itemDamage;

    bool playSound;

    void Start()
    {
        playSound = true;
        canAttack = false;
        isRanged = false;
        ChangeDamage();
        ChangeArmor();

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (canAttack && !isRanged) Attack();
        }
        else if (isPotion)
        {
            if (Input.GetMouseButton(0))
            {
                if (Heal(GetComponent<PlayerInventory>().activeItem.GetComponent<Item>().healthAmount))//method for potion
                {
                    GetComponent<PlayerInventory>().clearSlot(2);
                    isPotion=false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        updateCounter += Time.fixedDeltaTime;
    }
    
    public void ChangeDamage()//sets damage to the player base damage, item damage, and multiplier
    {
        int baseDamage = GetComponent<PlayerStats>().baseDamage;
        float damageMult = GetComponent<PlayerStats>().damageMult;

        if (GetComponent<PlayerInventory>().activeItem != null)
        {
            itemDamage = GetComponent<PlayerInventory>().activeItem.damage;
        }
        else { itemDamage = 0; }
        totalDamage = (int)((baseDamage + itemDamage) * damageMult);
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
        if (isRanged && !GetComponent<PlayerMovement>().hitDB)//this is for the bow stuff, change the bullet 1 prefab if u wanna change that pprojectile
        {
            if (GetComponent<PlayerInventory>().ammo > 0 && updateCounter>(1/attackSpeed))
            {
                // Locks shooting into 3 different angles on either side 
                // Ethan did this to make everything looks better
                // This change will make the bow more skillful aswell
                if (GetComponent<PlayerMovement>().angle != 0 && GetComponent<PlayerMovement>().angle != -180)
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
        }
        else if (updateCounter>(.7/attackSpeed))//this is for melee
        {
            updateCounter = 0;
            GameObject melee = Instantiate(meleeAttack, attackPoint.position, attackPoint.rotation);
            melee.GetComponent<Bullet>().SetDamage(totalDamage);
            Destroy(melee, .07f);
            FindObjectOfType<AudioManager>().PlayOneShot("SwordSlash");
            
        }
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(0.7f);
        playSound = true;
    }

    void TakeDamage(int damage)//normal takeDamage, if it is less than 0 then die;
    {

        ChangeArmor();
        GetComponent<PlayerStats>().UpdateHealth((int)(damage / totalArmor));

        FindObjectOfType<AudioManager>().PlayOneShot("Enemy_Hit");

        GetComponent<PlayerMovement>().TakeDamage();
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
            if(dead==false)
            {
                TakeDamage(other.gameObject.GetComponent<EnemyAttack>().damage); 
            }
        }
    }
    public bool Heal(int health)//heals player either to the potion amount or to max
    {
        int difference = 0;
        if (gameObject.GetComponent<PlayerStats>().currentHealth + health < gameObject.GetComponent<PlayerStats>().maxHealth)
        {
            gameObject.GetComponent<PlayerStats>().UpdateHealth(-health);
            return true;
        }
        else if (gameObject.GetComponent<PlayerStats>().maxHealth - gameObject.GetComponent<PlayerStats>().currentHealth != 0)
        {
            difference = gameObject.GetComponent<PlayerStats>().maxHealth - gameObject.GetComponent<PlayerStats>().currentHealth;
            gameObject.GetComponent<PlayerStats>().UpdateHealth(-difference);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Clear()
    {
        canAttack=false;
        isRanged=false;
        isPotion=false;
    }

}
