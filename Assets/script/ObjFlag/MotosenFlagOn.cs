using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotosenFlagOn : MonoBehaviour
{
    [SerializeField] Collider triggerCollider; // 追加したコライダーのフィールド
    private bool playerInsideCollider = false;
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private FlagManager flagManager; // フラグマネージャーのインスタンスをキャッシュ

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得
    }

    void Update()
    {
        // CameraZoomObjFlagがfalseなら早めに処理を終了
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj)) return;

        if (playerInsideCollider)
        {
            HandleFireButtonInput();
        }
    }

    // 丸ボタンの入力処理
    private void HandleFireButtonInput()
    {
        // 丸ボタンが押された時の処理
        if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
        {
            OnClickObj();
            isFireButtonPressed = true; // ボタンが押されたらフラグをセット
        }
        else if (!Input.GetButton("Fire2"))
        {
            // ボタンが離されたらフラグをリセット
            isFireButtonPressed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }

    public void OnClickObj()
    {
        // フラグを設定する
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Motosen, true);
        Debug.Log("Motosen Flag On");
    }
}
