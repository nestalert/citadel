using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CreditCardValidator : MonoBehaviour
{
    public TMP_InputField cardNumberField;   //16 digits, formatted
    public TMP_InputField expiryField;       //MM/YY
    public TMP_InputField nameField;         //Letters + spaces
    public TMP_InputField cvcField;          //3 digits
    public Button purchaseButton;            //Purchase button (with PurchaseButton script attached)
    public Button buttonToDisable;
    public Button buttonToDisable2;

    public Color normalColor = Color.white;
    public Color errorColor = Color.red;

    private void Awake()
    {
        expiryField.onValueChanged.AddListener(FormatExpiryInput);
        cardNumberField.onValueChanged.AddListener(FormatCardNumberInput);
        cvcField.onValueChanged.AddListener(LimitCVCInput);
    }

    //Auto-format expiry MM/YY
    private void FormatExpiryInput(string value)
    {
        string digitsOnly = Regex.Replace(value, @"\D", "");

        if (digitsOnly.Length > 4)
            digitsOnly = digitsOnly.Substring(0, 4);

        //Validate first digit for month
        if (digitsOnly.Length >= 1)
        {
            int firstDigit = int.Parse(digitsOnly[0].ToString());
            if (firstDigit > 1)
            {
                digitsOnly = "0" + firstDigit;
            }
        }

        //Validate full month when two digits are typed
        if (digitsOnly.Length >= 2)
        {
            int month = int.Parse(digitsOnly.Substring(0, 2));
            if (month < 1 || month > 12)
            {
                digitsOnly = digitsOnly.Substring(0, 1);
            }
        }

        //Insert slash
        if (digitsOnly.Length >= 3)
        {
            expiryField.text = digitsOnly.Substring(0, 2) + "/" + digitsOnly.Substring(2);
        }
        else
        {
            expiryField.text = digitsOnly;
        }

        expiryField.caretPosition = expiryField.text.Length;
    }

    //Auto-format card number #### #### #### #### ---
    private void FormatCardNumberInput(string value)
    {
        string digitsOnly = Regex.Replace(value, @"\D", "");

        if (digitsOnly.Length > 16)
            digitsOnly = digitsOnly.Substring(0, 16);

        //Group into 4 digits
        string spaced = "";
        for (int i = 0; i < digitsOnly.Length; i++)
        {
            if (i > 0 && i % 4 == 0)
                spaced += " ";
            spaced += digitsOnly[i];
        }

        cardNumberField.text = spaced;
        cardNumberField.caretPosition = cardNumberField.text.Length;
    }

    public void ValidateInputs()
    {
        bool validCard = Regex.IsMatch(Regex.Replace(cardNumberField.text, @"\s", ""), @"^\d{16}$");
        bool validExpiry = ValidateExpiry(expiryField.text);
        bool validName = Regex.IsMatch(nameField.text, @"^[A-Za-z\s]+$");
        bool validCVC = Regex.IsMatch(cvcField.text, @"^\d{3}$");

        SetFieldColor(cardNumberField, validCard);
        SetFieldColor(expiryField, validExpiry);
        SetFieldColor(nameField, validName);
        SetFieldColor(cvcField, validCVC);

        if (validCard && validExpiry && validName && validCVC)
        {
            Debug.Log(" All credit card inputs are valid! Processing purchase...");

            //Run the purchase button logic
            if (purchaseButton != null)
            {
                purchaseButton.onClick.Invoke();
            }

            //Disable buttons after successful validation
            if (buttonToDisable != null)
            {
                buttonToDisable.interactable = false;
            }
            if (buttonToDisable2 != null)
            {
                buttonToDisable2.interactable = false;
            }
            if (purchaseButton != null)
            {
                purchaseButton.interactable = false;
            }
        }
        else
        {
            Debug.Log(" One or more credit card inputs are invalid!");
        }
    }

    private bool ValidateExpiry(string input)
    {
        string cleaned = input.Replace("/", "");
        if (!Regex.IsMatch(cleaned, @"^\d{4}$"))
            return false;

        string monthStr = cleaned.Substring(0, 2);
        string yearStr = cleaned.Substring(2, 2);

        if (!int.TryParse(monthStr, out int month) || !int.TryParse(yearStr, out int year))
            return false;

        return (month >= 1 && month <= 12) && (year >= 25 && year <= 50);
    }

    private void SetFieldColor(TMP_InputField field, bool isValid)
    {
        Image bg = field.GetComponent<Image>();
        if (bg != null)
        {
            bg.color = isValid ? normalColor : errorColor;
        }
    }

    private void LimitCVCInput(string value)
    {
        string digitsOnly = Regex.Replace(value, @"\D", "");

        if (digitsOnly.Length > 3)
            digitsOnly = digitsOnly.Substring(0, 3);

        cvcField.text = digitsOnly;
        cvcField.caretPosition = cvcField.text.Length;
    }
}
