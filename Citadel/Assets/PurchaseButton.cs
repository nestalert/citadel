using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public string itemNameToPurchase;
    public int itemPrice;

    public GameObject notEnoughMoneyPopup;

    private Button button;

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
    }

    void OnPurchaseClicked()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("CurrencyManager.Instance is null.");
            return;
        }

        if (CurrencyManager.Instance.RemoveMoney(itemPrice))
        {
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

    void HidePopup()
    {
        if (notEnoughMoneyPopup != null)
        {
            notEnoughMoneyPopup.SetActive(false);
        }
    }
}

