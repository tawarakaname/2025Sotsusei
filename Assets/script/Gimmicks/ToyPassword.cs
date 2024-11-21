using UnityEngine;

public class ToyPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f; // 移動に関するクールダウン時間

    [SerializeField] private int[] correctNumbers; // 正解の番号
    [SerializeField] private ToyPasswordButton[] ToypasswordButtons; // 現在のパネルの数値
    private bool firstFireIgnored = false; // 最初のFire2入力を無視するためのフラグ

    private int currentPosition = 0; // 現在選択されているスロットの位置
    private float nextMoveTime = 0f; // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private int lastSelectedPosition = -1; // 選択中のボタンのインデックスを保持

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectToyButton(currentPosition); // 初期選択状態を設定
        firstFireIgnored = false; // 初回のFire2入力を無視するフラグを初期化
    }

    public void CheckClear()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.ToyPasswordclear))
            return;

        if (IsClear())
        {
            flagManager.SetFlag(FlagManager.FlagType.ToyPasswordclear, true);
        }
    }

    private void Update()
    {
        // ThreePasswordclearがtrueの場合、このスクリプトを無効化
        if (flagManager.GetFlag(FlagManager.FlagType.ToyPasswordclear))
        {
            this.enabled = false; // スクリプトを無効化
            return;
        }

        // CameraZoomObjがfalseの場合、初期化
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj))
        {
            firstFireIgnored = false;
        }

        // 必要なフラグが揃っていない場合は早期リターン
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) ||
            !flagManager.GetFlag(FlagManager.FlagType.ToyboxCamera))
        {
            return;
        }

        HandleHorizontalInput(); // 水平方向の入力を処理
        HandleFireButtonInput(); // 丸ボタンの入力を処理
    }

    private void HandleHorizontalInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.ToyboxCamera))
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
            flagManager.GetFlag(FlagManager.FlagType.ToyboxCamera))
        {
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
                if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
                {
                    var currentButton = ToypasswordButtons[currentPosition];
                    if (currentButton.IsButtonActive())
                    {
                        currentButton.OnClickToy(); // 現在のボタンをクリック
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

    private void SelectToyButton(int position)
    {
        if (lastSelectedPosition == position) return; // 既に選択されている場合は何もしない

        if (lastSelectedPosition >= 0)
        {
            ToypasswordButtons[lastSelectedPosition].HideBGToyPanel(); // 前の選択を解除
        }

        ToypasswordButtons[position].ShowBGPanel(); // 新たに選択されたボタンの背景パネルを表示
        lastSelectedPosition = position; // 新しい選択を記録
    }

    private void MoveSelection(float input)
    {
        currentPosition = (currentPosition + (input > 0 ? 1 : -1) + ToypasswordButtons.Length) % ToypasswordButtons.Length; // スロットの移動を処理
        SelectToyButton(currentPosition); // 新たに選択を更新
    }

    private bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (ToypasswordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }
}

