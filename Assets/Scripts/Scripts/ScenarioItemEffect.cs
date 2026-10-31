using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioItemEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image glowImage;
    public Color normalColor = new Color(0.4f, 0.4f, 0.4f, 0.4f);
    public Color hoverColor = new Color(0.2f, 0.85f, 1f, 0.95f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (glowImage != null)
        {
            glowImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (glowImage != null)
        {
            glowImage.color = normalColor;
        }
    }
}
