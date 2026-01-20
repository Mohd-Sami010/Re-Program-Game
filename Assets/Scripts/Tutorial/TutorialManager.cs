using UnityEngine;

public class TutorialManager :MonoBehaviour {

    [SerializeField] private bool isTutorialLevel = false;

    [SerializeField][TextArea(1, 2)] private string[] tutorialTexts = {
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
    [SerializeField] private bool disableTutorialOnPlay = true;

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
    }
    public bool IsTutorialLevel() { return isTutorialLevel; }
    public bool TextToOpenCodeEditor() { return textToOpenCodeEditor; }
    public bool TextToPlaceBlock() { return textToPlaceBlock; }
    public bool TextToChangeBlockValue() { return textToChangeBlockValue; }
    public bool DisableDeleteAreaObject() { return disableDeleteAreaObject; }
    public bool DisableSnippetsSpawnerObject() { return disableSnippetsSpawnerObject; }
    public bool CheckForGameToPlay() { return disableTutorialOnPlay; }

    public void CodeEditorOpened()
    {
        tutorialUI.SetTutorialText(tutorialTexts[currentTutorialTextIndex]);
        currentTutorialTextIndex++;
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
    }
    public void GamePlayed()
    {
        if (currentTutorialTextIndex >= tutorialTexts.Length)
        {
            Destroy(tutorialUI.gameObject);
            Destroy(this.gameObject);
        }
    }
}
