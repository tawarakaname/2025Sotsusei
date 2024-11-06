using UnityEngine;

public class SmellPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f; // 移動に関するクールダウン時間

    [SerializeField] private int[] correctNumbers; // 正解の番号
    [SerializeField] private SmellPasswordButton[] SmellpasswordButtons; // 現在のパネルの数値
    [SerializeField] private GameObject Bluekey; // 最初に非表示のオブジェクト

    private int currentPosition = 0; // 現在選択されているスロットの位置
    private float nextMoveTime = 0f; // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private int lastSelectedPosition = -1; // 選択中のボタンのインデックスを保持

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectSmellButton(currentPosition); // 初期選択状態を設定
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
            if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
            {
                var currentButton = SmellpasswordButtons[currentPosition];
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
