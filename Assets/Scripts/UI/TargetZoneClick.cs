using UnityEngine;
using UnityEngine.UI;

public class TargetZoneClick : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var btn = other.GetComponent<Button>() ?? other.GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick?.Invoke();
        }
    }
}
