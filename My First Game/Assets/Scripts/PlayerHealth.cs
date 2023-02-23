using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider playerHealth;

    public void SetHealth(float currentHp, float maxHp)
    {
        playerHealth.value = currentHp / maxHp;
    }
}
