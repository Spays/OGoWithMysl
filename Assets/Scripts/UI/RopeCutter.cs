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
        col.isTrigger = true; // ����� ����������� ���������
    }

    void OnTriggerEnter2D(Collider2D other) => TryCut(other);
    void OnTriggerStay2D(Collider2D other) => TryCut(other);

    void TryCut(Collider2D other)
    {
        // ����� �����: ��������� ������� ������� CustomLine ���-������ ���� �� ��������
        bool isLine = other.GetComponent<CustomLine>() || other.GetComponentInParent<CustomLine>();
        if (!isLine) return;

        if (joint) Destroy(joint); // ��������� ����� ����� �� ����
        onCut?.Invoke();
        Destroy(this); // ����� �� ������������ ��������
    }
}
