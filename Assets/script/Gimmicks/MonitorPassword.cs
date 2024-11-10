using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f; // 移動に関するクールダウン時間

    [SerializeField] private int[] correctNumbers; // 正解の番号
    [SerializeField] private MonitorPasswordButton[] monitorpasswordButtons; // 現在のパネルの数値


    private int currentPosition = 0; // 現在選択されているスロットの位置
    private float nextMoveTime = 0f; // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private bool firstFireIgnored = false; // 最初のFire2入力を無視するためのフラグ
    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private int lastSelectedPosition = -1; // 選択中のボタンのインデックスを保持
    private bool isUpScaleCam;

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectmonitorButton(currentPosition); // 初期選択状態を設定
        firstFireIgnored = false; // 初回のFire2入力を無視するフラグを初期化
    }

    public void CheckClear()
    {
        //ここ後から水槽の色が変わった時のフラグにかえる↓
        if (flagManager.GetFlag(FlagManager.FlagType.MonitorPasswordclear))
            return;

        if (IsClear())
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.MonitorPasswordclear, true);
            Debug.Log("MonitorPasswordclearFlagON");
        }
    }

    private void Update()
    {
        if (isUpScaleCam)
        {
            HandleHorizontalInput(); // 水平方向の入力を処理
            HandleFireButtonInput(); // 丸ボタンの入力を処理
        }

        isUpScaleCam = flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                       flagManager.GetFlag(FlagManager.FlagType.JugCamera);
    }

    private void HandleHorizontalInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.JugCamera))
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
            flagManager.GetFlag(FlagManager.FlagType.JugCamera))
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
                    var currentButton = monitorpasswordButtons[currentPosition];
                    if (currentButton.IsButtonActive())
                    {
                        currentButton.OnClickThis(); // 現在のボタンをクリック
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
    private void SelectmonitorButton(int position)
    {
        if (lastSelectedPosition == position) return; // 既に選択されている場合は何もしない

        if (lastSelectedPosition >= 0)
        {
           monitorpasswordButtons[lastSelectedPosition].HideBGSmellPanel(); // 前の選択を解除
        }

        monitorpasswordButtons[position].ShowBGPanel(); // 新たに選択されたボタンの背景パネルを表示
        lastSelectedPosition = position; // 新しい選択を記録
    }

    private void MoveSelection(float input)
    {
        currentPosition = (currentPosition + (input > 0 ? 1 : -1) + monitorpasswordButtons.Length) % monitorpasswordButtons.Length; // スロットの移動を処理
        SelectmonitorButton(currentPosition); // 新たに選択を更新
    }

    private bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (monitorpasswordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }
}
