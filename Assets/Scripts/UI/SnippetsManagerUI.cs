using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnippetsManagerUI :MonoBehaviour {

    public static SnippetsManagerUI Instance { get; private set; }

    private List<SnippetUI> snippetUIs = new();

    [SerializeField] private Transform snippetsContainerTransform;
    [SerializeField] private GameObject inputBlockerObject1;
    [SerializeField] private GameObject inputBlockerObject2;
    [SerializeField] private Button addMoveSnippetButton;
    [SerializeField] private Button addJumpSnippetButton;
    [SerializeField] private Button addTurnSnippetButton;
    [SerializeField] private Button addInteractSnippetButton;
    [SerializeField] private Button closeUIButton;
    [Space]
    [SerializeField] private GameObject moveSnippetPrefab;
    [SerializeField] private GameObject jumpSnippetPrefab;
    [SerializeField] private GameObject turnSnippetPrefab;
    [SerializeField] private GameObject interactSnippetPrefab;


    public event System.Action OnEnableDropArea;
    public event System.Action OnDisableDropArea;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        addMoveSnippetButton.onClick.AddListener(() => {
            SpawnSnippet(moveSnippetPrefab, addMoveSnippetButton.GetComponent<RectTransform>());
            SoundManager.Instance.PlaySnippetSpawnSound();
        });
        addJumpSnippetButton.onClick.AddListener(() => {
            SpawnSnippet(jumpSnippetPrefab, addJumpSnippetButton.GetComponent<RectTransform>());
            SoundManager.Instance.PlaySnippetSpawnSound();
        });
        addTurnSnippetButton.onClick.AddListener(() => {
            SpawnSnippet(turnSnippetPrefab, addTurnSnippetButton.GetComponent<RectTransform>());
            SoundManager.Instance.PlaySnippetSpawnSound();
        });
        addInteractSnippetButton.onClick.AddListener(() => {
            SpawnSnippet(interactSnippetPrefab, addInteractSnippetButton.GetComponent<RectTransform>());
            SoundManager.Instance.PlaySnippetSpawnSound();
        });
        closeUIButton.onClick.AddListener(() => {
            HUD.Instance.ShowEditButton();
            SoundManager.Instance.PlayUISound1();
            gameObject.SetActive(false);
        });
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        GameManager.Instance.OnGamePause += GameManager_OnGameStop;
        //GameManager.Instance.OnContinueGame += GameManager_OnContinueGame;

        inputBlockerObject1.SetActive(false);
        inputBlockerObject2.SetActive(false);
    }

    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        inputBlockerObject1.SetActive(true);
        inputBlockerObject1.GetComponent<RectTransform>().SetAsLastSibling();
        inputBlockerObject2.SetActive(true);
    }
    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        inputBlockerObject1.SetActive(false);
        inputBlockerObject2.SetActive(false);
    }
    private void SpawnSnippet(GameObject prefab, RectTransform spawnButton)
    {
        GameObject snippetGO = Instantiate(prefab, snippetsContainerTransform);
        RectTransform snippetRT = snippetGO.GetComponent<RectTransform>();

        // Step 1: Get world position above the button
        Vector3 worldPos = spawnButton.transform.parent.position;
        float yOffset = spawnButton.rect.height * 2f;
        worldPos += Vector3.up * yOffset;

        // Step 2: Convert world -> local position of container
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            snippetsContainerTransform as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null, worldPos),
            null,
            out Vector2 localPos
        );

        snippetRT.anchoredPosition = localPos;
        snippetRT.localScale = Vector3.one;

        SoundManager.Instance.PlaySnippetSpawnSound();
    }

    public void UpdateSnippetUIsList()
    {
        snippetUIs.Clear();
        SnippetUI snippetUI = StartSnippet.Instance.GetComponent<SnippetUI>().GetNextSnippet();
        while (snippetUI != null)
        {
            snippetUIs.Add(snippetUI);
            snippetUI = snippetUI.GetNextSnippet();
        }
        // Send the list to the CommandSnippetsManager
        CommandSnippetsManager.Instance.SetCommandSnippets(snippetUIs);

    }
    public void EnableDropArea()
    {
        OnEnableDropArea?.Invoke();
    }
    public void DisableDropArea()
    {
        OnDisableDropArea?.Invoke();
    }
    public Transform GetParentContainer()
    {
        return snippetsContainerTransform;
    }
    public int GetNumberOfSnippetsUsed()
    {
        return snippetUIs.Count;
    }
}
