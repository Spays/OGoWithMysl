using UnityEngine;

public class DrawController : MonoBehaviour
{
    public float currentStamina = 50f;
    public float maxStamina = 50f; // количество доступных точек
    public float staminaBuff = 0f;
    public float StaminaPrice = 1f;
    public float pointSpacing = 0.1f;
    public float StaminaReload = 1;
    public float LineCount = 5;

    private CustomLine currentLine;
    private Vector3 lastPointPos;
    private Coroutine staminaRestoreRoutine;

    public LineType selectedLineType = LineType.Defense;
    
    public GameObject deffenceLinePrefab;
    public GameObject attackLinePrefab;
    public GameObject defaultLinePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Получаем все возможные значения enum
            LineType[] types = (LineType[])System.Enum.GetValues(typeof(LineType));

            // Находим индекс текущего
            int currentIndex = System.Array.IndexOf(types, selectedLineType);

            // Переходим на следующий, если в конце — возвращаемся на 0
            int nextIndex = (currentIndex + 1) % types.Length;

            selectedLineType = types[nextIndex];

            Debug.Log("Выбран тип линии: " + selectedLineType);
        }
        
        if (Input.GetMouseButtonDown(0) && currentStamina > 0 && LineCount > 0)
        {
            CreateNewLine();
        }

        if (Input.GetMouseButton(0) && currentLine != null && currentStamina > 0)
        {
            Vector3 mousePos = GetMouseWorldPos();
            
            if (currentLine.points.Count == 0 || Vector3.Distance(lastPointPos, mousePos) >= pointSpacing)
            {
                currentLine.AddPoint(mousePos);
                lastPointPos = mousePos;
                currentStamina -= StaminaPrice; // уменьшаем стамину за точку
            }
        }
        
        
        if (currentStamina <= 0)
        {
            currentLine = null;
            currentStamina = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentLine = null;



            // Запускаем восстановление стамины, если ещё не запущено
            if (staminaRestoreRoutine == null)
            {
                staminaRestoreRoutine = StartCoroutine(StaminaRecovery());
            }

        }
    }

    private void CreateNewLine()
    {
        GameObject prefabToUse;

        switch (selectedLineType)
        {
            case LineType.Defense:
                prefabToUse = deffenceLinePrefab;
                break;
            case LineType.Attack:
                prefabToUse = attackLinePrefab;
                break;
            default:
                prefabToUse =  defaultLinePrefab;
                break;
        }
        
        GameObject newLineObj = Instantiate(prefabToUse);
        currentLine = newLineObj.GetComponent<CustomLine>();
        
        Debug.Log("I Create a new Line type of "  + selectedLineType);
        LineManager.Instance.AddLine(currentLine);
        lastPointPos = Vector3.zero;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        //mousePos.y -= 1.6f;
        //mousePos.x += 0.4f;
        mousePos.z = 10f; // расстояние до камеры
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private System.Collections.IEnumerator StaminaRecovery()
    {
        while (currentStamina < maxStamina)
        {
            //Player.GetComponent<DrawController>().staminaBuff += Player.GetComponent<PlayerStats>().StaminaReload;
            if (!Input.GetMouseButton(0))
            {
                currentStamina += StaminaReload;
                //Player.GetComponent<DrawController>().staminaBuff -= Player.GetComponent<DrawController>().StaminaReload;
                Debug.Log("Стамина добавилась буфер убавился");
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        staminaRestoreRoutine = null;
    }
}