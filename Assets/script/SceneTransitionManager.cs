using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] float delayBeforeActivation = 2f;
    private static SceneTransitionManager instance;
    private Player player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.SetControlEnabled(false); // 操作無効化
            player.StopMovement(); // ← 追加: 直前の移動入力をリセット
        }

        if (loadingScreen != null) loadingScreen.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        yield return new WaitForSeconds(delayBeforeActivation);

        while (async.progress < 0.9f) yield return null;

        async.allowSceneActivation = true;
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindObjectOfType<Player>();
        if (player != null) player.SetControlEnabled(true); // 操作有効化

        if (loadingScreen != null) loadingScreen.SetActive(false);
    }
}
