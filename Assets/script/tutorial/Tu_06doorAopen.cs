using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Tu_06doorAopen : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;      // プレイヤーのスクリプト（操作を無効化するため）
    //[SerializeField] private GameObject targetCamera;         // アニメーション後に無効化するカメラ
    [SerializeField] private GameObject in_AGameObject;     // Telop_Cが入ったeventsceneのgameobj

    private FlagManager flagManager;                          // フラグマネージャー
    public PlayableDirector director;

    private bool hasStarted = false; // 一度だけ実行するためのフラグ

    void Start()
    {
        flagManager = FlagManager.Instance;
    }

    void Update()
    {
        if (hasStarted || flagManager == null)
        {
            return;
        }

        // Tu_06clear フラグが true になったタイミングで実行
        if (flagManager.GetFlag(FlagManager.FlagType.Tu_06clear))
        {
            hasStarted = true; // 二度と実行しないようにフラグを立てる
            ExecuteEvent();
        }
    }

    private void ExecuteEvent()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.eventscenein_Aopen))
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
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
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
        // フラグを更新
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        flagManager.SetFlag(FlagManager.FlagType.eventscenein_Aopen, true);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();

        // Telop_Aオブジェクトを非表示
        if (in_AGameObject != null)
        {
            in_AGameObject.SetActive(false);
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
