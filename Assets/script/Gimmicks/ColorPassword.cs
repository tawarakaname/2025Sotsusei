using UnityEngine;

public class ColorPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f; // 移動に関するクールダウン時間

    [SerializeField] private int[] correctNumbers; // 正解の番号
    [SerializeField] private ColorPasswordButton[] ColorpasswordButtons; // 現在のパネルの数値

    private int currentPosition = 0; // 現在選択されているスロットの位置
    private float nextMoveTime = 0f; // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private int lastSelectedPosition = -1; // 選択中のボタンのインデックスを保持

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectColorButton(currentPosition); // 初期選択状態を設定
    }

    public void CheckClear()
    {
        if (IsClear())
        {
            // フラグを設定する
            FlagManager.Instance.SetFlag(FlagManager.FlagType.ColorPasswordclear, true);
            Debug.Log("ColorPasswordclearFlagON");
        }
    }
    private void Update()
    {
        // 必要なフラグが無効な場合は処理を終了
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.BoxBCamera))
            return;

        HandleHorizontalInput(); // 水平方向の入力を処理
        HandleFireButtonInput(); // 丸ボタンの入力を処理
    }

    private void HandleHorizontalInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BoxBCamera))
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
            flagManager.GetFlag(FlagManager.FlagType.BoxBCamera))
        {
            if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
            {
                var currentButton = ColorpasswordButtons[currentPosition];
                if (currentButton.BgPanel.activeSelf)
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

    private void SelectColorButton(int position)
    {
        if (lastSelectedPosition == position) return; // 既に選択されている場合は何もしない

        // 前の選択を解除
        if (lastSelectedPosition >= 0)
        {
           ColorpasswordButtons[lastSelectedPosition].HideBGColorPanel();
        }

        // 新たに選択されたボタンの背景パネルを表示
        ColorpasswordButtons[position].ShowBGPanel();
        lastSelectedPosition = position; // 新しい選択を記録
    }

    private void MoveSelection(float input)
    {
        currentPosition = (currentPosition + (input > 0 ? 1 : -1) + ColorpasswordButtons.Length) % ColorpasswordButtons.Length; // スロットの移動を処理
        SelectColorButton(currentPosition); // 新たに選択を更新
    }

    bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            Debug.Log($"Button {i} number: {ColorpasswordButtons[i].number}, correct: {correctNumbers[i]}"); // デバッグログ
            if (ColorpasswordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }


}
