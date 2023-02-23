using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public PlayerController player;
    public float health;
    public float knockback;
    public Vector3 mobPosition;
    public bool isBlocking;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        health = player.getCurrentHealth();
        knockback = 0;
    }

    void Update()
    {
        health = player.getCurrentHealth();
        isBlocking = player.isBlocking;
        if (knockback > 0)
        {
            player.TakeKnockback(knockback, mobPosition);
            knockback = 0;
        }
    }

    public void GetAttacked(float damage, float knockback, Vector3 position)
    {
        if (player.isAlive)
        {
            player.GetAttacked(damage, knockback, mobPosition);
        }
    }

    public void TakeDamage(float damage)
    {
        player.TakeDamage(damage);
    }
}
