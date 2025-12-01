using System;
using UnityEngine;

public class RobotHealthAndEnergy :MonoBehaviour {
    public static RobotHealthAndEnergy Instance { get; private set; }
    private float health = 100;
    private float energy = 100;

    public event EventHandler <OnHealthOrEnergyChangedEventArgs> OnHealthOrEnergyChanged;
    public class OnHealthOrEnergyChangedEventArgs :EventArgs
    {
        public float robotHealth;
        public float robotEnergy;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        DrainEnergy(0);

    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        UpdateUi();

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    public void DrainEnergy(float drainAmount)
    {
        if (energy <= 0)
        {
            energy = 0;
            GameManager.Instance.GameOver(GameManager.GameOverType.robotOutOfEnergy);
        }
        energy -= drainAmount;

        UpdateUi();
    }
    public void AddHealth(float healthToAdd)
    {
        health += healthToAdd;
        if (health > 100) health = 100;
        UpdateUi();
    }
    public void AddEnergy(float energyToAdd)
    {
        energy += energyToAdd;
        if (energy > 100) energy = 100;
        UpdateUi();
    }
    void Die()
    {
        GameManager.Instance.GameOver(GameManager.GameOverType.robotDied);
    }
    private void UpdateUi()
    {
        OnHealthOrEnergyChanged?.Invoke(this, new OnHealthOrEnergyChangedEventArgs
        {
            robotEnergy = energy,
            robotHealth = health
        });
    }
}
