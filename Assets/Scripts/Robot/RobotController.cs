using System.Collections;
using UnityEngine;

public class RobotController :MonoBehaviour {
    public static RobotController Instance { get; private set; }
    private Rigidbody2D robotRigidbody;

    private Vector3 initialPosition;

    [SerializeField] private InteractionHandler interactionHandler;
    [SerializeField] private bool isHoldingItem = false;

    [Header("Energy Usage")]
    [SerializeField] private float energyDrainInMoving = 2f;
    [SerializeField] private float energyDrainInJump = 5f;
    [SerializeField] private float energyDrainInTurn = 6f;
    [SerializeField] private float energyDrainInInteraction = 6f;
    [SerializeField] private float energyDrainMultiplierWhileHoldingItem = 1.1f;
    private enum RobotState
    {
        None,
        Moving,
        Jumping,
        Turning,
        Interacting,
    }
    private RobotState robotState = RobotState.None;

    public event System.EventHandler OnRobotStartMoving;
    public event System.EventHandler OnRobotStopMoving;
    public event System.EventHandler OnRobotJump;
    public event System.EventHandler OnRobotLand;
    public event System.EventHandler OnRobotInteract;
    public event System.EventHandler OnRobotPickItem;
    public event System.EventHandler OnRobotDropItem;

