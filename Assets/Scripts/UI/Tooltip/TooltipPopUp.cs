using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class TooltipPopUp : MonoBehaviour
{

    [SerializeField] private GameObject popupCanvasObject;
    [SerializeField] private RectTransform popupObject;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Vector3 popupOffset;
    [SerializeField] private float padding;

    private Canvas popupCanvas;

    private void Awake()
    {
        popupCanvas = popupCanvasObject.GetComponent<Canvas>();
    }

    private void Update()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        if (!popupCanvas.isActiveAndEnabled) { return; }

        Vector3 newPosition = Input.mousePosition + popupOffset;
        newPosition.z = 0;
        float rightEdgeToScreenEdgeDistance = Screen.width - (newPosition.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
        if (rightEdgeToScreenEdgeDistance < 0)
        {
            newPosition.x += rightEdgeToScreenEdgeDistance;
        }
        float leftEdgeToScreenEdgeDistance = 0 - (newPosition.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
        if (leftEdgeToScreenEdgeDistance > 0)
        {
            newPosition.x += leftEdgeToScreenEdgeDistance;
        }
        float topEdgeToScreenEdgeDistance = Screen.height - (newPosition.y + popupObject.rect.height * popupCanvas.scaleFactor / 2);
        if (topEdgeToScreenEdgeDistance < 0)
        {
            newPosition.y += topEdgeToScreenEdgeDistance;
        }
        popupObject.transform.position = newPosition;
    }

    public void DisplayInfo(TooltipUI tooltip)
    {
        infoText.text = tooltip.BuildToolTipText();

        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void HideInfo()
    {
        popupCanvasObject.SetActive(false);
    }
}
