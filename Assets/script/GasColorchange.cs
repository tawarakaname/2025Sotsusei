using UnityEngine;
using UnityEngine.Playables;

public class GasColorchange : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;           // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private GameObject targetCamera;              // アニメーション後に無効化するカメラ
    [SerializeField] private GameObject[] cupsules; 

    private FlagManager flagManager;                               // フラグマネージャー
    public PlayableDirector director;

    private bool animationCompleted = false;                       // アニメーションの完了フラグ

    void Start()
    {
        // フラグマネージャーのインスタンスを取得
        flagManager = FlagManager.Instance;

        // タイムラインの終了時にカメラを無効化
        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    void Update()
    {
        // 条件が成立した場合にのみ再生処理を実行
        if (flagManager.GetFlag(FlagManager.FlagType.capsuleclear) &&
            !flagManager.GetFlag(FlagManager.FlagType.FireColorchange))
        {
            OpenGasBanner();        
        }

        // アニメーション完了後にフラグを確認してカメラを無効化
        if (animationCompleted)
        {
            if (targetCamera != null)
            {
                targetCamera.SetActive(false);  // カメラを無効化
            }
            animationCompleted = false;  // 一度だけ無効化するようにフラグをリセット
        }
    }

    private void OpenGasBanner()
    {
        // フラグチェックを再度行い、再生が1回のみ行われるようにする
        if (!flagManager.GetFlag(FlagManager.FlagType.FireColorchange) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))  // 既にNowanimがtrueなら設定しない
        {
            // cupsules 配列のすべてのオブジェクトを非アクティブ化
            foreach (GameObject cupsule in cupsules)
            {
                if (cupsule != null)
                {
                    cupsule.SetActive(false);
                }
            }

            director.Play(); // Timelineの再生をここで実行

            // プレイヤー操作を無効化
            DisablePlayerControls();

            // Nowanim フラグを true に設定
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);

        }
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;  // プレイヤーのスクリプトを無効化
        }
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;  // プレイヤーのスクリプトを有効化
        }
    }

    // タイムラインが停止したときに呼ばれる
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // アニメーション終了後にフラグを設定
        animationCompleted = true;
        flagManager.SetFlag(FlagManager.FlagType.FireColorchange, true); // 再生フラグを設定して再生は一度だけにする

        // Nowanim フラグを false に設定
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();
    }

    private void OnDestroy()
    {
        // イベントの登録解除
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }
}
