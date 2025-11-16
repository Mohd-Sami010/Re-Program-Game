using UnityEngine;
using UnityEngine.UI;

public class SnippetUI :MonoBehaviour {
    public enum CommandType {
        Start,
        Move,
        Jump,
        Turn,
    }
    [SerializeField] private CommandType commandType;
    [SerializeField] private Image[] visualImages;
    private Color visualColorDefault;
    [SerializeField] private Color visualColorWhenRunning;
    [SerializeField] private Color visualColorWhenDone;
    [SerializeField] private TMPro.TMP_InputField valueInput;
    private SnippetUI nextSnippetUI;
    private float value;
    bool done = false;
    bool isRunning = false;

    private void Start()
    {
        visualColorDefault = visualImages[0].color;

        if (commandType == CommandType.Start)
        {
            GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        }
        else
        {
            valueInput.onValueChanged.AddListener(newValue => {
                if (float.TryParse(newValue, out float parsedValue)) value = parsedValue;
                else value = 0f;

                SnippetsManagerUI.Instance.UpdateSnippetUIsList();
            });
            if (commandType == CommandType.Jump)
            {
                valueInput.onSubmit.AddListener(newValue => {

                    if (float.TryParse(newValue, out float parsedValue)) value = parsedValue;
                    else value = 0f;

                    SnippetsManagerUI.Instance.UpdateSnippetUIsList();
                });
                valueInput.onDeselect.AddListener(newValue => {

                    if (float.TryParse(newValue, out float parsedValue)) value = parsedValue;
                    else value = 0f;

                    SnippetsManagerUI.Instance.UpdateSnippetUIsList();
                });
            }
        }

        GameManager.Instance.OnGameStop += ResetSnippetToDefault;
        if (commandType != CommandType.Start) GameManager.Instance.OnGameRestart += ResetSnippetToDefault;
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        ChangeVisualColor(visualColorWhenRunning);
    }

    private void ResetSnippetToDefault(object sender, System.EventArgs e)
    {
        done = false;
        isRunning = false;
        ChangeVisualColor(visualColorDefault);
    }

    public void SetNextSnippetUI(SnippetUI snippetUI)
    {
        nextSnippetUI = snippetUI;
    }
    public SnippetUI GetNextSnippet()
    {
        return nextSnippetUI;
    }
    public CommandType GetCommandType () { return commandType;}
    public float GetValue () { return value; }
    public bool IsDone() { return done; }
    public bool IsRunning() { return isRunning; }
    public void SetIsRunning(bool val)
    {
        isRunning = val;
        ChangeVisualColor(visualColorWhenRunning);
    }
    public void SetDone(bool val)
    {
        done = val;
        ChangeVisualColor(visualColorWhenDone);
    }
    private void ChangeVisualColor(Color newColor)
    {
        foreach (Image visualImage in visualImages)
        {
            visualImage.color = newColor;
            if (visualImage.GetComponent<Outline>() != null)
            {
                visualImage.GetComponent<Outline>().effectColor = newColor;
            }
        }
    }
    private void OnDestroy()
    {
        if (commandType == CommandType.Start)
        {
            GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        }
        GameManager.Instance.OnGameStop -= ResetSnippetToDefault;
        if (commandType != CommandType.Start) GameManager.Instance.OnGameRestart -= ResetSnippetToDefault;
    }
}
