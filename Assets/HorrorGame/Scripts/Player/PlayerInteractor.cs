using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    public Transform interactionOrigin;
    public float interactionRange = 3f;

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (!GameManager.PlayerInputEnabled)
        {
            GameManager.Instance.hud.ShowPrompt("");
            return;
        }

        IInteractable target = FindTarget();
        GameManager.Instance.hud.ShowPrompt(target == null ? "" : "[E]  " + target.GetPrompt());

        if (target != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            target.Interact();
    }

    IInteractable FindTarget()
    {
        if (Physics.Raycast(interactionOrigin.position, interactionOrigin.forward, out RaycastHit hit, interactionRange))
            return hit.collider.GetComponentInParent<IInteractable>();

        return null;
    }
}
