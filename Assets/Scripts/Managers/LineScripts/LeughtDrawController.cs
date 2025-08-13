using UnityEngine;

public class LeughtDrawController : MonoBehaviour
{
    public GameObject linePrefab;
    public float maxLineLength = 10f; // максимальная длина линии
    public float staminaBuff = 0f;    
    public float pointSpacing = 0.1f;

    private CustomLine currentLine;
    private Vector3 lastPointPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.GetComponent<DrawController>().LineCount > 0)
        {
            CreateNewLine();
        }

        if (Input.GetMouseButton(0) && currentLine != null)
        {
            Vector3 mousePos = GetMouseWorldPos();
            mousePos.y -= 1.6f;
            mousePos.x += 0.4f;

            if (currentLine.points.Count == 0 || Vector3.Distance(lastPointPos, mousePos) >= pointSpacing)
            {
                float currentLength = GetLineLength(currentLine);

                // считаем, какой отрезок мы хотим добавить
                float segmentLength = currentLine.points.Count > 0
                    ? Vector3.Distance(currentLine.points[currentLine.points.Count - 1], mousePos)
                    : 0f;

                // Если новая точка не превысит лимит — добавляем её
                if (currentLength + segmentLength <= maxLineLength + staminaBuff)
                {
                    currentLine.AddPoint(mousePos);
                    lastPointPos = mousePos;
                }
                else
                {
                    // Если превысит — ставим точку ровно на границе
                    float remainingLength = (maxLineLength + staminaBuff) - currentLength;
                    if (remainingLength > 0)
                    {
                        Vector3 lastPoint = currentLine.points[currentLine.points.Count - 1];
                        Vector3 limitedPoint = lastPoint + (mousePos - lastPoint).normalized * remainingLength;
                        currentLine.AddPoint(limitedPoint);
                    }
                    currentLine = null; // прекращаем рисование
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentLine = null;
        }
    }

    private void CreateNewLine()
    {
        Debug.Log("I Create a new Line");
        GameObject newLineObj = Instantiate(linePrefab);
        currentLine = newLineObj.GetComponent<CustomLine>();
        LineManager.Instance.AddLine(currentLine);
        lastPointPos = Vector3.zero;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // расстояние до камеры
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private float GetLineLength(CustomLine line)
    {
        float length = 0f;
        for (int i = 1; i < line.points.Count; i++)
        {
            length += Vector3.Distance(line.points[i - 1], line.points[i]);
        }
        return length;
    }
}
