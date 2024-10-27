using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBannerAOpen : MonoBehaviour
{
    [SerializeField] private List<GameObject> gasBannerDoors; // アニメーションが付いている複数のdoorオブジェクト
    [SerializeField] private MonoBehaviour playerScript;      // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private List<Collider> gasCameraColliders; // 管理したい各カメラのコライダー

    private List<Animator> gasBannerDoorAnimators = new List<Animator>(); // 各doorオブジェクトのアニメーター
    private bool controlsDisabled = false;                    // プレイヤー操作が無効かどうか
    private FlagManager flagManager;                          // フラグマネージャー

    void Start()
    {
        foreach (var door in gasBannerDoors)
        {
            if (door != null)
            {
                Animator animator = door.GetComponent<Animator>();
                if (animator != null)
                {
                    gasBannerDoorAnimators.Add(animator); // 各doorのアニメーターをリストに追加
                }
            }
        }

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
    }

    void Update()
    {
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
    }

    private void OpenGasBanner()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Gasbaneropen))
        {
            foreach (var animator in gasBannerDoorAnimators)
            {
                animator.SetTrigger("GasbanerAopen"); // 各doorのアニメーションを再生
            }

            // フラグをセットして1度だけ開くようにする
            flagManager.SetFlag(FlagManager.FlagType.Gasbaneropen, true);

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
}
