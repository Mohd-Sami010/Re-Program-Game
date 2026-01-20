using UnityEngine;

public class TutorialManager :MonoBehaviour {

    [SerializeField] private bool isTutorialLevel = false;

    [SerializeField][TextArea(1, 4)] private string[] tutorialTexts = {
        "Press <color=#FFC200><b>Code</b></color> button to open Code editor",
        "Drag and Drop <color=#FFC200><b>Move Block</b></color> under <color=#FFC200><b>Start Block</b></color>",
        "Change value inside <color=#FFC200><b>Move Block</b></color> to <color=#FFC200><b>10</b></color>",
        "Close <color=#FFC200><b>Code Editor</b></color> by pressing <color=#FF4141>X</color> and press <color=#0FC400><b>RUN</b></color>",
    };
    private int currentTutorialTextIndex = 0;

    [Header("Tutorial Level Settings")]
    [SerializeField] private bool textToOpenCodeEditor = true;
    [SerializeField] private bool textToPlaceBlock = true;
    [SerializeField] private bool textToChangeBlockValue = true;
    [SerializeField] private bool disableDeleteAreaObject = true;
    [SerializeField] private bool disableSnippetsSpawnerObject;
    [SerializeField] private bool disableSomeOfSnippetSpawnerButtonObject;
    [SerializeField] private int numOfSnippetSpawnerButtonsToDisable;
    [SerializeField] private bool disableTutorialOnPlay = true;

    [Header("Other Triggers")]
    [SerializeField] private bool buttonTrigger;

    private TutorialUI tutorialUI;

    private void Start()
    {
        if (isTutorialLevel)
        {
            tutorialUI = FindFirstObjectByType<TutorialUI>();
            tutorialUI.SetTutorialText(tutorialTexts[currentTutorialTextIndex]);
            currentTutorialTextIndex++;

            
        }
        else
        {
            Destroy(this);
        }
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        Destroy(tutorialUI.gameObject);
        Destroy(gameObject);
    }

    public bool IsTutorialLevel() { return isTutorialLevel; }
    public bool TextToOpenCodeEditor() { return textToOpenCodeEditor; }
    public bool TextToPlaceBlock() { return textToPlaceBlock; }
    public bool TextToChangeBlockValue() { return textToChangeBlockValue; }
    public bool DisableDeleteAreaObject() { return disableDeleteAreaObject; }
    public bool DisableSnippetsSpawnerObject() { return disableSnippetsSpawnerObject; }
    public bool DisableSomeSnippetSpawnerButtons() { return disableSomeOfSnippetSpawnerButtonObject; }
    public int GetNumOfSnippetSpawnerButtonsToDisable() { return  numOfSnippetSpawnerButtonsToDisable; }
    public bool CheckForGameToPlay() { return disableTutorialOnPlay; }
    public bool ButtonTrigger() { return buttonTrigger; }

    public void CodeEditorOpened()
    {
        tutorialUI.SetTutorialText(tutorialTexts[currentTutorialTextIndex]);
        currentTutorialTextIndex++;
        textToOpenCodeEditor = false;
    }
    public void BlockPlaced()
    {
        tutorialUI.SetTutorialText(tutorialTexts[currentTutorialTextIndex]);
        currentTutorialTextIndex++;
    }
    public void BlockValueChanged()
    {
        tutorialUI.SetTutorialText(tutorialTexts[currentTutorialTextIndex]);
        currentTutorialTextIndex++;
        textToChangeBlockValue = false;
    }
    public void GamePlayed()
    {
        if (currentTutorialTextIndex >= tutorialTexts.Length)
        {
            Destroy(tutorialUI.gameObject);
            Destroy(gameObject);
        }
    }
    public void TriggerButtonPressed()
    {
        tutorialUI.SetTutorialText(tutorialTexts[currentTutorialTextIndex]);
        currentTutorialTextIndex++;
        buttonTrigger = false;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }
}
