using System.Collections;
using UnityEngine;

public class RobotController :MonoBehaviour {
    public static RobotController Instance { get; private set; }
    private Rigidbody2D robotRigidbody;

    private Vector3 initialPosition;
    private enum RobotState
    {
        None,
        Moving,
        Jumping,
        Turning,
    }
    private RobotState robotState = RobotState.None;

    public event System.EventHandler OnRobotStartMoving;
    public event System.EventHandler OnRobotStopMoving;
    public event System.EventHandler OnRobotJump;
    public event System.EventHandler OnRobotLand;

    private void Awake()
    {
        Instance = this;
        robotRigidbody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void Start()
    {
        CommandSnippetsManager.Instance.OnMoveCommand += CommandSnippetsManager_OnMoveCommand;
        CommandSnippetsManager.Instance.OnJumpCommand += CommandSnippetsManager_OnJumpCommand;
        CommandSnippetsManager.Instance.OnTurnCommand += CommandSnippetsManager_OnTurnCommand;

        GroundCheck.Instance.OnGrounded += GroundCheck_OnGrounded;

        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver; ;
    }

    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        StopAllCoroutines();
        robotRigidbody.velocity = Vector2.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = initialPosition;
        robotState = RobotState.None;

        SoundManager.Instance.StopRobotMoveSound();
        SoundManager.Instance.StopRobotJumpSound();
    }

    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        robotRigidbody.velocity = Vector2.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = initialPosition;
        robotState = RobotState.None;

        SoundManager.Instance.StopRobotMoveSound();
        SoundManager.Instance.StopRobotJumpSound();
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        SoundManager.Instance.StopRobotMoveSound();
        SoundManager.Instance.StopRobotJumpSound();

        StopAllCoroutines();
        robotRigidbody.velocity = Vector2.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = initialPosition;
        robotState = RobotState.None;
    }

    private void GroundCheck_OnGrounded()
    {
        if (robotState == RobotState.Jumping)
        {
            robotRigidbody.velocity = Vector2.zero;
            CommandSnippetsManager.Instance.ReadyForCommand();
            robotState = RobotState.None;
            SoundManager.Instance.StopRobotJumpSound();
            
        }
        OnRobotLand?.Invoke(this, System.EventArgs.Empty);
    }

    private void CommandSnippetsManager_OnMoveCommand(object sender, CommandSnippetsManager.OnMoveCommandEventArgs e)
    {
        if (robotState == RobotState.None && GroundCheck.Instance.IsGrounded())
        {
            CommandSnippetsManager.Instance.CommandAccepted();

            robotState = RobotState.Moving;
            SoundManager.Instance.PlayRobotMoveSound();
            StartCoroutine(MoveCoroutine(e.moveDuration));
            OnRobotStartMoving?.Invoke(this, System.EventArgs.Empty);
        }
    }
    private void CommandSnippetsManager_OnJumpCommand(object sender, CommandSnippetsManager.OnJumpCommandEventArgs e)
    {
        if (robotState == RobotState.None && GroundCheck.Instance.IsGrounded())
        {
            CommandSnippetsManager.Instance.CommandAccepted();

            robotState = RobotState.Jumping;
            float minJumpPower = 8f;
            float jumpPower = e.jumpPower + minJumpPower;
            robotRigidbody.velocity = new Vector2(transform.localScale.x * jumpPower /2, jumpPower);
            SoundManager.Instance.PlayRobotJumpSound();
            OnRobotJump?.Invoke(this, System.EventArgs.Empty);
        }
    }
    private void CommandSnippetsManager_OnTurnCommand(object sender, System.EventArgs e)
    {
        if (robotState == RobotState.None && GroundCheck.Instance.IsGrounded())
        {
            CommandSnippetsManager.Instance.CommandAccepted();

            robotState = RobotState.Turning;
            SoundManager.Instance.PlayRobotTurnSound();
            StartCoroutine(TurnRobot());
        }
    }
    private IEnumerator MoveCoroutine(float moveDuration)
    {
        float timer = 0f;
        while (timer < moveDuration)
        {
            float moveSpeed = 3f;
            float rotation = transform.localScale.x;
            robotRigidbody.velocity = new Vector2(moveSpeed * rotation, robotRigidbody.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }
        robotRigidbody.velocity = new Vector2(0f, robotRigidbody.velocity.y);
        CommandSnippetsManager.Instance.ReadyForCommand();
        robotState = RobotState.None;
        SoundManager.Instance.StopRobotMoveSound();
        OnRobotStopMoving?.Invoke(this, System.EventArgs.Empty);
    }
    private IEnumerator TurnRobot()
    {
        float duration = 0.5f;
        float timer = 0f;
        Vector3 finalScale = new Vector3(-transform.localScale.x, 1, 1);
        while (timer < duration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, finalScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = finalScale;
        CommandSnippetsManager.Instance.ReadyForCommand();
        robotState = RobotState.None;
    }
    private void Update()
    {
        if (transform.position.y < -10f)
        {
            RobotHealth.Instance.TakeDamage(100);
        }
        if (robotState == RobotState.None && robotRigidbody.velocity.magnitude > 0f)
        {
            robotRigidbody.velocity = new Vector2(0, robotRigidbody.velocity.y);
        }
    }
}
