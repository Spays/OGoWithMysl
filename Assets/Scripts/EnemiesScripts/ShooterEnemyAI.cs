using UnityEngine;
using System;

public class ShooterEnemyAI : MonoBehaviour
{
    public Transform player;          // Игрок
    public int damage = 2;
    public float detectionRange = 15f; // Радиус обнаружения
    public float attackRange = 11f;    // Дистанция атаки
    public float stopDistance = 7f;
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

        if (player == null)
        {
            if (HitPlayerRoutine != null)
            {
                StopCoroutine(HitingPlayer());
            }
            return;
        }




        distance = Vector2.Distance(transform.position, player.position);

        if (player != null && distance <= attackRange)
        {
            if (HitPlayerRoutine == null)
            {
                HitPlayerRoutine = StartCoroutine(HitingPlayer());
            }
        }
        
        else if (distance <= detectionRange && distance  > stopDistance)
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
            if (player != null)
            {
                ChasePlayer();
                gameObject.GetComponent<GunController>().Shoot();
            }
            
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