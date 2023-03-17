using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoSingleton<AudioController>
{
    [SerializeField] private AudioManager audioManager;

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

    #region Music ID

    #endregion


    public void Mute()
    {
        audioManager.MuteAll();
    }

    public void Unmute()
    {
        audioManager.UnmuteAll();
    }

    public void PlayMenuBackground()
    {
        audioManager.PlayMusic("MenuBG");
    }

    public void PlayGameBackground()
    {
        audioManager.PlayMusic("GameBG");
    }

    public void PlayEditorBackground()
    {
        audioManager.PlayMusic("EditorBG");
    }

    public void PlayButtonHover()
    {
        audioManager.PlayEffect("ButtonHover");
    }

    public void PlayButtonClick()
    {
        audioManager.PlayEffect("ButtonClick");
    }

    public void PlayLoseMilestone()
    {
        audioManager.PlayEffect("LoseMilestone");
    }

    public void PlayWinMilestone()
    {
        audioManager.PlayEffect("WinMilestone");
    }

    public void PlayDrawingSound()
    {
        audioManager.PlayEffect("Drawing");
    }

    public void MuteDrawingSound()
    {
        audioManager.MuteEffectSound("Drawing");
    }

    public void UnmuteDrawingSound()
    {
        audioManager.UnmuteEffectSound("Drawing");
    }

    public void PlayWinSound()
    {
        audioManager.PlayEffect("Win", true);
    }

    public bool CheckAudioStatus()
    {
        return audioManager.isMute;
    }
}