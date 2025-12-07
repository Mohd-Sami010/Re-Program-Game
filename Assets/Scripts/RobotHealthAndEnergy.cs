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
        health = PlayerPrefs.GetFloat("RobotHealth", 100f);
        energy = PlayerPrefs.GetFloat("RobotEnergy", 100f);
    }
    private void Start()
    {
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
    }
    private bool updateUiAtStart = false;
    private void Update()
    {
        if (!updateUiAtStart)
        {
            UpdateUi();
            updateUiAtStart = true;
        }
    }
    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        DrainEnergy(0);
        TakeDamage(0);
    }

    public void TakeDamage(float damage)
    {
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GameState.GameOver) return;
        health -= damage;

        UpdateUi();
        SaveHealthAndEnergy();
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    public void DrainEnergy(float drainAmount)
    {
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GameState.GameOver) return;
        if (energy <= 0)
        {
            energy = 0;
            GameManager.Instance.GameOver(GameManager.GameOverType.robotOutOfEnergy);
        }
        energy -= drainAmount;

        UpdateUi();
        SaveHealthAndEnergy();
    }
    public void AddHealth(float healthToAdd)
    {
        health += healthToAdd;
        if (health > 100) health = 100;

        UpdateUi();
        SaveHealthAndEnergy();
    }
    public void AddEnergy(float energyToAdd)
    {
        energy += energyToAdd;
        if (energy > 100) energy = 100;

        UpdateUi();
        SaveHealthAndEnergy();
    }
    private void Die()
    {
        GameManager.Instance.GameOver(GameManager.GameOverType.robotDied);
        Debug.Log("Robot Died");
    }
    private void UpdateUi()
    {
        OnHealthOrEnergyChanged?.Invoke(this, new OnHealthOrEnergyChangedEventArgs
        {
            robotEnergy = energy,
            robotHealth = health
        });
    }
    private void SaveHealthAndEnergy()
    {
        PlayerPrefs.SetFloat("RobotHealth", health);
        PlayerPrefs.SetFloat("RobotEnergy", energy);
    }
    private void OnDestroy()
    {
        SaveHealthAndEnergy();
    }
}
