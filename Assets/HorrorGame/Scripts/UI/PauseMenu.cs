using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public OptionsMenu optionsMenu;
    public Button resumeButton;
    public Button optionsButton;
    public Button mainMenuButton;
    public Button quitButton;

    void Start()
    {
        resumeButton.onClick.AddListener(OnResume);
        optionsButton.onClick.AddListener(OnOptions);
        mainMenuButton.onClick.AddListener(OnMainMenu);
        quitButton.onClick.AddListener(OnQuit);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;

        GameState state = GameManager.Instance.State;
        if (state == GameState.Won || state == GameState.Lost) return;

        if (optionsMenu.IsOpen)
        {
            optionsMenu.Back();
            return;
        }

        if (state == GameState.Paused) GameManager.Instance.Resume();
        else GameManager.Instance.Pause();
    }

    public void Show()
    {
        pausePanel.SetActive(true);
    }

    public void Hide()
    {
        pausePanel.SetActive(false);
        optionsMenu.Close();
    }

    public void OnResume()
    {
        GameManager.Instance.Resume();
    }

    public void OnOptions()
    {
        optionsMenu.Open(pausePanel);
    }

    public void OnMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void OnQuit()
    {
        SceneLoader.QuitGame();
    }
}
