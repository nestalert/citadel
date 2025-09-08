// KeyButtonTrigger.cs
using UnityEngine;
using UnityEngine.UI;

public class KeyButtonTrigger : MonoBehaviour
{
    public Button targetButton;
    public KeyCode activationKey;

    void Update()
    {
        // Check if the GameManager has indicated that an interactive canvas is active.
        if (GameManager.Instance != null && GameManager.Instance.isInteractiveCanvasActive)
        {
            return; // If it is, exit and do not process input.
        }

        if (Input.GetKeyDown(activationKey))
        {
            if (targetButton != null)
            {
                targetButton.onClick.Invoke();
            }
        }
    }
}