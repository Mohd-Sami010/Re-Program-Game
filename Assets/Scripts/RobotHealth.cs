using UnityEngine;

public class RobotHealth :MonoBehaviour {
    public static RobotHealth Instance { get; private set; }
    int health = 100;

    private void Awake()
    {
        Instance = this;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GameManager.Instance.GameOver(GameManager.GameOverType.lose);
    }

}
