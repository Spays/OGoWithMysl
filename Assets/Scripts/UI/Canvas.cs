// UIRootPersist.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class UIRootPersist : MonoBehaviour
{
    public bool rebindCameraOnSceneLoad = true; // если Canvas в Screen Space - Camera
    private static UIRootPersist _instance;

    void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;

        DontDestroyOnLoad(gameObject);                  // весь UI не удаляется при загрузке сцен
        if (rebindCameraOnSceneLoad)
            SceneManager.sceneLoaded += OnSceneLoaded;  // обновим ссылку на Main Camera после reload

        RebindCamerasNow();
    }

    void OnDestroy()
    {
        if (rebindCameraOnSceneLoad)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m) => RebindCamerasNow();

    void RebindCamerasNow()
    {
        var canvases = GetComponentsInChildren<Canvas>(true);
        foreach (var c in canvases)
        {
            if (c.renderMode == RenderMode.ScreenSpaceCamera)
                c.worldCamera = Camera.main; // новая Main Camera после перезагрузки
        }
    }
}
