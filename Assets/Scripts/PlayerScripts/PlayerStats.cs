using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public int HP = 100;
	public int killers = 0;
    public static Transform Instance;
    
    public static event Action OnPlayerDeath;
    
    void Awake()
    {
        Instance = gameObject.transform;
    }

    public void OnEnable()
    {
        EnemyAI.HitPlayer += Hit;
		Health.OnMobDeath += AddKillers;
    }

    public void OnDisable()
    {
        EnemyAI.HitPlayer -= Hit;
		Health.OnMobDeath -= AddKillers;
    }
    public void TakeDamage(int damage)
    {
        Debug.Log($"Игрок получил {damage} урона");

        // Здесь условно убиваем игрока
        if (HP <= 0)
        {
			HP = 0;
            Debug.Log("Игрок умер");
            OnPlayerDeath?.Invoke(); // 2. Вызываем событие
            Destroy(gameObject);
        }
    }

    public void Hit(int damage)
    {
        HP -= damage;
		Debug.Log($"Игрок получил {damage} урона");

 		if (HP <= 0)
        {
			HP = 0;
            Debug.Log("Игрок умер");
            OnPlayerDeath?.Invoke(); // 2. Вызываем событие
            Destroy(gameObject);
        }
    }
	public void AddKillers(){
		killers += 1;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Instance = transform;
    }
}
