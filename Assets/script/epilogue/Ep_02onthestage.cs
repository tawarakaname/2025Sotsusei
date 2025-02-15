using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement; // シーン管理に必要

public class Ep_02onthestage : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;      // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private GameObject Ep02_GameObject;     // Ep02_GameObjectが入ったeventsceneのgameobj
    [SerializeField] private Collider Ep_02stageCollider;   // 指定したステージコライダー
    [SerializeField] private GameObject targetUI;           // 新たに指定するターゲットUI

    private FlagManager flagManager;                          // フラグマネージャー
    public PlayableDirector director;

    private bool isPlayerInCollider; 
    private bool hasStarted = false; // 一度だけ実行するためのフラグ
    private bool isEp01Cleared = false; // Ep_01_clearがtrueになったかを確認

    void Start()
    {
        flagManager = FlagManager.Instance;
        isEp01Cleared = flagManager.GetFlag(FlagManager.FlagType.Ep01_clear);

        // 最初はステージコライダーとUIを無効化
        if (Ep_02stageCollider != null)
        {
            Ep_02stageCollider.enabled = false;
        }

        if (targetUI != null)
        {
            targetUI.SetActive(false);
        }
    }

    void Update()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Ep01_clear))
        {
            return;
        }

        // Ep_01_clear が true になったら、コライダーと UI を有効化
        if (!isEp01Cleared && flagManager.GetFlag(FlagManager.FlagType.Ep01_clear))
        {
            isEp01Cleared = true; // 変更を記録
            ActivateStageAndUI();
        }

        if (!hasStarted && isPlayerInCollider && flagManager.GetFlag(FlagManager.FlagType.Ep01_clear))
        {
            hasStarted = true;
            ExecuteEvent();
        }
    }

    private void ActivateStageAndUI()
    {
        if (Ep_02stageCollider != null)
        {
            Ep_02stageCollider.enabled = true; // ステージコライダーを有効化
        }

        if (targetUI != null)
        {
            targetUI.SetActive(true); // UI を有効化
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // "Player"タグを持つオブジェクトのみ処理する
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = true; // プレイヤーがコライダー内にいる状態
        }
    }

    private void ExecuteEvent()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.Ep02_clear))
        {
            DisableScript();
            return;
        }
        if (targetUI != null)
        {
            targetUI.SetActive(false); // UI を有効化
        }

        Ep02();

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    private void Ep02()
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
        flagManager.SetFlag(FlagManager.FlagType.Ep02_clear, true);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();

        // シーンを"OP"に切り替え
        SceneManager.LoadScene("ED");

        // スクリプトを無効化
        DisableScript();
    }

    private void DisableScript()
    {

        // イベントの登録解除
        if (director != null)
        {
            // タイムラインを強制停止
            director.Stop();
        }

        // Ep02_オブジェクトを非表示
        if (Ep02_GameObject != null)
        {
            Ep02_GameObject.SetActive(false);
        }

        // スクリプトを無効化
        this.enabled = false;
    }
}
