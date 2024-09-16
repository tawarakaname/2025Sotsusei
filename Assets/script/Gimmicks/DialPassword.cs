using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f;  // 移動に関するクールダウン時間
    private const string FireButton = "Fire2"; // 丸ボタンに対応するボタン名
    private const string HorizontalInput = "Horizontal Stick-L"; // 横方向の入力キー名

    [SerializeField] int[] correctNumbers;
    [SerializeField] DialPasswordButton[] dialpasswordButtons;

    private int currentPosition = 0;  // 現在選択されているスロットの位置
    private float nextMoveTime = 0f;  // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理

    private FlagManager flagManager;

    // 選択中のボタンのインデックスを保持
    private int lastSelectedPosition = -1;

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectDialButton(currentPosition); // 初期選択状態を設定
    }

    // パスワードのクリアをチェック
    public void CheckClear()
    {
        if (IsClear())
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.DialPasswordclear, true);
            Debug.Log("DialPasswordclearFlagON");
        }
    }

    private void Update()
    {
        // CameraZoomObjFlagがfalseなら早めに処理を終了
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj)) return;

        // 横方向の入力を取得し、移動処理
        HandleHorizontalInput();

        // 丸ボタンの入力処理
        HandleFireButtonInput();
    }

    // 横方向の入力処理
    private void HandleHorizontalInput()
    {
        float horizontalInput = Input.GetAxisRaw(HorizontalInput);

        // 左右移動処理、次の移動が可能かチェック
        if (Time.time >= nextMoveTime && horizontalInput != 0)
        {
            MoveSelection(horizontalInput);
            nextMoveTime = Time.time + MoveCooldown; // 次に移動できる時間を更新
        }
    }

    // 丸ボタンの入力処理
    private void HandleFireButtonInput()
    {
        if (Input.GetButtonDown(FireButton) && !isFireButtonPressed)
        {
            var currentButton = dialpasswordButtons[currentPosition]; // キャッシュしてアクセス回数を減らす
            // 選択中のボタンが背景パネルを表示している場合のみ処理を実行
            if (currentButton.BgPanel.activeSelf)
            {
                currentButton.OnClickThis();
                Debug.Log("DialPasswordclick");

                // クリア判定を呼び出す
                CheckClear();
            }
            isFireButtonPressed = true; // 丸ボタン押下フラグをtrueに設定
        }
        else if (!Input.GetButton(FireButton))
        {
            // 丸ボタンが離されたらフラグをリセット
            isFireButtonPressed = false;
        }
    }

    // 現在選択されているダイヤルボタンを更新
    private void SelectDialButton(int position)
    {
        if (lastSelectedPosition == position) return; // 既に選択されている場合は何もしない

        // 前の選択を解除
        if (lastSelectedPosition >= 0)
        {
            dialpasswordButtons[lastSelectedPosition].HideBGDialPanel();
        }

        // 新たに選択されたボタンの背景パネルを表示
        dialpasswordButtons[position].ShowBGPanel();
        lastSelectedPosition = position; // 新しい選択を記録
    }

    // 選択を左右に移動
    private void MoveSelection(float input)
    {
        if (input > 0)
        {
            ShiftSlotRight();
        }
        else
        {
            ShiftSlotLeft();
        }
    }

    // スロットを右にシフト
    private void ShiftSlotRight()
    {
        currentPosition = (currentPosition + 1) % dialpasswordButtons.Length;
        SelectDialButton(currentPosition);
    }

    // スロットを左にシフト
    private void ShiftSlotLeft()
    {
        currentPosition = (currentPosition - 1 + dialpasswordButtons.Length) % dialpasswordButtons.Length;
        SelectDialButton(currentPosition);
    }

    // クリア条件をチェック
    private bool IsClear()
    {
        // 配列アクセスをキャッシュして無駄なアクセスを防ぐ
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (dialpasswordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }
}
