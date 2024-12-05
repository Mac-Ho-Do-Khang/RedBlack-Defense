using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;
    [SerializeField] int pointReward = 1;

    Bank bank;

    void Start()
    {
        bank = FindObjectOfType<Bank>();        
    }

    public void RewardGold()
    {
        if(bank == null) { return; }
        bank.Deposit(goldReward, pointReward);
    }

    public void StealGold()
    {
        if(bank == null) { return; }
        bank.Withdraw(goldPenalty);
    }
}
