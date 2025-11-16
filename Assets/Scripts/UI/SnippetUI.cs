using UnityEngine;

public class SnippetUI :MonoBehaviour {

    [SerializeField] private CommandSnippet.CommandType commandType;
    [SerializeField] private TMPro.TMP_InputField valueInput;
    private SnippetUI nextSnippetUI;
    private float value;

    private void Start()
    {
        valueInput.onValueChanged.AddListener(newValue => {
            if (float.TryParse(newValue, out float parsedValue))
            {
                value = parsedValue;
            }
            else
            {
                value = 0f;
            }
            SnippetsManagerUI.Instance.UpdateSnippetUIsList();
        });
        if (commandType == CommandSnippet.CommandType.Jump)
        {
            valueInput.onSubmit.AddListener(newValue => {
                if (float.TryParse(newValue, out float parsedValue))
                {
                    value = parsedValue;
                }
                else
                {
                    value = 0f;
                }
                SnippetsManagerUI.Instance.UpdateSnippetUIsList();
            });
            valueInput.onDeselect.AddListener(newValue => {
                if (float.TryParse(newValue, out float parsedValue))
                {
                    value = parsedValue;
                }
                else
                {
                    value = 0f;
                }
                SnippetsManagerUI.Instance.UpdateSnippetUIsList();
            });
        }
    }

    public void SetNextSnippetUI(SnippetUI snippetUI)
    {
        nextSnippetUI = snippetUI;
    }
    public SnippetUI GetNextSnippet()
    {
        return nextSnippetUI;
    }
    public CommandSnippet.CommandType GetCommandType { get { return commandType; } }
    public float GetValue { get { return value; } }

}
