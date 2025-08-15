using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Health : MonoBehaviour
{
    public int health = 100;
	
	public static event Action OnMobDeath;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " получил урон: " + damage + " | Осталось: " + health);

        if (health <= 0)
        {
			OnMobDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}

