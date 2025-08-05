using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int _playerMoney = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI moneyText;

    public int PlayerMoney
    {
        get { return _playerMoney; }
        private set
        {
            _playerMoney = value;
            UpdateMoneyUI();
            Debug.Log("Player money updated to: " + _playerMoney);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative money. Use RemoveMoney instead.");
            return;
        }
        PlayerMoney += amount;
    }

    public bool RemoveMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot remove negative money. Use AddMoney instead.");
            return false;
        }

        if (PlayerMoney >= amount)
        {
            PlayerMoney -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough money to remove " + amount + ". Current money: " + PlayerMoney);
            return false;
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "" + _playerMoney;
        }
    }
}
