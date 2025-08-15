using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("HUD (GameCanvas)")]
    [Tooltip("Укажи свой существующий HUD. Он не будет удаляться между сценами.")]
    public GameObject gameCanvas;
    public bool keepGameCanvasBetweenScenes = true; // оставить HUD жить между сценами
    private static GameObject s_GameCanvasRef;      // глобальная ссылка на живой HUD

    [Header("Пауза")]
    public GameObject pauseUIPrefab;
    public string pauseUIResourcePath = "UI/PauseCanvas";

    [Header("Геймовер")]
    public GameObject gameOverUIPrefab;
    public string gameOverUIResourcePath = "UI/GameOverCanvas";

    [Header("Победа")]
    public GameObject winUIPrefab;
    public string winUIResourcePath = "UI/WinCanvas";

    [Header("Навигация")]
    public string mainMenuSceneName = "MainMenu";
    public string gameSceneName = "";



    bool isPaused;

    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject winUI;

    // ---------- lifecycle ----------
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // сам менеджер общий

        Time.timeScale = 1f;

        AttachOrFindHUD();
        BindOverlaysInScene();
        Show(gameCanvas, true);
        HideAllOverlays();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this) { SceneManager.sceneLoaded -= OnSceneLoaded; }
        if (Time.timeScale != 1f) Time.timeScale = 1f;
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        Time.timeScale = 1f;
        isPaused = false;

        AttachOrFindHUD();     // HUD остаётся тем же объектом
        BindOverlaysInScene(); // подтянем сценовые оверлеи (если есть)

        Show(gameCanvas, true);
        HideAllOverlays();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    // ---------- Pause ----------
    public void TogglePause() { if (isPaused) Resume(); else Pause(); }

    public void Pause()
    {
        if (isPaused) return;
        EnsurePauseUILoaded();
        isPaused = true;
        Time.timeScale = 0f;
        Show(gameCanvas, false);
        Show(pauseUI, true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;
        Show(pauseUI, false);
        Show(gameCanvas, true);
    }

    void EnsurePauseUILoaded()
    {
        if (pauseUI) return;

        BindOverlaysInScene();
        if (pauseUI) { pauseUI.SetActive(false); return; }

        if (pauseUIPrefab) pauseUI = Instantiate(pauseUIPrefab, transform);
        else if (!string.IsNullOrEmpty(pauseUIResourcePath))
        {
            var prefab = Resources.Load<GameObject>(pauseUIResourcePath);
            if (!prefab) { Debug.LogError($"GameUIManager: нет Resources '{pauseUIResourcePath}'"); return; }
            pauseUI = Instantiate(prefab, transform);
        }
        if (pauseUI)
        {
            pauseUI.SetActive(false);
            var c = pauseUI.GetComponentInChildren<Canvas>(); if (c) c.sortingOrder = 9999;
        }
    }

    // ---------- States ----------
    public void GameOver()
    {
        EnsureGameOverUILoaded();
        Time.timeScale = 0f;
        Show(gameCanvas, false);
        Show(gameOverUI, true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Win()
    {
        EnsureWinUILoaded();
        Time.timeScale = 0f;
        Show(gameCanvas, false);
        Show(winUI, true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void EnsureGameOverUILoaded()
    {
        if (gameOverUI) return;

        BindOverlaysInScene();
        if (gameOverUI) { gameOverUI.SetActive(false); return; }

        if (gameOverUIPrefab) gameOverUI = Instantiate(gameOverUIPrefab, transform);
        else if (!string.IsNullOrEmpty(gameOverUIResourcePath))
        {
            var prefab = Resources.Load<GameObject>(gameOverUIResourcePath);
            if (!prefab) { Debug.LogError($"GameUIManager: нет Resources '{gameOverUIResourcePath}'"); return; }
            gameOverUI = Instantiate(prefab, transform);
        }
        if (gameOverUI)
        {
            gameOverUI.SetActive(false);
            var c = gameOverUI.GetComponentInChildren<Canvas>(); if (c) c.sortingOrder = 9999;
        }
    }

    void EnsureWinUILoaded()
    {
        if (winUI) return;

        BindOverlaysInScene();
        if (winUI) { winUI.SetActive(false); return; }

        if (winUIPrefab) winUI = Instantiate(winUIPrefab, transform);
        else if (!string.IsNullOrEmpty(winUIResourcePath))
        {
            var prefab = Resources.Load<GameObject>(winUIResourcePath);
            if (!prefab) { Debug.LogError($"GameUIManager: нет Resources '{winUIResourcePath}'"); return; }
            winUI = Instantiate(prefab, transform);
        }
        if (winUI)
        {
            winUI.SetActive(false);
            var c = winUI.GetComponentInChildren<Canvas>(); if (c) c.sortingOrder = 9999;
        }
    }

    // ---------- Buttons ----------
    public void RestartLevel()
    {
        // НЕ трогаем gameCanvas – он общий и хранит внешние привязки
        isPaused = false;
        HideAllOverlays();
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // HUD останется тем же объектом (DontDestroyOnLoad)
    }

    public void LoadMainMenu()
    {
        isPaused = false;
        HideAllOverlays();
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(mainMenuSceneName)) SceneManager.LoadScene(mainMenuSceneName);
        else Debug.LogError("GameUIManager: mainMenuSceneName не задан.");
    }

    public void NewGame()
    {
        isPaused = false;
        HideAllOverlays();
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(gameSceneName)) SceneManager.LoadScene(gameSceneName);
        else if (MenuManager.Instance != null) MenuManager.Instance.Perform(MenuAction.Start);
        else Debug.LogError("GameUIManager: gameSceneName не задан и нет MenuManager.");
    }

    public void QuitGame()
    {
        isPaused = false;
        HideAllOverlays();

        if (MenuManager.Instance != null) { MenuManager.Instance.Perform(MenuAction.Exit); return; }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ---------- Utils ----------
    void AttachOrFindHUD()
    {
        // если уже есть глобальная ссылка – используем её
        if (keepGameCanvasBetweenScenes && s_GameCanvasRef != null)
        {
            gameCanvas = s_GameCanvasRef;
        }

        // если в инспекторе не указали – ищем
        if (!gameCanvas)
        {
            // по тегу
            var tagged = GameObject.FindWithTag("GameCanvas");
            if (tagged) gameCanvas = tagged;
            // по имени среди активных
            if (!gameCanvas) gameCanvas = GameObject.Find("GameCanvas");
            // среди неактивных в корнях
            if (!gameCanvas)
            {
                var scene = SceneManager.GetActiveScene();
                foreach (var root in scene.GetRootGameObjects())
                {
                    var t = FindChildRecursive(root.transform, "GameCanvas");
                    if (t) { gameCanvas = t.gameObject; break; }
                }
            }
        }

        // закрепляем навсегда (и помечаем глобальную ссылку)
        if (keepGameCanvasBetweenScenes && gameCanvas != null)
        {
            if (s_GameCanvasRef == null)
            {
                s_GameCanvasRef = gameCanvas;
                DontDestroyOnLoad(gameCanvas);
            }
            else if (s_GameCanvasRef != gameCanvas)
            {
                // в сцене появился дубликат HUD – удалим сценовый
                if (gameCanvas.scene.IsValid()) Destroy(gameCanvas);
                gameCanvas = s_GameCanvasRef;
            }
        }
    }

    static Transform FindChildRecursive(Transform parent, string targetName)
    {
        if (parent.name == targetName) return parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            var hit = FindChildRecursive(parent.GetChild(i), targetName);
            if (hit) return hit;
        }
        return null;
    }

    GameObject FindOverlay(params string[] names)
    {
        // активные
        foreach (var n in names)
        {
            var go = GameObject.Find(n);
            if (go) return go;
        }
        // неактивные
        var scene = SceneManager.GetActiveScene();
        foreach (var root in scene.GetRootGameObjects())
        {
            foreach (var n in names)
            {
                var t = FindChildRecursive(root.transform, n);
                if (t) return t.gameObject;
            }
        }
        return null;
    }

    void BindOverlaysInScene()
    {
        if (!pauseUI) pauseUI = FindOverlay("Pausing Canvas", "PauseCanvas", "Pause");
        if (!gameOverUI) gameOverUI = FindOverlay("Losing Canvas", "GameOverCanvas", "GameOver");
        if (!winUI) winUI = FindOverlay("Winning Canvas", "WinCanvas", "Win");
    }

    void HideAllOverlays()
    {
        Show(pauseUI, false);
        Show(gameOverUI, false);
        Show(winUI, false);
    }

    static void Show(GameObject go, bool on)
    {
        if (go) go.SetActive(on);
    }
}
