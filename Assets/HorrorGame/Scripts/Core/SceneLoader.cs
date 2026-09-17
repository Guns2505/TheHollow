using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public const string MainMenuScene = "MainMenu";
    public const string LoadingScene = "Loading";
    public const string FirstLevelScene = "Level01";

    public static string TargetScene = MainMenuScene;

    public static void Load(string sceneName)
    {
        TargetScene = sceneName;
        Time.timeScale = 1f;
        GameManager.SetCursorLocked(false);
        SceneManager.LoadScene(LoadingScene);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
