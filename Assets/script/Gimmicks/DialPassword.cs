using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f;  // 移動に関するクールダウン時間
    private const float RotationDuration = 0.5f; // 回転にかかる時間（秒）

    [SerializeField] private int[] correctNumbers; // 正しい数字の配列
    [SerializeField] private DialPasswordButton[] dialpasswordButtons; // ダイヤルボタンの配列
    [SerializeField] private GameObject neji01; // ダイヤルのオブジェクト
    [SerializeField] private GameObject neji02; // ダイヤルのオブジェクト

    private int currentPosition = 0; // 現在選択されているスロットの位置
    private float nextMoveTime = 0f; // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理

    private FlagManager flagManager; // フラグマネージャーのインスタンス

    private float rotationProgress = 0f; // 回転の進捗
    private Quaternion startRotation; // 開始時の回転
    private Quaternion endRotation; // 終了時の回転
    private GameObject rotatingObject = null; // 現在回転中のオブジェクト

    private int lastSelectedPosition = -1; // 最後に選択したボタンのインデックス
    private bool isCleared = false; // クリア状態を記録

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectDialButton(currentPosition); // 初期選択状態を設定
    }

    private void CheckClear()
    {
        if (IsClear())
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.DialPasswordclear, true);
            Debug.Log("DialPasswordclearFlagON");

            // クリア状態をtrueに設定
            isCleared = true;

            // DialPasswordButtonクラスを無効化する
            DisableDialPasswordButtons();
        }
    }

    // DialPasswordButtonクラスを無効化するメソッド
    private void DisableDialPasswordButtons()
    {
        foreach (var button in dialpasswordButtons)
        {
            button.enabled = false; // 各ボタンを無効にする
        }
    }
    private void Update()
    {
        // クリアされた場合、SetObjの入力を有効にする
        if (isCleared)
        {
            EnableSetObjInput(flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear)); // DialPasswordclearフラグに基づいて入力を有効に
            return;
        }
        else
        {
            HandleInput(); // 入力処理を共通メソッドに分離
            RotateNejiOverTime();
        }

        // CameraZoomObjとGasCamera0のフラグがfalseの時にSetObjの入力を有効に
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.GasCamera0))
        {
            EnableSetObjInput(true); // SetObjの入力を有効に
        }
        else
        {
            EnableSetObjInput(false); // SetObjの入力を無効に
        }
    }



    private void EnableSetObjInput(bool enable)
    {
        SetObj[] setObjs = FindObjectsOfType<SetObj>(); // シーン内のすべての SetObj を取得
        foreach (var setObj in setObjs)
        {
            setObj.enabled = enable; // SetObj の有効状態を設定
        }
    }


    private void HandleInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.GasCamera0))
        {
            HandleHorizontalInput();
            HandleFireButtonInput();
        }
    }


    private void HandleHorizontalInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) && flagManager.GetFlag(FlagManager.FlagType.GasCamera0))
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal Stick-L");

            if (Time.time >= nextMoveTime && horizontalInput != 0)
            {
                MoveSelection(horizontalInput); // スロットの移動を処理
                nextMoveTime = Time.time + MoveCooldown; // 次の移動時間を設定
            }
        }
    }

    private void HandleFireButtonInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) && flagManager.GetFlag(FlagManager.FlagType.GasCamera0))
        {
            if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
            {
                var currentButton = dialpasswordButtons[currentPosition];
                if (currentButton.BgPanel.activeSelf)
                {
                    currentButton.OnClickThis(); // 現在のボタンをクリック
                    CheckClear(); // クリア条件をチェック
                    HandleRotationAnimation(); // 回転アニメーションを処理
                }
                isFireButtonPressed = true; // ボタンが押されたことを記録
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                isFireButtonPressed = false; // ボタンが離されたことを記録
            }
        }
    }

    private void HandleRotationAnimation()
    {
        GameObject targetNeji = (currentPosition < 2) ? neji01 : neji02; // 選択位置に応じて対象を決定
        float angle = (currentPosition % 2 == 0) ? 90f : -90f; // 90度または-90度の回転
        StartRotation(targetNeji, angle); // 回転処理を開始
    }

    private void StartRotation(GameObject neji, float angle)
    {
        rotatingObject = neji; // 回転対象のオブジェクトを設定
        startRotation = neji.transform.rotation; // 現在の回転を記録
        endRotation = startRotation * Quaternion.Euler(0, angle, 0); // 終了時の回転を計算
        rotationProgress = 0f; // 回転進捗をリセット
    }

    private void RotateNejiOverTime()
    {
        if (rotatingObject != null)
        {
            rotationProgress += Time.deltaTime / RotationDuration; // 回転進捗を更新
            rotatingObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress); // 線形補間で回転を適用

            if (rotationProgress >= 1f) // 回転が完了した場合
            {
                rotatingObject.transform.rotation = endRotation; // 最終的な回転を設定
                rotatingObject = null; // 回転完了後、対象をリセット
                rotationProgress = 0f; // 進捗をリセット
            }
        }
    }

    private void SelectDialButton(int position)
    {
        if (lastSelectedPosition == position) return; // 既に選択されている場合は何もしない

        if (lastSelectedPosition >= 0)
        {
            dialpasswordButtons[lastSelectedPosition].HideBGDialPanel(); // 前の選択を解除
        }


        dialpasswordButtons[position].ShowBGPanel(); // 新たに選択されたボタンの背景パネルを表示
        lastSelectedPosition = position; // 新しい選択を記録
    }

    private void MoveSelection(float input)
    {
        if (input > 0)
        {
            ShiftSlotRight(); // 右にシフト
        }
        else
        {
            ShiftSlotLeft(); // 左にシフト
        }
    }

    private void ShiftSlotRight()
    {
        currentPosition = (currentPosition + 1) % dialpasswordButtons.Length; // 現在の位置を更新
        SelectDialButton(currentPosition); // 新たに選択を更新
    }

    private void ShiftSlotLeft()
    {
        currentPosition = (currentPosition - 1 + dialpasswordButtons.Length) % dialpasswordButtons.Length; // 現在の位置を更新
        SelectDialButton(currentPosition); // 新たに選択を更新
    }

    private bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (dialpasswordButtons[i].number != correctNumbers[i])
            {
                return false; // クリア条件に満たない場合
            }
        }
        return true; // すべての条件を満たした場合
    }

}