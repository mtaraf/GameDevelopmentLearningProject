using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHitBox : MonoBehaviour
{
    public RedSlime mob;
    public float damage;
    public float knockback;

    void Start()
    {
        mob = GetComponentInParent<RedSlime>();
        damage = mob.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHurtBox"))
        {
            HurtBox hurtBox = other.GetComponent<HurtBox>();
            hurtBox.GetAttacked(damage, knockback, transform.position);
        }
    }
}
