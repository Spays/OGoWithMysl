using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " получил урон: " + damage + " | Осталось: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

