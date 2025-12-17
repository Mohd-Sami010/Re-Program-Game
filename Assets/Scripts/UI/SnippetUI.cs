using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SnippetUI :MonoBehaviour {
    public enum CommandType {
        Start,
        Move,
        Jump,
        Turn,
        Interact,
    }

    [SerializeField] private CommandType commandType;
    [SerializeField] private Image[] visualImages;
    [SerializeField] private Color visualColorWhenRunning;
    [SerializeField] private Color visualColorWhenDone;
    [SerializeField] private TMP_InputField valueInput;

    private Color visualColorDefault;
    private SnippetUI nextSnippetUI;

    private float value = 1f;
    private bool done;
    private bool isRunning;

    private void Start()
    {
        visualColorDefault = visualImages[0].color;

        if (commandType == CommandType.Start)
        {
            GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        }
        else
        {
            SetDefaultValue();
            RegisterInputListeners();
        }

        GameManager.Instance.OnGameStop += ResetSnippetToDefault;
        if (commandType != CommandType.Start)
            GameManager.Instance.OnGameRestart += ResetSnippetToDefault;
    }

    private void SetDefaultValue()
    {
        value = 1f;
        valueInput.SetTextWithoutNotify("1");
    }

    private void RegisterInputListeners()
    {
        valueInput.onValueChanged.AddListener(ValidateAndSetValue);
        valueInput.onSubmit.AddListener(ValidateAndSetValue);
        valueInput.onDeselect.AddListener(_ => ValidateAndSetValue(valueInput.text));
    }

    private void ValidateAndSetValue(string input)
    {
        if (!float.TryParse(input, out float parsedValue) || parsedValue <= 0f)
        {
            parsedValue = 1f;
            valueInput.SetTextWithoutNotify("1");
        }

        value = parsedValue;
        SnippetsManagerUI.Instance.UpdateSnippetUIsList();
    }

    private void GameManager_OnGameRestart(object sender, EventArgs e)
    {
        ChangeVisualColor(visualColorWhenRunning);
    }

    private void ResetSnippetToDefault(object sender, EventArgs e)
    {
        done = false;
        isRunning = false;
        ChangeVisualColor(visualColorDefault);
    }

    public void SetNextSnippetUI(SnippetUI snippetUI)
    {
        nextSnippetUI = snippetUI;
    }

    public SnippetUI GetNextSnippet() => nextSnippetUI;
    public CommandType GetCommandType() => commandType;
    public float GetValue() => value;
    public bool IsDone() => done;
    public bool IsRunning() => isRunning;

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
        foreach (Image img in visualImages)
        {
            img.color = newColor;

            Outline outline = img.GetComponent<Outline>();
            if (outline != null)
                outline.effectColor = newColor;
        }
    }

    private void OnDestroy()
    {
        if (commandType == CommandType.Start)
            GameManager.Instance.OnGameRestart -= GameManager_OnGameRestart;

        GameManager.Instance.OnGameStop -= ResetSnippetToDefault;

        if (commandType != CommandType.Start)
            GameManager.Instance.OnGameRestart -= ResetSnippetToDefault;
    }
}
