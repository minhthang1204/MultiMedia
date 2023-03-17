using System.Collections;
using System.Collections.Generic;
using Main;
using UnityEngine;

public class SceneLoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            //Todo: Make a method for loading an animation then callback, not just only 1 update
            isFirstUpdate = false;
            SceneController.instance.SceneLoaderCallback();
        }
    }
}