using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mob : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float damage;
    public float defense;
    public float moveSpeed;
    public float aggroDistance;
    public float attackRange;
    public bool isAlive;
    public bool canAttack;
    public bool isInRange;
    public bool destroy;
    public bool attackCooldown;
    public Vector3 startPos;


    public Rigidbody rb;
    public GameObject goldCoinDrop;
    public GameManager gameManager;
    public PlayerController player;

    public bool reachedRandomLocation;
    public Vector3 movementDirection;

    public virtual void DealDamage()
    {
        player.TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public virtual Vector3 MoveToPlayer()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        return playerDirection;
    }

    public virtual void Move()
    {
            float xDistance = Math.Abs(transform.position.x - player.transform.position.x);
            float zDistance = Math.Abs(transform.position.z - player.transform.position.z);
            if (xDistance <= attackRange && zDistance <= attackRange)
            {
                Attack();
                canAttack = true;
            }
            else if (xDistance <= aggroDistance && zDistance <= aggroDistance)
            {
                movementDirection = MoveToPlayer();
                isInRange = true;
            }
            else
            {
                movementDirection = transform.position;
            }

            if (!canAttack && isInRange)
            {
                transform.Translate(movementDirection.normalized * moveSpeed * Time.deltaTime);
            }
    }

    public void RotateMob(Vector3 direction)
    {

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * 2f);
    }

    public virtual void Attack()
    {
        canAttack = false;
    }

    public virtual Vector3 GenerateRandomMovementPosition()
    {
        float randomXDistance = Random.Range(-10, 10);
        float randomZDistance = Random.Range(-10, 10);
        Vector3 newPosition = new Vector3(transform.position.x + randomXDistance, transform.position.y, transform.position.z + randomZDistance);
        return newPosition;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        isAlive = true;
        canAttack = false;
        aggroDistance = 10;
        startPos = new Vector3(-20, 0.5f, 94);
        transform.position = startPos;
    }

    public void TakeKnockback(float knockback)
    {
        Vector3 playerDirection = transform.position - player.transform.position;
        rb.AddForce(playerDirection * knockback, ForceMode.Impulse);
    }
    
    public virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        destroy = true;
    }

    public IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(2);
        attackCooldown = false;
    }

    public IEnumerator EnableAttackHitBox(GameObject hitbox)
    {
        yield return new WaitForSeconds(0.5f);
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitbox.SetActive(false);
    }
}
