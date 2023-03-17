using System;
using System.Collections;
using System.Collections.Generic;
using Main;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Image loadingBar;

    #region Components

    

    #endregion

    #region Variable

    

    #endregion
    
    private void Update()
    {
        loadingBar.fillAmount = SceneController.instance.GetLoadingProgress();
    }
}
