using UnityEngine;

public class DrawController : MonoBehaviour
{
    public GameObject linePrefab;
    public float stamina = 50f; // количество доступных точек
    public float staminaBuff = 0F;
    public float pointSpacing = 0.1f;
    public float StaminaReload = 1;
    public float LineCount = 5;

    private CustomLine currentLine;
    private Vector3 lastPointPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && stamina > 0 && LineCount > 0)
        {
            CreateNewLine();
        }

        if (Input.GetMouseButton(0) && currentLine != null && stamina > 0)
        {
            Vector3 mousePos = GetMouseWorldPos();
            mousePos.y -= 1.6f;
            mousePos.x += 0.4f;
            if (currentLine.points.Count == 0 || Vector3.Distance(lastPointPos, mousePos) >= pointSpacing)
            {
                currentLine.AddPoint(mousePos);
                lastPointPos = mousePos;
                stamina -= 1f; // уменьшаем стамину за точку
            }
        }

        
        if (stamina == 0)
        {
            currentLine = null;
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentLine = null;
        }
    }

    private void CreateNewLine()
    {
        Debug.Log("I Create a new Line type of base");
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
}