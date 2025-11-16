using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class CommandSnippet {

    public enum CommandType {
        Move,
        Jump,
        Turn,
    }

    [SerializeField] private string name;
    [SerializeField] private CommandType commandType;
    
    [Space]
    [Header("Moving")]
    [SerializeField] private float moveDuration = 0f;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 0f;

    private bool done = false;
    public CommandType GetCommandType() { return commandType; }
    public float GetMoveDuration() { return moveDuration; }
    public float GetJumpPower() { return jumpPower; }
    public bool IsDone() { return done; }
    public void SetDone(bool value) { done = value; }


    public CommandSnippet(string name, CommandType commandType, float moveDuration, float jumpPower, bool done)
    {
        this.name = name;
        this.commandType = commandType;
        this.moveDuration = moveDuration;
        this.jumpPower = jumpPower;
        this.done = done;
    }

    public CommandSnippet Clone()
    {
        return new CommandSnippet(name, commandType, moveDuration, jumpPower, done);
    }
}
