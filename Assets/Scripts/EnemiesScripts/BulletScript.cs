using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f; // Время жизни снаряда
    public int damage = 10;     // Урон

    void Start()
    {
        Destroy(gameObject, lifetime); // Автоудаление
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats target = collision.gameObject.GetComponent<PlayerStats>();
        CustomLine LineTarget = collision.gameObject.GetComponent<CustomLine>();
        
        if (LineTarget != null /*&& LineTarget.GetComponent<LineType>() != LineType.Attack*/)
        {
            
            Debug.Log("Попадание в линию");
            Destroy(gameObject); // Пуля исчезает при столкновении
        }
        
        if (target != null)
        {
            target.Hit(damage);
            Debug.Log("Попадание");
            Destroy(gameObject); // Пуля исчезает при столкновении
        }
        /*
        if (collision.gameObject.GetComponent<CustomLine>().lineType != LineType.Attack || !collision.gameObject.GetComponent<Health>())
        {
            Destroy(gameObject); // Пуля исчезает при столкновении
        }
        */
    }
}
