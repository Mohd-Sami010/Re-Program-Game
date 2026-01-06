using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI :MonoBehaviour {
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider backgroundSlider;

    [Header("Buttons")]
    [SerializeField] private Button resetDataButton;
    [SerializeField] private Button closeButton;

    [Header("Other")]
    [SerializeField] private GameObject uiToEnableAfterClosing;

    [Header("Reset Confirmation")]
    [SerializeField] private GameObject confirmationUi;
    [SerializeField] private Button confirmYesButton;
    [SerializeField] private Button confirmNoButton;

    private const string MASTER_VOL = "MasterVolume";
    private const string BG_VOL = "BackgroundVolume";

    private const string MASTER_PREF = "MasterVolumePref";
    private const string BG_PREF = "BackgroundVolumePref";

    private const float DEFAULT_VOLUME = 0.75f;
    private const float MIN_DB = -80f;
    private const float ZERO_DB_SLIDER = 0.75f;
    private const float MAX_BOOST_DB = 6f;

    private void Awake()
    {
        // Load saved values or fallback to default (75%)
        float masterValue = PlayerPrefs.GetFloat(MASTER_PREF, DEFAULT_VOLUME);
        float bgValue = PlayerPrefs.GetFloat(BG_PREF, DEFAULT_VOLUME);

        masterSlider.SetValueWithoutNotify(masterValue);
        backgroundSlider.SetValueWithoutNotify(bgValue);

        ApplyMasterVolume(masterValue);
        ApplyBackgroundVolume(bgValue);

        // Listeners
        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        backgroundSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);

        if (resetDataButton != null)
        {
            resetDataButton.onClick.AddListener(() => {
                SoundManager.Instance.PlayUISound1();
                confirmationUi.SetActive(true);
            });
            confirmYesButton.onClick.AddListener(() => {
                SoundManager.Instance.PlayUISound1();
                ResetAllData();
            });
            confirmNoButton.onClick.AddListener(() => {
                SoundManager.Instance.PlayUISound1();
                confirmationUi.SetActive(false);
            });
        }
        closeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            if (confirmationUi != null) confirmationUi.SetActive(false);
            gameObject.SetActive(false);
            uiToEnableAfterClosing.SetActive(true);
        });
        gameObject.SetActive(false);
    }

    // ------------------ SLIDER HANDLERS ------------------

    private void OnMasterSliderChanged(float value)
    {
        ApplyMasterVolume(value);
        PlayerPrefs.SetFloat(MASTER_PREF, value);
    }

    private void OnBackgroundSliderChanged(float value)
    {
        ApplyBackgroundVolume(value);
        PlayerPrefs.SetFloat(BG_PREF, value);
    }

    private void ApplyMasterVolume(float value)
    {
        audioMixer.SetFloat(MASTER_VOL, SliderToDb(value));
    }

    private void ApplyBackgroundVolume(float value)
    {
        audioMixer.SetFloat(BG_VOL, SliderToDb(value));
    }

    // ------------------ UTILS ------------------

    private float SliderToDb(float value)
    {
        // Silence
        if (value <= 0.0001f)
            return MIN_DB;

        // Below 0 dB (0 -> 0.75)
        if (value <= ZERO_DB_SLIDER)
        {
            float normalized = value / ZERO_DB_SLIDER; // 0 -> 1
            return Mathf.Log10(normalized) * 20f;
        }

        // Above 0 dB (0.75 -> 1.0)
        float boostT = (value - ZERO_DB_SLIDER) / (1f - ZERO_DB_SLIDER);
        return Mathf.Lerp(0f, MAX_BOOST_DB, boostT);
    }


    // ------------------ BUTTON ACTIONS ------------------

    private void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Reset sliders visually + audio
        masterSlider.SetValueWithoutNotify(DEFAULT_VOLUME);
        backgroundSlider.SetValueWithoutNotify(DEFAULT_VOLUME);

        ApplyMasterVolume(DEFAULT_VOLUME);
        ApplyBackgroundVolume(DEFAULT_VOLUME);

        QuitGame();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
