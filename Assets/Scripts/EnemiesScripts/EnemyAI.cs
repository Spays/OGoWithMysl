using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    public Transform player;          // Игрок
    public int damage = 2;
    public float detectionRange = 5f; // Радиус обнаружения
    public float attackRange = 1f;    // Дистанция атаки
    public float moveSpeed = 3f;      // Скорость
    private float distance;

    private Vector2 startPosition;    // Точка спавна

    private Coroutine HitPlayerRoutine;
    
    public static event Action<int> HitPlayer;
    public void OnEnable()
    {
        PlayerStats.OnPlayerDeath += Patrol;
    }
    public void OnDisable()
    {
        PlayerStats.OnPlayerDeath -= Patrol;
    }
    
    void Start()
    {
        player = PlayerStats.Instance;
        startPosition = transform.position;
    }

    void Update()
    {
        
        if (player == null) return;

        
        distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (HitPlayerRoutine == null)
            {
                HitPlayerRoutine = StartCoroutine(HitingPlayer());
            }
        }
        else if (distance <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private System.Collections.IEnumerator HitingPlayer()
    {
        while (distance <= attackRange)
        {
            AttackPlayer();
            yield return new WaitForSeconds(0.5f);
        }
        
        HitPlayerRoutine =  null;
    }
    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );

        // Поворот к игроку (если нужно для спрайта)
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void AttackPlayer()
    {
        Debug.Log("Атакую игрока!");
        HitPlayer?.Invoke(damage);
        // Тут можно вызвать у игрока метод получения урона
        
    }

    void Patrol()
    {
        // Возврат на исходную позицию
        transform.position = Vector2.MoveTowards(
            transform.position,
            startPosition,
            moveSpeed * Time.deltaTime
        );
    }
}