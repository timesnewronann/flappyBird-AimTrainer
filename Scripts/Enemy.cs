using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        // Handle the damage, e.g., reduce health
        Debug.Log(gameObject.name + " took " + damage + " damage!");

        // Optional: Destroy the enemy if health is 0
        // if (health <= 0) Destroy(gameObject);
    }
}
