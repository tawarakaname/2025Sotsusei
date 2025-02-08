using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen; // ロード画面
    [SerializeField] float delayBeforeActivation = 2f; // シーン遷移前の待機時間
    private AsyncOperation async; // 非同期操作の参照
    private static SceneTransitionManager instance;

    private Player player; // プレイヤーの操作スクリプト（適宜変更）

    private void Awake()
    {
        // シングルトンパターン
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        // プレイヤーの操作を無効化
        if (player == null)
            player = FindObjectOfType<Player>(); // シーン内のプレイヤーを探す
        if (player != null)
            player.SetControlEnabled(false); // 操作無効化（PlayerController にメソッドを作成）

        // ロード画面を表示
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // シーンを非同期でロード
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

    private IEnumerator EnablePlayerControl()
    {
        float timeout = 3f; // 最大3秒待機（必要に応じて調整）
        float elapsedTime = 0f;

        while (elapsedTime < timeout)
        {
            player = FindObjectOfType<Player>();

            if (player != null && player.gameObject.activeInHierarchy) // プレイヤーが見つかり、表示されているか？
            {
                player.SetControlEnabled(true); // 操作再開
                yield break; // ループを終了
            }

            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f); // 0.1秒ごとにチェック
        }

        Debug.LogWarning("プレイヤーが見つからない、または非表示のままです。");
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(EnablePlayerControl());

        // 新しいシーンのプレイヤーを取得して操作を有効化
        player = FindObjectOfType<Player>();
        if (player != null)
            player.SetControlEnabled(true); // 操作再開

        // ロード画面を非表示
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
}
