using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public GameObject panel;
    public Text titleText;
    public Text messageText;
    public Button retryButton;
    public Button mainMenuButton;

    public Color victoryColor = new Color(0.5f, 1f, 0.6f);
    public Color defeatColor = new Color(0.9f, 0.15f, 0.15f);

    void Start()
    {
        retryButton.onClick.AddListener(OnRetry);
        mainMenuButton.onClick.AddListener(OnMainMenu);
        panel.SetActive(false);
    }

    public void ShowVictory()
    {
        panel.SetActive(true);
        titleText.text = "YOU ESCAPED";
        titleText.color = victoryColor;
        messageText.text = "You found every key and made it out alive.";
        retryButton.GetComponentInChildren<Text>().text = "Play Again";
    }

    public void ShowDefeat()
    {
        panel.SetActive(true);
        titleText.text = "YOU DIED";
        titleText.color = defeatColor;
        messageText.text = "It caught you in the dark.";
        retryButton.GetComponentInChildren<Text>().text = "Try Again";
    }

    public void OnRetry()
    {
        GameManager.Instance.Restart();
    }

    public void OnMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
