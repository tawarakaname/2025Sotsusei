using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen; // ロード画面
    [SerializeField] float delayBeforeActivation = 2f; // シーン遷移前の待機時間
    private AsyncOperation async; // 非同期操作の参照

    private static SceneTransitionManager instance;

    private void Awake()
    {
        // シングルトンパターンを実装して、重複を防ぐ
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 永続化
        }
    }

    private void OnEnable()
    {
        // シーンロード完了時に呼び出されるイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // イベント登録を解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        // ロード画面を表示
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; // シーン切り替えを手動制御

        // 指定された遅延時間を待つ
        yield return new WaitForSeconds(delayBeforeActivation);

        // シーンロードの進捗を確認しながら待機
        while (async.progress < 0.9f)
        {
            yield return null; // 1フレーム待機
        }

        // シーン遷移を許可
        async.allowSceneActivation = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン遷移後にロード画面を非表示
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
}
