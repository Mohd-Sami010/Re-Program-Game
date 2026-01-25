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
    public event Action OnRobotDamage;

    private const string HEALTH_PLAYERPREF = "RobotHealth";
    private const string ENERGY_PLAYERPREF = "RobotEnergy";

    private void Awake()
    {
        Instance = this;
        health = PlayerPrefs.GetFloat(HEALTH_PLAYERPREF, 100f);
        energy = PlayerPrefs.GetFloat(ENERGY_PLAYERPREF, 100f);
    }
    private void Start()
    {
        UpdateUi();
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
    }
    bool uiUpdated = false;
    private void Update()
    {
        if (!uiUpdated)
        {
            UpdateUi();
            uiUpdated = true;
        }
    }
    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        DrainEnergy(0);
        TakeDamage(0);
    }
    public void TakeDamage(float damage)
    {
        if (RobotController.Instance == null) return;
        health -= damage;

        UpdateUi();
        SaveHealthAndEnergy();
        if (damage > 0) OnRobotDamage?.Invoke();

        if (health <= 0)
        {
            health = 0;
            SaveHealthAndEnergy();
            Die();
        }
    }
    public void DrainEnergy(float drainAmount)
    {
        if (RobotController.Instance == null) return;
        if (energy <= 0)
        {
            energy = 0;
            SaveHealthAndEnergy();
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
        PlayerPrefs.SetFloat(HEALTH_PLAYERPREF, health);
        PlayerPrefs.SetFloat(ENERGY_PLAYERPREF, energy);
        PlayerPrefs.Save();
    }
    private void OnDestroy()
    {
        SaveHealthAndEnergy();
    }
}
