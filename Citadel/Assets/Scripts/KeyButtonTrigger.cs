using UnityEngine;
using UnityEngine.UI;

public class KeyButtonTrigger : MonoBehaviour
{
    public Button targetButton;      // The button to trigger
    public KeyCode activationKey;    // The key to press

    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            if (targetButton != null)
            {
                targetButton.onClick.Invoke(); // Simulate button press
            }
        }
    }
}
