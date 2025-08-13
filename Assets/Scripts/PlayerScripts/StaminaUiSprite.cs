using UnityEngine;
using UnityEngine.UI;

public class StaminaUiSprite : MonoBehaviour
{
    public Image fillImage; // ссылка на UI Image
    public float maxStamina;
    public float currentStamina;
    public GameObject Player;

    void Start()
    {
        currentStamina = Player.GetComponent<DrawController>().currentStamina;
        maxStamina = Player.GetComponent<DrawController>().maxStamina;
    }

    void Update()
    {
        maxStamina = Player.GetComponent<DrawController>().maxStamina;
        currentStamina = Player.GetComponent<DrawController>().currentStamina;
        // Обновляем UI
        fillImage.fillAmount = currentStamina / maxStamina;
    }
}