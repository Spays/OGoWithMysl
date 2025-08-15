using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // скорость передвижения
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator Player_Animator;

    void Start()
    {
        Player_Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ввод с клавиатуры
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if (!Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("LOMisl_IdleAnimation") && movement.x == 0 && movement.y == 0)
        {
            Player_Animator.SetTrigger("Idle_OMisl");
        }
        else if (!Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("OMisl_rightrunAnimation") && movement.x > 0 && Math.Abs(movement.x) > Math.Abs(movement.y))
        {
            Player_Animator.SetTrigger("RightRun_OMisl");
        }
        else if (!Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("OMisl_leftrunAnimation") && movement.x < 0 && Math.Abs(movement.x) > Math.Abs(movement.y))
        {
            Player_Animator.SetTrigger("LeftRun_OMisl");
        }
        else if (!Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("OMisl_backrunAnimation") && movement.y > 0 && Math.Abs(movement.y) > Math.Abs(movement.x))
        {
            Player_Animator.SetTrigger("BackRun_OMisl");
        }
        else if (!Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("OMisl_frontrunAnimation") && movement.y < 0 && Math.Abs(movement.y) > Math.Abs(movement.x))
        {
            Player_Animator.SetTrigger("FrontRun_OMisl");
        }
        
        else if (!Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("OMisl_rightrunAnimation") && movement.x > 0 && Math.Abs(movement.x) == Math.Abs(movement.y))
        {
            Player_Animator.SetTrigger("RightRun_OMisl");
        }
        else if (!Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("OMisl_leftrunAnimation") && movement.x < 0 && Math.Abs(movement.x) == Math.Abs(movement.y))
        {
            Player_Animator.SetTrigger("LeftRun_OMisl");
        }
        
        

        /*
        // Нормализация вектора
        movement = movement.normalized;

        // Поворот в сторону движения
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
        */
    }

    void FixedUpdate()
    {
        
        
        // Передвижение
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
