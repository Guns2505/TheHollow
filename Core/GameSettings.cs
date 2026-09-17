using UnityEngine;

public static class GameSettings
{
    public static float MouseSensitivity
    {
        get => PlayerPrefs.GetFloat("MouseSensitivity", 3f);
        set { PlayerPrefs.SetFloat("MouseSensitivity", value); PlayerPrefs.Save(); }
    }

    public static float MasterVolume
    {
        get => PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        set { PlayerPrefs.SetFloat("MasterVolume", value); PlayerPrefs.Save(); AudioListener.volume = value; }
    }

    public static bool Fullscreen
    {
        get => PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
            PlayerPrefs.Save();
            Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }
    }

    public static int QualityLevel
    {
        get => Mathf.Clamp(PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel()), 0, QualitySettings.names.Length - 1);
        set
        {
            PlayerPrefs.SetInt("QualityLevel", value);
            PlayerPrefs.Save();
            QualitySettings.SetQualityLevel(value);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Apply()
    {
        AudioListener.volume = MasterVolume;
        QualitySettings.SetQualityLevel(QualityLevel);
    }
}
