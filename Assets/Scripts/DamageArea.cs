using UnityEngine;

public class DamageArea :MonoBehaviour {

    [Range(0f, 100f)]
    [SerializeField] private float damage = 40;

    [SerializeField] private bool damageOnExit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameplayRunning) return;
        if (damageOnExit) return;
        if (collision.TryGetComponent(out RobotHealthAndEnergy robotHealth))
        {
            robotHealth.TakeDamage(damage);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameplayRunning) return;
        if (!damageOnExit) return;
        if (collision.TryGetComponent(out RobotHealthAndEnergy robotHealth))
        {
            robotHealth.TakeDamage(damage);
        }
    }
}
