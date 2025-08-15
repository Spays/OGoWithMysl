using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLineScript : MonoBehaviour
{
    public int damage = 10;
    
    private Coroutine HitEnemyRoutine;
    
    
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Health target = collision.gameObject.GetComponent<Health>();
        // Если у объекта есть компонент "Health" — наносим урон
        if (target != null)
        {
            if (HitEnemyRoutine == null)
            {
                HitEnemyRoutine = StartCoroutine(HitingEnemy(target));
            }
        }
        else
        {
            if (HitEnemyRoutine != null)
            {
                StopCoroutine(HitingEnemy(target));
            }
        }
    }
    
    private IEnumerator HitingEnemy(Health target)
    {
        while (target != null)
        {
            Debug.Log("Ауч!");
            target.TakeDamage(damage);
            
            yield return new WaitForSeconds(1f);
        }
        
        HitEnemyRoutine =  null;
    }
    
}
