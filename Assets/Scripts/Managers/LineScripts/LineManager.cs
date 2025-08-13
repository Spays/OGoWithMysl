using UnityEngine;
using System.Collections.Generic;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance;
    public List<CustomLine> allLines = new List<CustomLine>();

    public float pointRemoveInterval = 0.1f; // время между удалением точек
    public GameObject Player;

    void Awake()
    {
        Instance = this;
    }

    public void AddLine(CustomLine newLine)
    {
        allLines.Add(newLine);
        StartCoroutine(RemovePointsOverTime(newLine));

        Player.GetComponent<DrawController>().LineCount -= 1;
    }

    private System.Collections.IEnumerator RemovePointsOverTime(CustomLine line)
    {
        // Ждём, пока линия будет существовать с полным количеством точек
        yield return new WaitForSeconds(1f); // пауза перед началом удаления (можно убрать)

        while (line != null && line.points.Count > 0)
        {
            // Удаляем первую точку
            line.points.RemoveAt(0);
            line.colliderPoints.RemoveAt(0);
            //Player.GetComponent<DrawController>().staminaBuff += Player.GetComponent<DrawController>().StaminaReload;
            //Debug.Log("буфер добавился");

            // Обновляем LineRenderer
            line.GetComponent<LineRenderer>().positionCount = line.points.Count;
            for (int i = 0; i < line.points.Count; i++)
            {
                line.GetComponent<LineRenderer>().SetPosition(i, line.points[i]);
                line.GetComponent<EdgeCollider2D>().points = line.colliderPoints.ToArray();
            }
            
            
            
            /*
            if (!Input.GetMouseButton(0)  && Player.GetComponent<DrawController>().staminaBuff != 0)
            {
                Player.GetComponent<DrawController>().currentStamina += Player.GetComponent<DrawController>().StaminaReload;
                Player.GetComponent<DrawController>().staminaBuff -= Player.GetComponent<DrawController>().StaminaReload;
                Debug.Log("Стамина добавилась буфер убавился");
            }
            */
            
            yield return new WaitForSeconds(pointRemoveInterval);

        }
        
        // Когда точек не осталось — удаляем объект
        if (line != null)
        {
            allLines.Remove(line);
            Destroy(line.gameObject);
            //Player.GetComponent<DrawController>().stamina += Player.GetComponent<PlayerStats>().StaminaReload;
            Player.GetComponent<DrawController>().LineCount += 1;
        }
        
        
    }
    
}