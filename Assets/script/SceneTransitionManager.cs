using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen; // ロード画面
    [SerializeField] private float delayBeforeActivation = 1f; // シーン遷移前の待機時間
    [SerializeField] private float fadeDuration = 0.3f; // フェードの時間

    private CanvasGroup canvasGroup; // フェード制御用のCanvasGroup

    private void Awake()
    {
        // loadingScreen に CanvasGroup を追加または取得
        if (loadingScreen != null)
        {
            canvasGroup = loadingScreen.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = loadingScreen.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f; // 初期状態は非表示
            loadingScreen.SetActive(false); // ロード画面を非アクティブ化
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        // ロード画面を表示しフェードイン
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            yield return StartCoroutine(FadeIn());
        }

        // 非同期でシーンをロード
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // 手動で制御

        // ロード進捗を監視
        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(delayBeforeActivation); // 待機時間を設定
                operation.allowSceneActivation = true; // シーンを有効化
            }
            yield return null; // 次のフレームまで待機
        }

        // フェードアウトしてロード画面を非表示
        if (loadingScreen != null)
        {
            yield return StartCoroutine(FadeOut());
            loadingScreen.SetActive(false);
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // 確実にフェードイン完了
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = fadeDuration;
        while (elapsedTime > 0f)
        {
            elapsedTime -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f; // 確実にフェードアウト完了
    }
}
