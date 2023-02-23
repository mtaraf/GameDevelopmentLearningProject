using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using OpenCover.Framework.Model;
using TMPro;
using UnityEngine;
using File = System.IO.File;

public class PlayerController : MonoBehaviour
{
    public GameObject gameManagerObject;
    public GameObject inventory;
    public GameObject regularAttackHitBox;
    public GameObject specialAttackHitBox;
    public List<InventoryItemData> inventoryItems;
    public TextMeshProUGUI goldCoinsText;
    public GameObject campfire;

    private GameManager gameManager;
    private Animator playerAnimator;
    private Rigidbody playerRb;
    private PlayerHealth playerHealth;
    private InventorySystem inventorySystem;

    private float walkSpeed = 3.0f;
    private float rotationSpeed = 60.0f;
    private Vector3 startPosition = new Vector3(-5, 1, 82);
    private float verticalInput = 0;

    public float maxHealth;
    public float currentHealth;
    private int attack = 5;
    private int defense = 5;
    private int numCoins = 0;

    public bool isBlocking;
    public bool isAlive;
    public bool healthCooldown;



    void Start()
    {
        healthCooldown = false;
        isAlive = true;
        isBlocking = false;
        campfire = GameObject.Find("CampFire");
        inventorySystem = inventory.GetComponent<InventorySystem>();
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
        Debug.Log(gameManagerObject.GetComponent<GameManager>());
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();

        if (MainManager.instance != null)
        {
            if (MainManager.instance.isNewGame)
            {
                maxHealth = 40;
                currentHealth = 40;
                transform.position = startPosition;
            }
            else
            {
                LoadPlayerData();
            }
        }
    }

    private void Update()
    {
        if (gameManager.GetIsGameActive() && isAlive)
        {
            goldCoinsText.text = numCoins.ToString();
            verticalInput = Input.GetAxis("Vertical");
            MovePlayer(verticalInput);
            CombatMechanics();
            IsInRangeOfCampfire();
        }
        else
        {
            MovePlayer(0);
        }
    }

    public void IsInRangeOfCampfire()
    {
        float healingRange = 2.0f;
        float xDistance = Math.Abs(Math.Abs(transform.position.x) - Math.Abs(campfire.transform.position.x));
        float zDistance = Math.Abs(Math.Abs(transform.position.z) - Math.Abs(campfire.transform.position.z));
        if (xDistance <= healingRange && zDistance <= healingRange && !healthCooldown)
        {
            StartCoroutine(RegenerateHealth());
        }
    }


    IEnumerator RegenerateHealth()
    {
        healthCooldown = true;
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            playerHealth.SetHealth(currentHealth, maxHealth);
        }
        yield return new WaitForSeconds(0.5f);
        healthCooldown = false;
    }

    IEnumerator EnableAttackHitBox(GameObject hitbox)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitbox.SetActive(false);
    }

    void MovePlayer(float verticalInput)
    {
        if (verticalInput != 0 && !isBlocking)
        {
            playerAnimator.SetBool("walkForward", true);
            transform.Translate(Vector3.forward * verticalInput * walkSpeed * Time.deltaTime);
        }
        else
        {
            playerAnimator.SetBool("walkForward", false);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }

    void CombatMechanics()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetTrigger("attack");
            StartCoroutine(EnableAttackHitBox(regularAttackHitBox));

            isBlocking = false;
            playerAnimator.SetBool("isBlocking", false);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetTrigger("specialAttack");
            StartCoroutine(EnableAttackHitBox(specialAttackHitBox));

            isBlocking = false;
            playerAnimator.SetBool("isBlocking", false);
        }
        else if (Input.GetMouseButton(1))
        {
            isBlocking = true;
            playerAnimator.SetBool("isBlocking", true);
        }
        else
        {
            isBlocking = false;
            playerAnimator.SetBool("isBlocking", false);
        }
    }


    public void SavePlayerData()
    {
        if (MainManager.instance != null)
        {
            MainManager.instance.playerAttack = attack;
            MainManager.instance.playerDefense = defense;
            MainManager.instance.playerMaxHealth = maxHealth;
            MainManager.instance.playerCurrentHealth = currentHealth;
            MainManager.instance.playerPosition = transform.position;
            MainManager.instance.numCoins = numCoins;
            MainManager.instance.SavePlayerData();
        }
    }

    public void LoadPlayerData()
    {
        if (MainManager.instance != null)
        {
            attack = MainManager.instance.playerAttack;
            defense = MainManager.instance.playerDefense;
            maxHealth = MainManager.instance.playerMaxHealth;
            currentHealth = MainManager.instance.playerCurrentHealth;
            transform.position = MainManager.instance.playerPosition;
            numCoins = MainManager.instance.numCoins;
        }
        playerHealth.SetHealth(currentHealth, maxHealth);
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        playerHealth.SetHealth(currentHealth, maxHealth);
    }

    public void TakeKnockback(float knockback, Vector3 mobPosition)
    {
        Vector3 playerDirection = transform.position - mobPosition;
        playerRb.AddForce(playerDirection * knockback, ForceMode.Impulse);
    }

    public void GetAttacked(float damage, float knockback, Vector3 mobPosition)
    {
        playerAnimator.SetTrigger("isAttacked");
        if (isBlocking)
        {
            TakeKnockback(knockback, mobPosition);
        }
        else
        {
            TakeKnockback(knockback, mobPosition);
            TakeDamage(damage);
        }

        if (currentHealth <= 0)
        {
            playerAnimator.SetTrigger("die");
            isAlive = false;
        }
    }

    public float GetRegularAttackDamage()
    {
        return attack;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoldCoins"))
        {
            numCoins++;
            Destroy(collision.gameObject);
        }
    }
}
