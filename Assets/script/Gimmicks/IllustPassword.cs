using UnityEngine;

public class IllustPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f; // 移動に関するクールダウン時間

    [SerializeField] private int[] correctNumbers; // 正解の番号
    [SerializeField] private IllustPasswordButton[] IllustpasswordButtons; // 現在のパネルの数値
    [SerializeField] private GameObject capsuleD; // 最初に非表示のオブジェクト

    private int currentPosition = 0; // 現在選択されているスロットの位置
    private float nextMoveTime = 0f; // 次に移動できる時間を記録
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private FlagManager flagManager; // フラグマネージャーのインスタンス

    private int lastSelectedPosition = -1; // 選択中のボタンのインデックスを保持

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        SelectIllustButton(currentPosition); // 初期選択状態を設定
    }

    public void CheckClear()
    {
        if (IsClear())
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.IllustPasswordclear, true);
            Debug.Log("IllustPasswordclearFlagON");
            capsuleD.SetActive(true);
        }
    }

    private void Update()
    {
        // 必要なフラグが無効な場合は処理を終了
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.BoxACamera))
            return;

        HandleHorizontalInput(); // 水平方向の入力を処理
        HandleFireButtonInput(); // 丸ボタンの入力を処理
    }

    private void HandleHorizontalInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BoxACamera))
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
            flagManager.GetFlag(FlagManager.FlagType.BoxACamera))
        {
            if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
            {
                var currentButton = IllustpasswordButtons[currentPosition];
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

    private void SelectIllustButton(int position)
    {
        if (lastSelectedPosition == position) return; // 既に選択されている場合は何もしない

        // 前の選択を解除
        if (lastSelectedPosition >= 0)
        {
            IllustpasswordButtons[lastSelectedPosition].HideBGIllustPanel();
        }

        // 新たに選択されたボタンの背景パネルを表示
        IllustpasswordButtons[position].ShowBGPanel();
        lastSelectedPosition = position; // 新しい選択を記録
    }

    private void MoveSelection(float input)
    {
        currentPosition = (currentPosition + (input > 0 ? 1 : -1) + IllustpasswordButtons.Length) % IllustpasswordButtons.Length; // スロットの移動を処理
        SelectIllustButton(currentPosition); // 新たに選択を更新
    }

    private bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (IllustpasswordButtons[i].number != correctNumbers[i])
            {
                return false; // クリア条件に満たない場合
            }
        }
        return true; // すべての条件を満たした場合
    }
}
