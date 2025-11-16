using UnityEngine;

public class StartSnippet :MonoBehaviour {
    public static StartSnippet Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
}
