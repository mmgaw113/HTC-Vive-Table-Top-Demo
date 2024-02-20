using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnButtonDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool pressed;
    bool entered;
    [SerializeField] private bool isLocationButton;
    [SerializeField] private bool isWristButton;
    public enum type
    {
        ZoomIn,
        ZoomOut,
    }

    [SerializeField] private Sprite originalImage;
    [SerializeField] private Sprite newImage;
    public XRTableTopInteractor tableTopInteractor;
    public type Type;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        entered = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        entered = false;
    }

    void Start()
    {
        originalImage = GetComponent<Image>().sprite;
    }

    private void Update()
    {
        if (!isLocationButton && !isWristButton)
        {
            if (pressed)
            {
                if (Type == type.ZoomIn)
                {
                    tableTopInteractor.ZoomInMap();
                }
                else if (Type == type.ZoomOut)
                {
                    tableTopInteractor.ZoomOutMap();
                }
            }
        }
    }
}
