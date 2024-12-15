using UnityEngine;
using UnityEngine.Playables;

public class Telop_C : MonoBehaviour
{

    [SerializeField] private MonoBehaviour playerScript;      // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private GameObject targetCamera;         // アニメーション後に無効化するカメラ
    [SerializeField] private GameObject telopCGameObject;     // Telop_Cが入ったeventsceneのgameobj
    [SerializeField] private GameObject telopC;     // telop_Cを格納

    private FlagManager flagManager;                          // フラグマネージャー
    public PlayableDirector director;

    void Start()
    {

        // フラグマネージャーのインスタンスを取得
        flagManager = FlagManager.Instance;

        // Bstartsceneok フラグが既に true ならスクリプトを無効化
        if (flagManager.GetFlag(FlagManager.FlagType.Cstartsceneok))
        {
            DisableScript();
            return;
        }

        // タイムラインの再生
        TelopC();

        // タイムラインの終了時にカメラを無効化
        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    private void TelopC()
    {
        // タイムラインの再生
        if (director != null)
        {
            director.Play();
        }

        // プレイヤー操作を無効化
        DisablePlayerControls();

        // Telop フラグを設定
        flagManager.SetFlag(FlagManager.FlagType.Telop, true);
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
        // カメラを非表示
        if (targetCamera != null)
        {
            targetCamera.SetActive(false);
        }

        // フラグを更新
        flagManager.SetFlag(FlagManager.FlagType.Telop, false);
        flagManager.SetFlag(FlagManager.FlagType.Cstartsceneok, true);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();

        // Telop_Cオブジェクトを非表示
        if (telopCGameObject != null)
        {
            telopCGameObject.SetActive(false);
            telopC.SetActive(false);
        }

        // スクリプトを無効化
        DisableScript();
    }

    private void DisableScript()
    {
        // イベントの登録解除
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }

        // スクリプトを無効化
        this.enabled = false;
    }
}
