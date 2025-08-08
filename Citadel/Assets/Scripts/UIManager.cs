using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Needed for Dictionary

public class UIManager : MonoBehaviour
{
    // Static instance of the UIManager
    public static UIManager Instance;

    [Header("Assignable UI Elements")]
    public TextMeshProUGUI infoText; // Your single TextMeshPro field
    public Button[] managedButtons; // Assign your buttons here in the Inspector

    // Optional: A dictionary to easily find buttons by name
    private Dictionary<string, Button> buttonDictionary = new Dictionary<string, Button>();

    void Awake()
    {
        // Implement the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Assuming this is already done by another script on the Canvas
        }
        else
        {
            // If another instance already exists, destroy this one
            Debug.LogWarning("Duplicate UIManager instance found, destroying this one.");
            Destroy(gameObject);
            return; // Prevent further execution
        }

        // Populate the button dictionary for easy access
        PopulateButtonDictionary();

        // Optionally, hide buttons by default
        SetAllButtonsActive(false);
    }

    // Populate the dictionary (useful if adding buttons dynamically or by reference)
    void PopulateButtonDictionary()
    {
        if (managedButtons != null)
        {
            buttonDictionary.Clear();
            foreach (Button btn in managedButtons)
            {
                if (btn != null && !buttonDictionary.ContainsKey(btn.gameObject.name))
                {
                    buttonDictionary.Add(btn.gameObject.name, btn);
                    // Debug.Log($"Added button '{btn.gameObject.name}' to dictionary.");
                }
                else if (btn != null)
                {
                     Debug.LogWarning($"Button with name '{btn.gameObject.name}' already exists in dictionary or is null.");
                }
            }
        }
    }

    // Public method to activate a specific button by name
    public void ActivateButton(string buttonName)
    {
        if (buttonDictionary.TryGetValue(buttonName, out Button button))
        {
            button.gameObject.SetActive(true);
            Debug.Log($"Activated button: {buttonName}");
        }
        else
        {
            Debug.LogWarning($"Button with name '{buttonName}' not found in managedButtons array.");
        }
    }

    // Public method to deactivate a specific button by name
    public void DeactivateButton(string buttonName)
    {
         if (buttonDictionary.TryGetValue(buttonName, out Button button))
        {
            button.gameObject.SetActive(false);
            Debug.Log($"Deactivated button: {buttonName}");
        }
        else
        {
            Debug.LogWarning($"Button with name '{buttonName}' not found in managedButtons array.");
        }
    }

    // Optional: Method to activate/deactivate all managed buttons
    public void SetAllButtonsActive(bool isActive)
    {
        if (managedButtons != null)
        {
            foreach (Button btn in managedButtons)
            {
                if(btn != null)
                    btn.gameObject.SetActive(isActive);
            }
        }
    }

    // You can also add the text update logic here or in a separate script
    public void UpdateInfoText(string text)
    {
        if (infoText != null)
        {
            infoText.text = text;
        }
    }

    // Public method to "purchase" an item and show its UI element
    public void PurchaseItem(string itemName)
    {
        ActivateButton(itemName);
        Debug.Log("Purchased: " + itemName);
    }

}