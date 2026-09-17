using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public OptionsMenu optionsMenu;
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.SetCursorLocked(false);

        playButton.onClick.AddListener(OnPlay);
        optionsButton.onClick.AddListener(OnOptions);
        quitButton.onClick.AddListener(OnQuit);

        mainPanel.SetActive(true);
    }

    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;
        if (optionsMenu.IsOpen) optionsMenu.Back();
    }

    public void OnPlay()
    {
        SceneLoader.Load(SceneLoader.FirstLevelScene);
    }

    public void OnOptions()
    {
        optionsMenu.Open(mainPanel);
    }

    public void OnQuit()
    {
        SceneLoader.QuitGame();
    }
}
