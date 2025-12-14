using UnityEngine;

public class DamageArea :MonoBehaviour {

    [Range(0f, 100f)]
    [SerializeField] private float damage = 40;
    [Space]
    [SerializeField] private bool damageOnStay = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Running) return;

        if (collision.TryGetComponent(out RobotHealthAndEnergy robotHealth))
        {
            robotHealth.TakeDamage(damage);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!damageOnStay) return;
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Running) return;
        if (collision.TryGetComponent(out RobotHealthAndEnergy robotHealth))
        {
            robotHealth.TakeDamage(damage * Time.deltaTime);
        }
    }
}
