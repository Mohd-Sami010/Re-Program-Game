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

    [Space]
    [Header("Open Code Editor Button")]
    [SerializeField] private Button openCodeEditorButton;
    [SerializeField] private Button runButton;
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
                    tutorialManager.CodeEditorOpened();
                });
            }

            // Block Placed under StartSnippet
            if (snippetUI != null && tutorialManager.TextToPlaceBlock())
            {
                StartSnippet.Instance.GetComponent<SnippetUI>().OnNextSnippetChanged += SnippetUI_OnNextSnippetChanged;
                Debug.Log("TutorialUI: Subscribed to SnippetUI OnNextSnippetChanged event.");
            }

            // Block Value Changed to 10
            if (snippetUI != null && tutorialManager.TextToChangeBlockValue())
            {
                snippetUiInputField.onValueChanged.AddListener(_ => {
                    if (snippetUiInputField.text.Trim() == "10")
                    {
                        tutorialManager.BlockValueChanged();
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
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void SnippetUI_OnNextSnippetChanged()
    {
        SnippetUI nextSnippet = StartSnippet.Instance.GetComponent<SnippetUI>().GetNextSnippet();
        if (nextSnippet != null)
        {
            tutorialManager.BlockPlaced();
            Debug.Log("TutorialUI: BlockPlaced event triggered.");
        }
    }

    public void SetTutorialText(string text)
    {
        SoundManager.Instance.PlayTutorialTextPopSound();
        tutorialText.transform.parent.gameObject.SetActive(true);
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
