using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    public float pointSpacing = 0.1f; // минимальное расстояние между точками линии
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // пока ничего не рисуем
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Начинаем новую линию
            //points.Clear();
            //lineRenderer.positionCount = 0;
            AddPoint(GetMouseWorldPos());
        }
        else if (Input.GetMouseButton(0))
        {
            // Продолжаем рисование
            Vector3 mousePos = GetMouseWorldPos();
            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], mousePos) >= pointSpacing)
            {
                AddPoint(mousePos);
            }
        }
    }

    private void AddPoint(Vector3 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // расстояние от камеры до игрового поля
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}