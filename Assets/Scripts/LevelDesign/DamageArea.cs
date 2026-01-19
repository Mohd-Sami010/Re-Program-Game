using UnityEngine;

public class DamageArea :MonoBehaviour {

    [Range(0f, 100f)]
    [SerializeField] private float damage = 40;
    [Space]
    [SerializeField] private bool damageOnStay = false;
    private bool isRobotInArea = false;

    private void Start()
    {
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop += GameManager_OnGameRestart;
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        isRobotInArea = false;
        if (damageOnStay)
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Running) return;
        
        if (collision.TryGetComponent(out RobotHealthAndEnergy robotHealth))
        {
            robotHealth.TakeDamage(damage);
        }
        if (damageOnStay)
        {
            PlaySound();
            isRobotInArea = true;
        }
        else SoundManager.Instance.PlayRobotHurtSound();
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
    private void Update()
    {
        if (damageOnStay && isRobotInArea)
        {
            RobotHealthAndEnergy.Instance.TakeDamage(damage * Time.deltaTime);
            if (!GetComponent<AudioSource>().isPlaying)
            {
                PlaySound();
            }
            if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Running && damageOnStay)
            {
                GetComponent<AudioSource>().Stop();
                isRobotInArea = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.Instance.GetCurrentGameState() != GameManager.GameState.Running) return;
        if (collision.TryGetComponent(out RobotHealthAndEnergy robotHealth))
        {
            isRobotInArea = false;
            if (damageOnStay)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
    }
    private void PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }
}
