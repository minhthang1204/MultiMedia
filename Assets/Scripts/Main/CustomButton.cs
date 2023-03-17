using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioController.instance.PlayButtonHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioController.instance.PlayButtonClick();
    }
}
