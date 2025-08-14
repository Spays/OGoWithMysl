using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public int HP = 100;
    public static Transform Instance;
    
    public static event Action OnPlayerDeath;
    
    void Awake()
    {
        Instance = gameObject.transform;
    }

    public void OnEnable()
    {
        EnemyAI.HitPlayer += Hit;
    }

    public void OnDisable()
    {
        EnemyAI.HitPlayer -= Hit;
    }
    public void TakeDamage(int damage)
    {
        Debug.Log($"Игрок получил {damage} урона");

        // Здесь условно убиваем игрока
        if (damage >= 100)
        {
            Debug.Log("Игрок умер");
            OnPlayerDeath?.Invoke(); // 2. Вызываем событие
            Destroy(gameObject);
        }
    }

    public void Hit(int damage)
    {
        HP -= damage;
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
