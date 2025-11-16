using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnippetsManagerUI :MonoBehaviour {

    public static SnippetsManagerUI Instance { get; private set; }

    private List<SnippetUI> snippetUIs = new();

    [SerializeField] private Button addMoveSnippetButton;
    [SerializeField] private Button addJumpSnippetButton;
    [SerializeField] private Button addTurnSnippetButton;
    [SerializeField] private Button closeUIButton;
    [Space]
    [SerializeField] private GameObject moveSnippetPrefab;
    [SerializeField] private GameObject jumpSnippetPrefab;
    [SerializeField] private GameObject turnSnippetPrefab;

    public event System.Action OnEnableDropArea;
    public event System.Action OnDisableDropArea;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        addMoveSnippetButton.onClick.AddListener(() => {
            Instantiate(moveSnippetPrefab, transform);
            SoundManager.Instance.PlaySnippetSpawnSound();
        });
        addJumpSnippetButton.onClick.AddListener(() => {
            Instantiate(jumpSnippetPrefab, transform);
            SoundManager.Instance.PlaySnippetSpawnSound();
        });
        addTurnSnippetButton.onClick.AddListener(() => {
            Instantiate(turnSnippetPrefab, transform);
            SoundManager.Instance.PlaySnippetSpawnSound();
        });
        closeUIButton.onClick.AddListener(() => {
            HUD.Instance.ShowEditButton();
            SoundManager.Instance.PlayUISound1();
            gameObject.SetActive(false);
        });
    }
    public void UpdateSnippetUIsList()
    {
        snippetUIs.Clear();
        SnippetUI snippetUI = StartSnippet.Instance.GetComponent<SnippetUI>().GetNextSnippet();
        while (snippetUI != null)
        {
            snippetUIs.Add(snippetUI);
            snippetUI = snippetUI.GetNextSnippet();
        }

        // Create a fresh CommandSnippet list
        List<CommandSnippet> commandSnippets = new List<CommandSnippet>();

        foreach (SnippetUI ui in snippetUIs)
        {
            CommandSnippet newSnippet = null;

            switch (ui.GetCommandType)
            {
                case CommandSnippet.CommandType.Move:
                    newSnippet = new CommandSnippet(
                        "Move",
                        CommandSnippet.CommandType.Move,
                        moveDuration: ui.GetValue,
                        jumpPower: 0f,
                        done: false
                    );
                    break;

                case CommandSnippet.CommandType.Jump:
                    newSnippet = new CommandSnippet(
                        "Jump",
                        CommandSnippet.CommandType.Jump,
                        moveDuration: 0f,
                        jumpPower: ui.GetValue,
                        done: false
                    );
                    break;

                case CommandSnippet.CommandType.Turn:
                    newSnippet = new CommandSnippet(
                        "Turn",
                        CommandSnippet.CommandType.Turn,
                        moveDuration: 0f,
                        jumpPower: 0f,
                        done: false
                    );
                    break;
            }

            if (newSnippet != null)
                commandSnippets.Add(newSnippet);
        }

        // Send the list to the CommandSnippetsManager
        CommandSnippetsManager.Instance.SetCommandSnippets(commandSnippets);

    }
    public void EnableDropArea()
    {
        OnEnableDropArea?.Invoke();
    }
    public void DisableDropArea()
    {
        OnDisableDropArea?.Invoke();
    }
}
