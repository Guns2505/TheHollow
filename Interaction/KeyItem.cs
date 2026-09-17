using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public float spinSpeed = 70f;
    public float floatAmount = 0.12f;
    public float floatSpeed = 2f;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatAmount;
    }

    public string GetPrompt()
    {
        return "Take the key";
    }

    public void Interact()
    {
        GameManager.Instance.CollectKey();
        Destroy(gameObject);
    }
}
