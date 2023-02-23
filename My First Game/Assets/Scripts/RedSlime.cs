using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : Mob
{
    private Animator slimeAnimator;
    public GameObject attackHitbox;

    public RedSlime()
    {
        currentHealth = 20;
        maxHealth = 20;
        damage = 5;
        defense = 5;
        moveSpeed = 2;
        isAlive = true;
        aggroDistance = 10;
    }

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        destroy = false;
        attackCooldown = false;
        currentHealth = 20;
        isInRange = false;
        maxHealth = 20;
        damage = 5;
        defense = 5;
        moveSpeed = 2;
        isAlive = true;
        canAttack = false;
        aggroDistance = 10;
        attackRange = 1.5f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        reachedRandomLocation = true;
        movementDirection = transform.position;

        slimeAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamage();
        }
    }

    void Update()
    {
        if (isAlive)
        {
            if (canAttack)
            {
                Attack();
            }
            else
            {
                Move();
                RotateMob(movementDirection);
            }
            if (currentHealth <= 0)
            {
                isAlive = false;
                slimeAnimator.SetBool("isDead", true);
                StartCoroutine(Die());
            }
        }
        else
        {
            if (destroy)
            {
                Instantiate(goldCoinDrop, transform.position, transform.rotation);
                gameManager.MobKilled();
                Destroy(gameObject);
            }
        }
    }

    public override void Move()
    {
        float xDistance = Math.Abs(transform.position.x - player.transform.position.x);
        float zDistance = Math.Abs(transform.position.z - player.transform.position.z);
        if (xDistance <= attackRange && zDistance <= attackRange && !attackCooldown)
        {
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

        if (!canAttack && isInRange && !attackCooldown)
        {
            slimeAnimator.SetBool("isWalking", true);
            transform.Translate(movementDirection.normalized * moveSpeed * Time.deltaTime);
        }
    }

    public override void Attack()
    {
        slimeAnimator.SetTrigger("attack");
        slimeAnimator.SetBool("isWalking", false);
        StartCoroutine(AttackCooldown());
        StartCoroutine(EnableAttackHitBox(attackHitbox));
        canAttack = false;
        attackCooldown = true;
    }
}