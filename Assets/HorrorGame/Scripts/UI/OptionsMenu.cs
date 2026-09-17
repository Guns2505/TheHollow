using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider sensitivitySlider;
    public Text sensitivityValue;
    public Slider volumeSlider;
    public Text volumeValue;
    public Button qualityPreviousButton;
    public Button qualityNextButton;
    public Text qualityValue;
    public Button fullscreenButton;
    public Text fullscreenValue;
    public Button backButton;

    GameObject previousPanel;
    int qualityIndex;
    bool fullscreen;

    public bool IsOpen => optionsPanel.activeSelf;

    void Start()
    {
        sensitivitySlider.minValue = 0.5f;
        sensitivitySlider.maxValue = 10f;
        sensitivitySlider.value = GameSettings.MouseSensitivity;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = GameSettings.MasterVolume;

        qualityIndex = GameSettings.QualityLevel;
        fullscreen = GameSettings.Fullscreen;

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        qualityPreviousButton.onClick.AddListener(PreviousQuality);
        qualityNextButton.onClick.AddListener(NextQuality);
        fullscreenButton.onClick.AddListener(ToggleFullscreen);
        backButton.onClick.AddListener(Back);

        RefreshLabels();
        optionsPanel.SetActive(false);
    }

    public void Open(GameObject previous)
    {
        previousPanel = previous;
        if (previous != null) previous.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void Back()
    {
        optionsPanel.SetActive(false);
        if (previousPanel != null) previousPanel.SetActive(true);
    }

    public void Close()
    {
        optionsPanel.SetActive(false);
    }

    void OnSensitivityChanged(float value)
    {
        GameSettings.MouseSensitivity = value;
        RefreshLabels();
    }

    void OnVolumeChanged(float value)
    {
        GameSettings.MasterVolume = value;
        RefreshLabels();
    }

    void PreviousQuality()
    {
        ChangeQuality(-1);
    }

    void NextQuality()
    {
        ChangeQuality(1);
    }

    void ChangeQuality(int step)
    {
        int count = QualitySettings.names.Length;
        qualityIndex = (qualityIndex + step + count) % count;
        GameSettings.QualityLevel = qualityIndex;
        RefreshLabels();
    }

    void ToggleFullscreen()
    {
        fullscreen = !fullscreen;
        GameSettings.Fullscreen = fullscreen;
        RefreshLabels();
    }

    void RefreshLabels()
    {
        sensitivityValue.text = sensitivitySlider.value.ToString("0.0");
        volumeValue.text = Mathf.RoundToInt(volumeSlider.value * 100f) + "%";
        qualityValue.text = QualitySettings.names[qualityIndex];
        fullscreenValue.text = fullscreen ? "Fullscreen" : "Windowed";
    }
}
