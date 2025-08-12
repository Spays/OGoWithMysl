using UnityEngine;
using UnityEngine.UI;

public class StaminaUiSprite : MonoBehaviour
{
    public Image fillImage; // ссылка на UI Image
    public float maxStamina = 50f;
    public float currentStamina;
    public GameObject Player;

    void Start()
    {
        currentStamina = Player.GetComponent<DrawController>().stamina;
    }

    void Update()
    {
        currentStamina = Player.GetComponent<DrawController>().stamina;
        // Обновляем UI
        fillImage.fillAmount = currentStamina / maxStamina;
    }
}