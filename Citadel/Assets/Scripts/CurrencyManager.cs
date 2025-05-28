using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int _playerMoney = 0; 

    public int PlayerMoney
    {
        get { return _playerMoney; }
        private set // Private set to control how money is changed
        {
            _playerMoney = value;
            // You can add events here to update UI whenever money changes
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
}