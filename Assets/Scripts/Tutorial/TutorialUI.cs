using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI :MonoBehaviour {

    [SerializeField] private TextMeshProUGUI tutorialText;

    [SerializeField] private SnippetUI snippetUI;
    [SerializeField] private TMP_InputField snippetUiInputField;

    [Header("Delete Area")]
    [SerializeField] private GameObject deleteAreaObject;

    [Header("Snippets Spawner")]
    [SerializeField] private GameObject snippetsSpawnerObject;
    [SerializeField] private GameObject[] snippetSpawnerButtonObjects;

    [Space]
    [Header("Open Code Editor Button")]
    [SerializeField] private Button openCodeEditorButton;
    [SerializeField] private Button runButton;

    [SerializeField] private Button otherTriggerButton;
    private TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindFirstObjectByType<TutorialManager>();
        if (tutorialManager != null && tutorialManager.IsTutorialLevel())
        {
            // Disable Delete Area Object
            if (tutorialManager.DisableDeleteAreaObject() && deleteAreaObject != null)
            {
                Destroy(deleteAreaObject);
            }
            // Disable Snippets Spawner Object
            if (tutorialManager.DisableSnippetsSpawnerObject() && snippetsSpawnerObject != null)
            {
                Destroy(snippetsSpawnerObject);
            }

            // Open Code Editor Button
            if (openCodeEditorButton != null && tutorialManager.TextToOpenCodeEditor())
            {
                openCodeEditorButton.onClick.AddListener(() =>
                {
                    if (tutorialManager.TextToOpenCodeEditor()) tutorialManager.CodeEditorOpened();
                });
            }

            // Block Placed under StartSnippet
            if (snippetUI != null && tutorialManager.TextToPlaceBlock())
            {
                StartSnippet.Instance.GetComponent<SnippetUI>().OnNextSnippetChanged += SnippetUI_OnNextSnippetChanged;
                
            }

            // Block Value Changed to 10
            if (snippetUI != null && tutorialManager.TextToChangeBlockValue())
            {
                snippetUiInputField.onValueChanged.AddListener(_ => {
                    if (snippetUiInputField.text.Trim() == "10")
                    {
                        if (tutorialManager.TextToChangeBlockValue()) tutorialManager.BlockValueChanged();
                    }
                });
            }

            if (tutorialManager.CheckForGameToPlay())
            {
                runButton.onClick.AddListener(() =>
                {
                    tutorialManager.GamePlayed();
                });
            }

            if (tutorialManager.DisableSomeSnippetSpawnerButtons())
            {
                for (int i = tutorialManager.GetNumOfSnippetSpawnerButtonsToDisable(); i > 0; i--)
                {
                    Destroy(snippetSpawnerButtonObjects[i]);
                }
            }

            if (tutorialManager.ButtonTrigger())
            {
                otherTriggerButton.onClick.AddListener(() => {
                    if (tutorialManager.ButtonTrigger()) tutorialManager.TriggerButtonPressed();
                });
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SnippetUI_OnNextSnippetChanged()
    {
        SnippetUI nextSnippet = StartSnippet.Instance.GetComponent<SnippetUI>().GetNextSnippet();
        if (nextSnippet != null)
        {
            tutorialManager.BlockPlaced();
            StartSnippet.Instance.GetComponent<SnippetUI>().OnNextSnippetChanged -= SnippetUI_OnNextSnippetChanged;
        }
    }

    public void SetTutorialText(string text)
    {
        SoundManager.Instance.PlayTutorialTextPopSound();
        tutorialText.transform.parent.gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Pop");
        if (tutorialText != null)
        {
            tutorialText.text = text;
        }
    }
    public void HideTutorialText()
    {
        tutorialText.transform.parent.gameObject.SetActive(false);
    }
}
