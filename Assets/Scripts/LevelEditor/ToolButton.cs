using System;
using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour
{
    [SerializeField] private LevelEditorManager.EditorMode mode;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private int currentValue;

    [SerializeField] private Image currentSelectedImage;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Shape backgroundShape;

    private void Start()
    {
        currentValue = 0;
        leftButton.interactable = false;
        currentSelectedImage.sprite = sprites[currentValue];
    }

    public void ChangeLeft()
    {
        rightButton.interactable = true;
        if (currentValue == 1)
        {
            leftButton.interactable = false;
        }

        currentValue--;
        currentSelectedImage.sprite = sprites[currentValue];
    }

    public void ChangeRight()
    {
        leftButton.interactable = true;
        if (currentValue == sprites.Count-2)
        {
            rightButton.interactable = false;
        }

        currentValue++;
        currentSelectedImage.sprite = sprites[currentValue];
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    public void Select()
    {
        EditorPanel.instance.UnselectAll();
        backgroundShape.settings.fillColor = new Color32(213,177,149,255);
        
        LevelEditorManager.instance.SelectEditorMode(mode);
        
    }

    public void Unselect()
    {
        backgroundShape.settings.fillColor = new Color32(255,244,232,255);
    }

}
