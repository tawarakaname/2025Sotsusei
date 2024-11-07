using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f; // 移動に関するクールダウン時間
    private const float RotationDuration = 0.5f; // 回転にかかる時間（秒）

    [SerializeField] private int[] correctNumbers; // 正解の番号
    [SerializeField] private SmellPasswordButton[] SmellpasswordButtons; // 現在のパネルの数値
    [SerializeField] private GameObject[] rotatingObjects; // 各ボタンに連動する回転オブジェクトの配列

    [SerializeField] private GameObject Bluekey; // 最初に非表示のオブジェクト

    private int currentPosition = 0; // 現在選択されているスロットの位置
    private float nextMoveTime = 0f; // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private bool firstFireIgnored = false; // 最初のFire2入力を無視するためのフラグ
    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private int lastSelectedPosition = -1; // 選択中のボタンのインデックスを保持

    private Quaternion startRotation; // 開始時の回転
    private Quaternion endRotation; // 終了時の回転
    private GameObject rotatingObject = null; // 現在回転中のオブジェクト
    private float rotationProgress = 0f; // 回転の進捗

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectSmellButton(currentPosition); // 初期選択状態を設定
        firstFireIgnored = false; // 初回のFire2入力を無視するフラグを初期化
    }

    public void CheckClear()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.SmellPasswordclear))
            return;

        if (IsClear())
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.SmellPasswordclear, true);
            Debug.Log("SmellPPasswordclearFlagON");
            Bluekey.SetActive(true);
        }
    }

    private void Update()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.OdoguCamera))
            return;

        HandleHorizontalInput(); // 水平方向の入力を処理
        HandleFireButtonInput(); // 丸ボタンの入力を処理
        RotateObjectOverTime(); // 回転アニメーションの更新
    }

    private void HandleHorizontalInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.OdoguCamera))
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
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.OdoguCamera))
        {
            // 初回のFire2入力を無視する処理
            if (!firstFireIgnored)
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    firstFireIgnored = true; // 最初の入力を無視し、フラグを設定
                    return;
                }
            }
            else
            {
                // 2回目以降のFire2入力処理
                if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
                {
                    var currentButton = SmellpasswordButtons[currentPosition];
                    if (currentButton.IsButtonActive())
                    {
                        currentButton.OnClickThis(); // 現在のボタンをクリック
                        StartRotation(rotatingObjects[currentPosition], 90f); // 回転処理を開始
                        CheckClear(); // クリア条件をチェック
                    }
                    isFireButtonPressed = true; // ボタンが押されたことを記録
                }
                else if (Input.GetButtonUp("Fire2"))
                {
                    isFireButtonPressed = false; // ボタンが離されたことを記録
                }
            }
        }
    }

    private void StartRotation(GameObject obj, float angle)
    {
        rotatingObject = obj; // 回転対象のオブジェクトを設定
        startRotation = obj.transform.rotation; // 現在の回転を記録
        endRotation = startRotation * Quaternion.Euler(0, angle, 0); // 終了時の回転を計算
        rotationProgress = 0f; // 回転進捗をリセット
    }

    private void RotateObjectOverTime()
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

    private void SelectSmellButton(int position)
    {
        if (lastSelectedPosition == position) return; // 既に選択されている場合は何もしない

        if (lastSelectedPosition >= 0)
        {
            SmellpasswordButtons[lastSelectedPosition].HideBGSmellPanel(); // 前の選択を解除
        }

        SmellpasswordButtons[position].ShowBGPanel(); // 新たに選択されたボタンの背景パネルを表示
        lastSelectedPosition = position; // 新しい選択を記録
    }

    private void MoveSelection(float input)
    {
        currentPosition = (currentPosition + (input > 0 ? 1 : -1) + SmellpasswordButtons.Length) % SmellpasswordButtons.Length; // スロットの移動を処理
        SelectSmellButton(currentPosition); // 新たに選択を更新
    }

    private bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (SmellpasswordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }
}
