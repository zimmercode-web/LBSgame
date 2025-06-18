using TMPro;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{

    public float startinMoney = 0f; // Starting money for the player
    public TextMeshProUGUI moneyText; // Reference to the UI text component to display money

    private float currentMoney; // Current amount of money the player has

    void Start()
    {
        currentMoney = startinMoney; // Initialize current money with starting amount
        UpdateMoneyUI(); // Update the UI to reflect the starting money
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount; // Increase current money by the specified amount
        UpdateMoneyUI(); // Update the UI to reflect the new amount of money
    }

    public bool spendMoney(float amount)
    {
        if (currentMoney >= amount) // Check if the player has enough money
        {
            currentMoney -= amount; // Deduct the specified amount from current money
            UpdateMoneyUI(); // Update the UI to reflect the new amount of money
            return true; // Return true indicating the transaction was successful
        }
        return false; // Return false indicating insufficient funds
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            if (moneyText.text != null)
            {
                moneyText.text = "$" + currentMoney.ToString("F2"); // Update the UI text to show the current money formatted to 2 decimal places
            }
        }
    }
}
