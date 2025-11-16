using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandSnippetsManager : MonoBehaviour {

    public static CommandSnippetsManager Instance { get; private set; }

    [SerializeField] private List<CommandSnippet> commandSnippets;
    private List<CommandSnippet> commandSnippetsToRun;
    private CommandSnippet currentCommand;
    int index = 0;
    private bool canGiveCommands = false;


    // Event for Move command
    public event EventHandler<OnMoveCommandEventArgs> OnMoveCommand;
    public class OnMoveCommandEventArgs : EventArgs {
        public float moveDuration;
    }

    // Event for Jump Command
    public event EventHandler<OnJumpCommandEventArgs> OnJumpCommand;
    public class OnJumpCommandEventArgs : EventArgs {
        public float jumpPower;
    }

    // Event for Turn Command
    public event EventHandler OnTurnCommand;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        canGiveCommands = false;
        index = 0;
        commandSnippetsToRun = null;
    }

    private void GameManager_OnGameStop(object sender, EventArgs e)
    {
        canGiveCommands = false;
        index = 0;
        commandSnippetsToRun = null;
    }

    private void GameManager_OnGameRestart(object sender, EventArgs e)
    {
        index = 0;
        commandSnippetsToRun = null;
        commandSnippetsToRun = commandSnippets
            .Select(snippet => snippet.Clone())
            .ToList();

        canGiveCommands = true;
    }

    private void Update()
    {
        if (commandSnippetsToRun == null || commandSnippetsToRun.Count == 0 || !canGiveCommands) return;
        currentCommand = commandSnippetsToRun[index];
        if (currentCommand.IsDone()) return;
        switch (currentCommand.GetCommandType())
        {
            case CommandSnippet.CommandType.Move:
                OnMoveCommand?.Invoke(this, new OnMoveCommandEventArgs { moveDuration = currentCommand.GetMoveDuration() });
                break;
            case CommandSnippet.CommandType.Jump:
                OnJumpCommand?.Invoke(this, new OnJumpCommandEventArgs
                {
                    jumpPower = currentCommand.GetJumpPower()
                });
                break;
            case CommandSnippet.CommandType.Turn:
                OnTurnCommand?.Invoke(this, EventArgs.Empty);
                break;
        }
    }
    public void ReadyForCommand(bool value = true) { canGiveCommands = value; }
    public void CommandAccepted()
    {
        index = (index + 1) % commandSnippetsToRun.Count;
        canGiveCommands = false;
        currentCommand.SetDone(true);
    }
    public void SetCommandSnippets(List<CommandSnippet> snippets) { commandSnippets = snippets; }
}
