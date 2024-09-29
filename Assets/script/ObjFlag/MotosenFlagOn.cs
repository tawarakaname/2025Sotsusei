using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotosenFlagOn : MonoBehaviour
{
    [SerializeField] Collider triggerCollider; // 追加したコライダーのフィールド
    private bool playerInsideCollider = false;
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private FlagManager flagManager; // フラグマネージャーのインスタンスをキャッシュ

    public GameObject huta; // motosenhuta を指定するためのフィールド
    private bool isRotating = false; // 一度だけ回転させるためのフラグ

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
        if (Input.GetButtonDown("Fire2") && !isFireButtonPressed && !isRotating)
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

        // motosenhutaを滑らかにY方向に90度回転させる
        if (huta != null && !isRotating)
        {
            StartCoroutine(RotateHutaSmoothly());
        }
    }

    // コルーチンで滑らかに回転させる
    private IEnumerator RotateHutaSmoothly()
    {
        isRotating = true;
        float currentRotation = 0f; // 現在の回転角度
        float targetRotation = 90f; // 目標の回転角度
        float rotationSpeed = 90f; // 1秒で90度回転するスピード

        while (currentRotation < targetRotation)
        {
            float step = rotationSpeed * Time.deltaTime; // フレームごとの回転量
            huta.transform.Rotate(0, step, 0); // Y軸に回転
            currentRotation += step;
            yield return null; // 次のフレームまで待つ
        }

        // 最後に90度に正確にセットする
        huta.transform.rotation = Quaternion.Euler(0, 90, 0);
        Debug.Log("motosenhutaが90度に回転しました");
        isRotating = false;
    }
}
