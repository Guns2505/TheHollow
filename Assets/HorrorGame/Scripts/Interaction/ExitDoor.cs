using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    public Light signLight;
    public Color lockedColor = new Color(0.9f, 0.1f, 0.1f);
    public Color unlockedColor = new Color(0.2f, 1f, 0.3f);

    void Update()
    {
        if (signLight != null)
            signLight.color = GameManager.Instance.HasAllKeys ? unlockedColor : lockedColor;
    }

    public string GetPrompt()
    {
        if (GameManager.Instance.HasAllKeys) return "Open the exit door";
        return "Locked  -  " + GameManager.Instance.KeysCollected + " / " + GameManager.Instance.TotalKeys + " keys";
    }

    public void Interact()
    {
        if (!GameManager.Instance.HasAllKeys)
        {
            GameManager.Instance.hud.ShowMessage("The door is locked. Find every key first.");
            return;
        }

        GameManager.Instance.Win();
    }
}
