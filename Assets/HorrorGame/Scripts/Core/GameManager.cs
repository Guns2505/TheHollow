using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HUD hud;
    public PauseMenu pauseMenu;
    public EndScreen endScreen;

    int keysCollected;
    int totalKeys;
    GameState state = GameState.Playing;

    public GameState State => state;
    public int KeysCollected => keysCollected;
    public int TotalKeys => totalKeys;
    public bool HasAllKeys => keysCollected >= totalKeys;

    public static bool PlayerInputEnabled => Instance != null && Instance.state == GameState.Playing;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        totalKeys = FindObjectsByType<KeyItem>(FindObjectsSortMode.None).Length;
        hud.UpdateKeys(keysCollected, totalKeys);
        SetCursorLocked(true);
    }

    public void CollectKey()
    {
        keysCollected++;
        hud.UpdateKeys(keysCollected, totalKeys);
        hud.ShowMessage(HasAllKeys ? "All keys found. Escape through the exit door." : "You picked up a key.");
    }

    public void Pause()
    {
        if (state != GameState.Playing) return;
        state = GameState.Paused;
        Time.timeScale = 0f;
        SetCursorLocked(false);
        pauseMenu.Show();
    }

    public void Resume()
    {
        if (state != GameState.Paused) return;
        state = GameState.Playing;
        Time.timeScale = 1f;
        SetCursorLocked(true);
        pauseMenu.Hide();
    }

    public void Win()
    {
        if (state == GameState.Won || state == GameState.Lost) return;
        state = GameState.Won;
        Time.timeScale = 0f;
        SetCursorLocked(false);
        pauseMenu.Hide();
        endScreen.ShowVictory();
    }

    public void Lose()
    {
        if (state == GameState.Won || state == GameState.Lost) return;
        state = GameState.Lost;
        Time.timeScale = 0f;
        SetCursorLocked(false);
        pauseMenu.Hide();
        endScreen.ShowDefeat();
    }

    public void Restart()
    {
        SceneLoader.Load(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneLoader.Load(SceneLoader.MainMenuScene);
    }

    public static void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
