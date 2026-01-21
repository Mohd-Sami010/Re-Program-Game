using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorUI :MonoBehaviour {

    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject mainMenuUi;
    [SerializeField] private LoadingUI loadingUi;

    [Header("Level Selection")]
    [SerializeField] private TextMeshProUGUI levelTitleText;
    [SerializeField] private TextMeshProUGUI levelInfoText;
    [SerializeField][TextArea(3, 6)] private string[] levelInfos;
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonTextMesh;
    [SerializeField] private int selectedLevelIndex;
    [SerializeField] private Button[] levelButtons;

    [Header("Navigation buttons")]
    [SerializeField] private GameObject[] levelButtonsSlides;
    [SerializeField] private Button nextSlideButton;
    [SerializeField] private Button previousSlideButton;
    [SerializeField] private TextMeshProUGUI slideIndicatorText;

    [Space]
    [Header("Outline Colors")]
    [SerializeField] private Color completedOutlineColor;
    [SerializeField] private Color lockedOutlineColor;
    [SerializeField] private Color continueOutlineColor;

    private int currentLevelsSlideIndex = 0;

    private Animator animator;

    private void Awake()
    {
        levelTitleText.text = "";
        levelInfoText.text = "";

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;

            levelButtons[index].onClick.AddListener(() => {
                SoundManager.Instance.PlayUISound1();
                SelectThisButton(index);
            });
            bool isLevelCompleted = PlayerPrefs.GetInt("Level_" + (index+1) + "_Completed", 0) == 1;
            if (isLevelCompleted)
            {
                levelButtons[i].GetComponent<Outline>().effectColor = completedOutlineColor;
            }
            else if (PlayerPrefs.GetInt("LevelToContinue", 1) == i+1)
            {
                levelButtons[i].GetComponent<Outline>().effectColor = continueOutlineColor;
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().fontSize +=10;
            }
            else
            {
                levelButtons[i].GetComponent<Outline>().effectColor = lockedOutlineColor;
            }
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = index +1 + "";

        }
        playButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound2();
            loadingUi.EnableLoadingUI();
            SceneManager.LoadSceneAsync(selectedLevelIndex + 1);
        });

        closeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound2();
            mainMenuUi.SetActive(true);
            StartCoroutine(CloseUI());
        });
        nextSlideButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            ChangeLevelSlide(1);
        });
        previousSlideButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            ChangeLevelSlide(-1);
        });
        ChangeLevelSlide(0);

        // Select the Continue level Button by Default
        int levelIndexToContinue = PlayerPrefs.GetInt("LevelToContinue", 1) -1;
        SelectThisButton(levelIndexToContinue);

        int slideIndex = levelIndexToContinue / 9;
        for (int i = 0; i < slideIndex; i++)
        {
            ChangeLevelSlide(1);
        }

        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    private void SelectThisButton(int i)
    {
        if (i >= levelButtons.Length) i = levelButtons.Length - 1;

        bool isLevelCompleted = PlayerPrefs.GetInt("Level_" + (i + 1) + "_Completed", 0) == 1;
        selectedLevelIndex = i;

        for (int j = 0; j < levelButtons.Length; j++)
        {
            if (j == i) levelButtons[j].GetComponent<CanvasGroup>().alpha = 0.9f;
            else levelButtons[j].GetComponent<CanvasGroup>().alpha = 1f;
        }
        levelTitleText.text = "Level " + (i + 1);
        levelInfoText.text = levelInfos[i];

        playButton.interactable = true;
        if (isLevelCompleted)
        {
            playButtonTextMesh.text = "Replay";
            levelInfoText.text += "\n\n<size=40><color=#E47474><b>Note</b>: When replaying a level, finish faster or use fewer code blocks than before to earn rewards.</color></size>";
        }
        else if (PlayerPrefs.GetInt("LevelToContinue", 1) == i + 1)
        {
            playButtonTextMesh.text = "Play";
        }
        else
        {
            playButtonTextMesh.text = "Locked";
            playButton.interactable = false;
        }
    }
    private void ChangeLevelSlide(int direction)
    {
        currentLevelsSlideIndex += direction;
        if (currentLevelsSlideIndex == levelButtonsSlides.Length - 1)
        {
            nextSlideButton.gameObject.SetActive(false);
        }
        else
        {
            nextSlideButton.gameObject.SetActive(true);
        }
        if (currentLevelsSlideIndex == 0)
        {
            previousSlideButton.gameObject.SetActive(false);
        }
        else 
        {
            previousSlideButton.gameObject.SetActive(true);
        }

        for (int i = 0; i < levelButtonsSlides.Length; i++)
        {
            if (i == currentLevelsSlideIndex) levelButtonsSlides[i].SetActive(true);
            else levelButtonsSlides[i].SetActive(false);
        }
        slideIndicatorText.text = $"{currentLevelsSlideIndex + 1} /{levelButtonsSlides.Length}";
    }
    private IEnumerator CloseUI()
    {
        animator.SetTrigger("Disable");
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        animator.SetTrigger("Enable");
    }

}
