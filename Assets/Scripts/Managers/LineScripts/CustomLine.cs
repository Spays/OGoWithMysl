using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class CustomLine : MonoBehaviour
{
    public string lineType = "Default"; // Тип линии
    public float lifetime = 20f; // Время жизни линии
    public List<Vector3> points = new List<Vector3>();

    private LineRenderer lr;

    private EdgeCollider2D edgeCollider;
    public System.Collections.Generic.List<Vector2> colliderPoints = new System.Collections.Generic.List<Vector2>();
    
    
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void AddPoint(Vector3 point)
    {
        points.Add(point);
        lr.positionCount = points.Count;
        lr.SetPosition(points.Count - 1, point);
        
        // Добавление точки в коллайдер
        colliderPoints.Add(new Vector2(point.x, point.y));
        edgeCollider.points = colliderPoints.ToArray();
    }

    public void SetColor(Color color)
    {
        lr.startColor = color;
        lr.endColor = color;
    }
}