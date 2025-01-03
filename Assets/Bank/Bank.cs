using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    [SerializeField] int currentPoints = 0;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI displayBalance;
    [SerializeField] GameOverScreen game_over_screen;

    void Awake()
    {
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    public void Deposit(int gold_amount, int point_amount)
    {
        currentBalance += Mathf.Abs(gold_amount);
        currentPoints += Mathf.Abs(point_amount);
        UpdateDisplay();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();

        if(currentBalance < 0) // Lose the game;
        {
            game_over_screen.setup(currentPoints);
            Time.timeScale = 0f;
        }
    }

    void UpdateDisplay()
    {
        displayBalance.text = "$" + currentBalance;
    }
}
