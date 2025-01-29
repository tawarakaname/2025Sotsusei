using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Telop_A : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;      // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private GameObject targetCamera;         // アニメーション後に無効化するカメラ
    [SerializeField] private GameObject telopAGameObject;     // Telop_Cが入ったeventsceneのgameobj
    [SerializeField] private GameObject telopA;     // telop_を格納

    private FlagManager flagManager;                          // フラグマネージャー
    public PlayableDirector director;

    void Start()
    {
        flagManager = FlagManager.Instance;

        if (flagManager == null)
        {
            return;
        }

        if (flagManager.GetFlag(FlagManager.FlagType.Astartsceneok))
        {
            DisableScript();
            return;
        }

        TelopA();

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }


    private void TelopA()
    {
        // タイムラインの再生
        if (director != null)
        {
            director.Play();
        }

        // プレイヤー操作を無効化
        DisablePlayerControls();

    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;  // プレイヤーのスクリプトを無効化
        }


        // Telop フラグを設定
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Telop, true);
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
        flagManager.SetFlag(FlagManager.FlagType.Astartsceneok, true);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();

        // Telop_Aオブジェクトを非表示
        if (telopAGameObject != null)
        {
            telopAGameObject.SetActive(false);
            telopA.SetActive(false);
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

            // タイムラインを強制停止
            director.Stop();
        }

        // スクリプトを無効化
        this.enabled = false;
    }
}
