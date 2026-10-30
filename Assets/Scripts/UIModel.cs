using System;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerModel
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public int Kills { get; private set; }
    public int KillsToComplete { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnKillsChanged;
    public event Action OnPlayerDied;
    public event Action OnLevelCompleted;

    private bool levelCompleted = false;

    public PlayerModel(float maxHealth, int killsToComplete = 5)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        KillsToComplete = killsToComplete;
    }

    public void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth = Math.Max(0, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0) OnPlayerDied?.Invoke();
    }

    public void AddKill()
    {
        if (CurrentHealth <= 0) return; 
        if (levelCompleted) return;

        Kills++;
        Debug.Log($"[Model] AddKill → Kills={Kills} / {KillsToComplete}");
        OnKillsChanged?.Invoke(Kills);

        if (Kills >= KillsToComplete)
        {
            levelCompleted = true;
            OnLevelCompleted?.Invoke();
        }
    }
}
