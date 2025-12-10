using UnityEngine;

public class FinishLine :MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GameOver(GameManager.GameOverType.win);
    }

}
