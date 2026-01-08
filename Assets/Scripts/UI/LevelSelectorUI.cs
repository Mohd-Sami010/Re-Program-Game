using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorUI :MonoBehaviour {

    [SerializeField] private int levelIndex;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject[] levelButtonsSlides;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject mainMenuUi;
    [SerializeField] private GameObject loadingUi;

    [Header("Navigation buttons")]
    [SerializeField] private Button nextSlideButton;
    [SerializeField] private Button previousSlideButton;
    [SerializeField] private TextMeshProUGUI slideIndicatorText;

    [Space]
    [Header("Outline Colors")]
    [SerializeField] private Color completedOutlineColor;
    [SerializeField] private Color lockedOutlineColor;
    [SerializeField] private Color continueOutlineColor;

    private int currentLevelsSlideIndex = 0;
    private void Awake()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i; // Capture the current value of i
            levelButtons[i].onClick.AddListener(() => {
                SoundManager.Instance.PlayUISound1();
                loadingUi.SetActive(true);
                SceneManager.LoadScene(index + 1);
            });
            bool isLevelCompleted = PlayerPrefs.GetInt("Level_" + (i+1) + "_Completed", 0) == 1;
            if (isLevelCompleted)
            {
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponent<Outline>().effectColor = completedOutlineColor;
            }
            else if (PlayerPrefs.GetInt("LevelToContinue", 1) == i+1)
            {
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponent<Outline>().effectColor = continueOutlineColor;
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().fontSize +=10;
            }
            else
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Outline>().effectColor = lockedOutlineColor;
            }
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = i+1 + "";
        }
        closeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound2();
            mainMenuUi.SetActive(true);
            gameObject.SetActive(false);
        });
        nextSlideButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            if (currentLevelsSlideIndex < levelButtonsSlides.Length - 1)
            {
                levelButtonsSlides[currentLevelsSlideIndex].SetActive(false);
                currentLevelsSlideIndex++;
                levelButtonsSlides[currentLevelsSlideIndex].SetActive(true);
                UpdateSlideIndicator();
            }
        });
        previousSlideButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            if (currentLevelsSlideIndex > 0)
            {
                levelButtonsSlides[currentLevelsSlideIndex].SetActive(false);
                currentLevelsSlideIndex--;
                levelButtonsSlides[currentLevelsSlideIndex].SetActive(true);
                UpdateSlideIndicator();
            }
        });
        UpdateSlideIndicator();

        gameObject.SetActive(false);
    }
    private void UpdateSlideIndicator()
    {
        slideIndicatorText.text = $"{currentLevelsSlideIndex + 1} /{levelButtonsSlides.Length}";
    }

}
