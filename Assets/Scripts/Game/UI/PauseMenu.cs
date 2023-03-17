using System.Collections;
using System.Collections.Generic;
using Main;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI audioStatus;
    public void OpenPausePanel()
    {
        CheckAudioStatus();
    }
    
    public void Resume()
    {
        GameManager.instance.UnPause(true);
    }

    public void ExitToMenu()
    {
        SceneController.instance.Load("Menu", () =>
        {
            Time.timeScale = 1;
        });
    }

    private void CheckAudioStatus()
    {
        if (AudioController.instance.CheckAudioStatus())
        {
            audioStatus.text = "AUDIO: OFF";
        }
        else
        {
            audioStatus.text = "AUDIO: ON";
        }
    }

    public void ToggleAudio()
    {
        if (!AudioController.instance.CheckAudioStatus())
        {
            AudioController.instance.Mute();
            audioStatus.text = "AUDIO: OFF";
        }
        else
        {
            AudioController.instance.Unmute();
            audioStatus.text = "AUDIO: ON";
            AudioController.instance.MuteDrawingSound();
        }
    }

}
