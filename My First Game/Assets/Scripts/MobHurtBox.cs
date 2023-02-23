using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHurtBox : MonoBehaviour
{
    public RedSlime mob;
    public float health;
    public float takeKnockback;

    void Start()
    {
        mob = GetComponentInParent<RedSlime>();
        health = mob.currentHealth;
        takeKnockback = 0;
    }

    void Update()
    {
        if (takeKnockback > 0)
        {
            mob.TakeKnockback(takeKnockback);
            takeKnockback = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        mob.TakeDamage(damage);
    }
}
