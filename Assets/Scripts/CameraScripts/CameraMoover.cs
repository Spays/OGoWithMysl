using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Цель — персонаж
    public float smoothSpeed = 5f; // Скорость догоняния камеры
    public Vector3 offset;        // Смещение камеры относительно цели

    void FixedUpdate()
    {
        if (target == null) return;

        // Желаемая позиция камеры
        Vector3 desiredPosition = target.position + offset;

        // Плавное перемещение
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}