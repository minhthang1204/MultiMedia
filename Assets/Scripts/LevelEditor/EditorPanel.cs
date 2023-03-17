using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorPanel : MonoSingleton<EditorPanel>
{
    [SerializeField] private bool isMainLevel = false;

    public ToolButton tileOption;
    public ToolButton ballOption;
    public ToolButton backgroundOption;

    [SerializeField] private Button saveButton;

    [SerializeField] private TMP_InputField idField;
    [SerializeField] private TMP_InputField maxDrawPointField;
    [SerializeField] private List<TMP_InputField> milestoneFields;

    [SerializeField] private float customUpdateRate;

    [Header("Panel")] [SerializeField] private Image fadeBackground;
    [SerializeField] private Ease fadeTweenEaseIn;
    [SerializeField] private Ease fadeTweenEaseOut;
    [SerializeField] private float fadeTweenTime;
    [SerializeField] private GameObject saveConfirmedPanel;

    private void Start()
    {
        StartCoroutine(CheckSaveButton());
        tileOption.Select();
    }

    IEnumerator CheckSaveButton()
    {
        while (true)
        {
            if (idField.text == "" || maxDrawPointField.text == "" || milestoneFields[0].text == "" ||
                milestoneFields[1].text == "" || milestoneFields[2].text == "")
            {
                saveButton.interactable = false;
            }
            else
            {
                saveButton.interactable = true;
            }

            yield return new WaitForSeconds(customUpdateRate);
        }
    }

    public void Save()
    {
        int currentBackgroundValue = backgroundOption.GetCurrentValue();
        int currentID = int.Parse(idField.text);
        int currentMaxPoint = int.Parse(maxDrawPointField.text);
        int currentMilestone1 = int.Parse(milestoneFields[0].text);
        int currentMilestone2 = int.Parse(milestoneFields[1].text);
        int currentMilestone3 = int.Parse(milestoneFields[2].text);

        LevelEditorManager.instance.SaveLevel(isMainLevel, currentID, currentMaxPoint, currentMilestone1,
            currentMilestone2, currentMilestone3, currentBackgroundValue);

        ShowSaveConfirmed();
    }

    public void ExitToMenu()
    {
        SceneController.instance.Load("Menu");
    }

    public void UnselectAll()
    {
        tileOption.Unselect();
        ballOption.Unselect();
        backgroundOption.Unselect();
    }

    private void ShowSaveConfirmed()
    {
        fadeBackground.gameObject.SetActive(true);
        fadeBackground.DOFade(0.7f, fadeTweenTime).SetEase(fadeTweenEaseIn);
        saveConfirmedPanel.transform.DOScale(new Vector3(1, 1, 1), fadeTweenTime).SetEase(fadeTweenEaseIn);

        fadeBackground.DOFade(0f, fadeTweenTime).SetEase(fadeTweenEaseOut).SetDelay(fadeTweenTime + 1.2f);
        saveConfirmedPanel.transform.DOScale(Vector3.zero, fadeTweenTime).SetEase(fadeTweenEaseOut).OnComplete(() =>
        {
            fadeBackground.gameObject.SetActive(false);
        }).SetDelay(fadeTweenTime + 1.2f);
    }

    protected override void InternalInit()
    {
    }

    protected override void InternalOnDestroy()
    {
    }

    protected override void InternalOnDisable()
    {
    }

    protected override void InternalOnEnable()
    {
    }
}