using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(HingeJoint2D))]
public class RopeCutter : MonoBehaviour
{
    public event Action onCut;

    HingeJoint2D joint;

    void Awake()
    {
        joint = GetComponent<HingeJoint2D>();
        var col = GetComponent<Collider2D>();
        col.isTrigger = true; // ловим пересечение триггером
    }

    void OnTriggerEnter2D(Collider2D other) => TryCut(other);
    void OnTriggerStay2D(Collider2D other) => TryCut(other);

    void TryCut(Collider2D other)
    {
        // Любая линия: проверяем признак наличия CustomLine где-нибудь выше по иерархии
        bool isLine = other.GetComponent<CustomLine>() || other.GetComponentInParent<CustomLine>();
        if (!isLine) return;

        if (joint) Destroy(joint); // разрываем связь вверх по цепи
        onCut?.Invoke();
        Destroy(this); // чтобы не триггерилось повторно
    }
}
