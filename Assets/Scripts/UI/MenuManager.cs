// MenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuAction { Start, Exit }

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Сцена игры")]
#if UNITY_EDITOR
    [Tooltip("Перетащи сюда сцену игры (SceneAsset). Имя подтянется автоматически.")]
    [SerializeField] private UnityEditor.SceneAsset gameSceneAsset;
#endif
    [SerializeField, HideInInspector] private string gameSceneName = "";

    [Header("Загрузка")]
    public bool useAsyncLoad = false;
    public bool allowSceneActivation = true;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // ВАЖНО: НЕ сохраняем между сценами
        // DontDestroyOnLoad(gameObject);  // удалено
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (gameSceneAsset != null)
        {
            var path = UnityEditor.AssetDatabase.GetAssetPath(gameSceneAsset);
            gameSceneName = System.IO.Path.GetFileNameWithoutExtension(path);

            bool inBuild = false;
            foreach (var s in UnityEditor.EditorBuildSettings.scenes)
            {
                if (!s.enabled) continue;
                var name = System.IO.Path.GetFileNameWithoutExtension(s.path);
                if (name == gameSceneName) { inBuild = true; break; }
            }
            if (!inBuild)
                Debug.LogWarning($"MenuManager: сцены \"{gameSceneName}\" нет в Build Settings (File > Build Settings).");
        }
    }
#endif

    public void Perform(MenuAction action)
    {
        switch (action)
        {
            case MenuAction.Start: StartGame(); break;
            case MenuAction.Exit: ExitGame(); break;
        }
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("MenuManager: не указана сцена игры (перетащи сцену или задавай имя).");
            return;
        }

        if (useAsyncLoad) StartCoroutine(LoadGameAsync());
        else SceneManager.LoadScene(gameSceneName);
    }

    System.Collections.IEnumerator LoadGameAsync()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("MenuManager: не указана сцена игры.");
            yield break;
        }

        var op = SceneManager.LoadSceneAsync(gameSceneName);
        op.allowSceneActivation = allowSceneActivation;
        while (!op.isDone) yield return null;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
