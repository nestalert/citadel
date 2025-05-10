using UnityEngine;
using TMPro; // Make sure to include the TMPro namespace
using UnityEngine.UI; // Include for Button if you need to reference buttons in code, though not strictly necessary for this setup

public class TextUpdater : MonoBehaviour
{
    public TextMeshProUGUI myTextMeshPro; // Public variable to hold your TextMeshPro text field

    // A public function that can be called by the buttons
    public void UpdateText(string newText)
    {
        if (myTextMeshPro != null)
        {
            myTextMeshPro.text = newText;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI reference is not set in TextUpdater script!");
        }
    }
}