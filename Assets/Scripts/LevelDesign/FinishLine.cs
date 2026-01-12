using UnityEngine;

public class FinishLine :MonoBehaviour {

    [SerializeField] private GameObject confettiParticle1;
    [SerializeField] private GameObject confettiParticle2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GameOver(GameManager.GameOverType.win);
        confettiParticle1.SetActive(true);
        confettiParticle2.SetActive(true);
    }

}