    private void Awake()
    {
        Instance = this;
        robotRigidbody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void Start()
    {
        CommandManager.Instance.OnMoveCommand += CommandSnippetsManager_OnMoveCommand;
        CommandManager.Instance.OnJumpCommand += CommandSnippetsManager_OnJumpCommand;
        CommandManager.Instance.OnTurnCommand += CommandSnippetsManager_OnTurnCommand;
        CommandManager.Instance.OnInteractCommand += CommandSnippetsManager_OnInteractCommand;

        GroundCheck.Instance.OnGrounded += GroundCheck_OnGrounded;

        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }
    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        StopAllCoroutines();
        robotRigidbody.linearVelocity = Vector2.zero;
        robotState = RobotState.None;

        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            soundManager.StopRobotMoveSound();
            soundManager.StopRobotJumpSound();
        }
    }

    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        robotRigidbody.linearVelocity = Vector2.zero;
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
        robotRigidbody.linearVelocity = Vector2.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = initialPosition;
        robotState = RobotState.None;
    }

    private void GroundCheck_OnGrounded()
    {
        if (robotState == RobotState.Jumping)
        {
            robotRigidbody.linearVelocity = Vector2.zero;
            CommandManager.Instance.ReadyForCommand();
            robotState = RobotState.None;
            SoundManager.Instance.StopRobotJumpSound();
            
        }
        OnRobotLand?.Invoke(this, System.EventArgs.Empty);
    }

    private void CommandSnippetsManager_OnMoveCommand(object sender, CommandManager.OnMoveCommandEventArgs e)
    {
        if (robotState == RobotState.None && GroundCheck.Instance.IsGrounded())
        {
            CommandManager.Instance.CommandAccepted();

            robotState = RobotState.Moving;
            SoundManager.Instance.PlayRobotMoveSound();
            StartCoroutine(MoveCoroutine(e.moveDuration > 0 ? e.moveDuration : 0.05f));
            OnRobotStartMoving?.Invoke(this, System.EventArgs.Empty);
        }
    }
    private void CommandSnippetsManager_OnJumpCommand(object sender, CommandManager.OnJumpCommandEventArgs e)
    {
        if (robotState == RobotState.None && GroundCheck.Instance.IsGrounded())
        {
            CommandManager.Instance.CommandAccepted();

            robotState = RobotState.Jumping;
            float minJumpPower = 11f;
            float jumpPower = (e.jumpPower * 4)+ minJumpPower;
            robotRigidbody.linearVelocity = new Vector2(transform.localScale.x * jumpPower /2, jumpPower);
            DrainEnergy(energyDrainInJump + e.jumpPower/10);
            SoundManager.Instance.PlayRobotJumpSound();
            OnRobotJump?.Invoke(this, System.EventArgs.Empty);
        }
    }
    private void CommandSnippetsManager_OnTurnCommand(object sender, System.EventArgs e)
    {
        if (robotState == RobotState.None && GroundCheck.Instance.IsGrounded())
        {
            CommandManager.Instance.CommandAccepted();

            robotState = RobotState.Turning;
            DrainEnergy(energyDrainInTurn);
            SoundManager.Instance.PlayRobotTurnSound();
            StartCoroutine(TurnRobot());
        }
    }
    private void CommandSnippetsManager_OnInteractCommand(object sender, System.EventArgs e)
    {
        if (robotState == RobotState.None && GroundCheck.Instance.IsGrounded())
        {
            CommandManager.Instance.CommandAccepted();

            robotState = RobotState.Interacting;
            DrainEnergy(energyDrainInInteraction);
            SoundManager.Instance.PlayRobotTurnSound();
            float interactionDelay = 0.56f;
            InteractionHandler.InteractionType interactionType = interactionHandler.CheckInteractionType();
            if (interactionType == InteractionHandler.InteractionType.PickItem)
            {
                OnRobotPickItem?.Invoke(this, System.EventArgs.Empty);
            }
            else if (interactionType == InteractionHandler.InteractionType.DropItem)
            {
                interactionDelay = 0.2f;
                OnRobotDropItem?.Invoke(this, System.EventArgs.Empty);
            }
            else
            {
                OnRobotInteract?.Invoke(this, System.EventArgs.Empty);
            }
            StartCoroutine(Interact(interactionDelay));
        }
    }

    private IEnumerator MoveCoroutine(float moveDuration)
    {
        float timer = 0f;
        while (timer < moveDuration)
        {
            float moveSpeed = 3f;
            float rotation = transform.localScale.x;
            robotRigidbody.linearVelocity = new Vector2(moveSpeed * rotation, robotRigidbody.linearVelocity.y);
            DrainEnergy(energyDrainInMoving * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        robotRigidbody.linearVelocity = new Vector2(0f, robotRigidbody.linearVelocity.y);
        CommandManager.Instance.ReadyForCommand();
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
        CommandManager.Instance.ReadyForCommand();
        robotState = RobotState.None;
    }
    private IEnumerator Interact(float interactionDelay)
    {

        yield return new WaitForSeconds(interactionDelay);
        interactionHandler.Interact();
        yield return new WaitForSeconds(0.45f);
        CommandManager.Instance.ReadyForCommand();
        robotState = RobotState.None;
    }
    private void Update()
    {
        if (robotState == RobotState.None && robotRigidbody.linearVelocity.magnitude > 0f)
        {
            robotRigidbody.linearVelocity = new Vector2(0, robotRigidbody.linearVelocity.y);
        }
    }
    private void DrainEnergy(float drainAmount)
    {
        if (isHoldingItem)
        {
            drainAmount *= energyDrainMultiplierWhileHoldingItem;
        }
        RobotHealthAndEnergy.Instance.DrainEnergy(drainAmount);
    }
    private void OnDestroy()
    {
        CommandManager.Instance.OnMoveCommand -= CommandSnippetsManager_OnMoveCommand;
        CommandManager.Instance.OnJumpCommand -= CommandSnippetsManager_OnJumpCommand;
        CommandManager.Instance.OnTurnCommand -= CommandSnippetsManager_OnTurnCommand;

        GroundCheck.Instance.OnGrounded -= GroundCheck_OnGrounded;

        GameManager.Instance.OnGameRestart -= GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop -= GameManager_OnGameStop;
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }
    public bool IsHoldingItem()
    {
        return isHoldingItem;
    }
    public void SetIsHoldingItem(bool holding)
    {
        isHoldingItem = holding;
    }
}
