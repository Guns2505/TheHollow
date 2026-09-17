using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Image progressFill;
    public Text percentText;
    public Text tipText;
    public float minimumDuration = 2.5f;

    public string[] tips =
    {
        "The monster hears you when you dash.",
        "Your flashlight battery does not last forever.",
        "Standing still makes you harder to hear.",
        "Find every key before you reach the exit door.",
        "Press F to turn the flashlight on and off.",
        "Corners are safer than long corridors."
    };

    IEnumerator Start()
    {
        GameManager.SetCursorLocked(false);
        tipText.text = tips[Random.Range(0, tips.Length)];

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneLoader.TargetScene);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (true)
        {
            timer += Time.unscaledDeltaTime;

            float loadProgress = Mathf.Clamp01(operation.progress / 0.9f);
            float timeProgress = Mathf.Clamp01(timer / minimumDuration);
            float progress = Mathf.Min(loadProgress, timeProgress);

            progressFill.fillAmount = progress;
            percentText.text = Mathf.RoundToInt(progress * 100f) + "%";

            if (progress >= 1f) break;

            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
