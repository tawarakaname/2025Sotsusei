using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GasBannerAOpen : MonoBehaviour
{
    [SerializeField] private List<GameObject> gasBannerDoors;      // アニメーションが付いている複数のdoorオブジェクト
    [SerializeField] private MonoBehaviour playerScript;           // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private List<Collider> gasCameraColliders;    // 管理したい各カメラのコライダー
    [SerializeField] private GameObject targetCamera;              // アニメーション後に無効化するカメラ

    private bool controlsDisabled = false;                         // プレイヤー操作が無効かどうか
    private FlagManager flagManager;                               // フラグマネージャー
    public PlayableDirector director;

    private bool animationCompleted = false;                       // アニメーションの完了フラグ

    void Start()
    {
        // フラグマネージャーのインスタンスを取得
        flagManager = FlagManager.Instance;

        // 初期状態でカメラのトリガーコライダーを無効化
        foreach (var collider in gasCameraColliders)
        {
            if (collider != null)
            {
                collider.enabled = false; // コライダーを無効化
            }
        }

        // タイムラインの終了時にカメラを無効化
        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    void Update()
    {
        // 条件が成立した場合にのみ再生処理を実行
        if (flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear) &&
            !flagManager.GetFlag(FlagManager.FlagType.Gasbaneropen))
        {
            DisablePlayerControls();  // プレイヤー操作を無効化
            OpenGasBanner();          // ガスバナーを開く
        }

        // プレイヤー操作が無効な場合はFire1, Fire2の入力を無視する
        if (controlsDisabled && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")))
        {
            return;  // 入力を無効化
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
        if (!flagManager.GetFlag(FlagManager.FlagType.Gasbaneropen) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))  // 既にNowanimがtrueなら設定しない
        {
            director.Play(); // Timelineの再生をここで実行

            // Nowanim フラグを true に設定
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);

            // カメラのコライダーを有効化
            foreach (var collider in gasCameraColliders)
            {
                if (collider != null)
                {
                    collider.enabled = true; // コライダーを有効化
                }
            }

            // プレイヤー操作を再び有効化
            playerScript.enabled = true;
        }
    }


    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;  // プレイヤーのスクリプトを無効化
        }
        controlsDisabled = true;  // Fire1, Fire2の入力を無効化
    }

    // タイムラインが停止したときに呼ばれる
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // アニメーション終了後にフラグを設定
        animationCompleted = true;
        flagManager.SetFlag(FlagManager.FlagType.Gasbaneropen, true); // 再生フラグを設定して再生は一度だけにする

        // Nowanim フラグを false に設定
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
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
