using UnityEngine;
using UnityEngine.Playables;


public class AquariumTurnOn : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;           // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private GameObject targetCamera;         // アニメーション後に無効化するカメラ
    [SerializeField] private GameObject ButtonUIImage; // 表示・非表示を制御するImage
    private FlagManager flagManager;                               // フラグマネージャー
    private bool isleverdown = false; // トリガーが既に実行されたかどうかを管理
    public PlayableDirector director;


    void Start()
    {
        // フラグマネージャーのインスタンスを取得
        flagManager = FlagManager.Instance;
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(false); // 初期状態は非表示
        }

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    void Update()
    {
        // 共通フラグ取得
        bool LeverCamera = flagManager.GetFlag(FlagManager.FlagType.LeverCamera);

        // LeverCamera フラグが false なら処理をスキップ
        if (!LeverCamera) return;

        // その他のフラグ取得
        bool ToyPasswordclear = flagManager.GetFlag(FlagManager.FlagType.ToyPasswordclear);
        bool leverdown = flagManager.GetFlag(FlagManager.FlagType.Leverdown);

        // `Fire2` 押下時の処理
        if (ToyPasswordclear && !leverdown)
        {
            if (ButtonUIImage.activeSelf  && Input.GetButtonDown("Fire2"))
            {
                AquariumTurnon();
            }
            else if (!isleverdown)
            {
                pushleverdown();
            }
        }
    }

    private void pushleverdown()
    {
        // 既にトリガーが実行されている場合は処理を終了
        if (isleverdown || flagManager.GetFlag(FlagManager.FlagType.Leverdown))
            return;

        // ButtonUIImageを表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(true);
        }

        // トリガー済みフラグを設定
        isleverdown = true;

    }

    private void AquariumTurnon()
    {
        // フラグチェックを再度行い、再生が1回のみ行われるようにする
        if (!flagManager.GetFlag(FlagManager.FlagType.Leverdown) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))  // 既にNowanimがtrueなら設定しない
        {
            director.Play(); // Timelineの再生をここで実行

            // プレイヤー操作を無効化
            DisablePlayerControls();

            // Nowanim フラグを true に設定
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);

            // ButtonUIImageを非表示
            if (ButtonUIImage != null)
            {
                ButtonUIImage.SetActive(false);
            }

            // フラグを解除
            isleverdown = false;
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
        flagManager.SetFlag(FlagManager.FlagType.Leverdown, true); // 再生フラグを設定して再生は一度だけにする

        // Nowanim フラグを false に設定
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();
        // カメラを非表示

        if (targetCamera != null)
        {
            targetCamera.SetActive(false);
        }
    }

}
