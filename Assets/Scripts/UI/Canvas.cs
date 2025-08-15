// UIRootPersist.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class UIRootPersist : MonoBehaviour
{
    public bool rebindCameraOnSceneLoad = true; // ���� Canvas � Screen Space - Camera
    private static UIRootPersist _instance;

    void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;

        DontDestroyOnLoad(gameObject);                  // ���� UI �� ��������� ��� �������� ����
        if (rebindCameraOnSceneLoad)
            SceneManager.sceneLoaded += OnSceneLoaded;  // ������� ������ �� Main Camera ����� reload

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
                c.worldCamera = Camera.main; // ����� Main Camera ����� ������������
        }
    }
}
