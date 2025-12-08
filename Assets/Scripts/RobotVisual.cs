using UnityEngine;

public class RobotVisual :MonoBehaviour {

    private Animator robotAnimator;

    private void Awake()
    {
        robotAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        RobotController.Instance.OnRobotStartMoving += RobotController_OnRobotStartMoving;
        RobotController.Instance.OnRobotStopMoving += RobotController_OnRobotStopMoving;
        RobotController.Instance.OnRobotJump += RobotController_OnRobotJump;
        RobotController.Instance.OnRobotLand += RobotController_OnRobotLand;
        RobotController.Instance.OnRobotInteract += RobotController_OnRobotInteract;

        RobotController.Instance.OnRobotPickItem += RobotController_OnRobotPickItem;
        RobotController.Instance.OnRobotDropItem += RobotController_OnRobotDropItem;

        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop += GameManager_OnGameStop; ;
    }

    private void RobotController_OnRobotPickItem(object sender, System.EventArgs e)
    {
        robotAnimator.SetBool("HoldingItem", true);
    }
    private void RobotController_OnRobotDropItem(object sender, System.EventArgs e)
    {
        robotAnimator.SetBool("HoldingItem", false);
    }
    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        robotAnimator.SetBool("Moving", false);
        robotAnimator.SetBool("HoldingItem", false);
    }
    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        robotAnimator.SetBool("Moving", false);
        robotAnimator.SetBool("HoldingItem", false);
    }
    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        robotAnimator.SetBool("Moving", false);
    }

    private void RobotController_OnRobotStartMoving(object sender, System.EventArgs e)
    {
        robotAnimator.SetBool("Moving", true);
    }
    private void RobotController_OnRobotStopMoving(object sender, System.EventArgs e)
    {
        robotAnimator.SetBool("Moving", false);
    }
    private void RobotController_OnRobotJump(object sender, System.EventArgs e)
    {
        robotAnimator.SetTrigger("Jump");
    }
    private void RobotController_OnRobotLand(object sender, System.EventArgs e)
    {
        robotAnimator.SetTrigger("Land");
    }
    private void RobotController_OnRobotInteract(object sender, System.EventArgs e)
    {
        robotAnimator.SetTrigger("Interact");
    }
}
