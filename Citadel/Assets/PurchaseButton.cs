using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PurchaseButton : MonoBehaviour
{
    public string itemNameToPurchase;
    public int itemPrice;

    public GameObject notEnoughMoneyPopup;

    private Button button;

    // Keep track of purchased items globally
    private static HashSet<string> purchasedItems = new HashSet<string>();

    void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnPurchaseClicked);
        }

        if (notEnoughMoneyPopup != null)
        {
            notEnoughMoneyPopup.SetActive(false);
        }

        // Disable button if item is already purchased
        if (purchasedItems.Contains(itemNameToPurchase))
        {
            DisableButton();
        }
    }

    void OnPurchaseClicked()
    {
        if (purchasedItems.Contains(itemNameToPurchase))
        {
            Debug.Log("Item already purchased: " + itemNameToPurchase);
            return; // prevent repurchase
        }

        if (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("CurrencyManager.Instance is null.");
            return;
        }

        if (CurrencyManager.Instance.RemoveMoney(itemPrice))
        {
            purchasedItems.Add(itemNameToPurchase);
            DisableButton();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.PurchaseItem(itemNameToPurchase);
            }
            else
            {
                Debug.LogWarning("UIManager.Instance is null.");
            }
        }
        else
        {
            Debug.Log("Not enough money!");
            if (notEnoughMoneyPopup != null)
            {
                notEnoughMoneyPopup.SetActive(true);
                Invoke(nameof(HidePopup), 2f);
            }
        }
    }

    void DisableButton()
    {
        if (button != null)
        {
            button.interactable = false;
        }
    }

    void HidePopup()
    {
        if (notEnoughMoneyPopup != null)
        {
            notEnoughMoneyPopup.SetActive(false);
        }
    }
}

