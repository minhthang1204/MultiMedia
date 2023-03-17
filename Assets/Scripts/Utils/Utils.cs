using UnityEngine;


public static class Utils
{
    public static void ResetAllAnimatorParameters(this Animator animator, AnimatorControllerParameterType type)
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == type)
            {
                if (type == AnimatorControllerParameterType.Trigger) animator.ResetTrigger(trigger.name);
                else if (type == AnimatorControllerParameterType.Bool)
                {
                    animator.SetBool(trigger.name, false);
                }
            }
        }
    }

    public static void SaveLevelStats(int levelID, bool isMainLevel, int point)
    {
        string key = "";
        int pointToSave = 0;
        if (isMainLevel)
        {
            key = $"MainLevel{levelID}";
            PlayerPrefs.SetInt($"MainLevel{levelID}", point);
        }
        else
        {
            key = $"CustomLevel{levelID}";
            PlayerPrefs.SetInt($"CustomLevel{levelID}", point);
        }

        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetInt(key) < point)
            {
                pointToSave = point;
            }
            else
            {
                pointToSave = PlayerPrefs.GetInt(key);
            }
        }
        else 
        {
            pointToSave = point;
        }
        
        PlayerPrefs.SetInt(key, pointToSave);
    }

    public static int GetLevelStats(int levelID, bool isMainLevel)
    {
        int point;

        if (isMainLevel)
        {
            point = PlayerPrefs.GetInt($"MainLevel{levelID}", -1);
        }
        else
        {
            point = PlayerPrefs.GetInt($"CustomLevel{levelID}", -1);
        }

        return point;
    }
}