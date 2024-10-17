using UnityEngine;
using TMPro;

public class DialPasswordButton : MonoBehaviour
{
    private const int MaxNumber = 5; // 最大値
    private const int MinNumber = 0; // 最小値

    [SerializeField] private TMP_Text numberText; // 番号表示用のテキスト
    public int number { get; private set; } // 外部から数値を変更できないプロパティ

    [SerializeField] private Renderer objectRenderer; // オブジェクトのレンダラー
    [SerializeField] private GameObject DialbgPanel; // 背景パネル

    private FlagManager flagManager; // フラグマネージャーのインスタンス
    public GameObject BgPanel => DialbgPanel; // 背景パネルの参照を公開

    private bool isSelected = false; // 選択状態を管理するフラグ

    private void Start()
    {
        InitializeButton(); // ボタンの初期化処理
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得
       
    }


    // ボタンの初期化処理を分離
    private void InitializeButton()
    {
        number = MinNumber; // 初期値を設定
        UpdateNumberDisplay(); // 表示を更新
        HideBGDialPanel(); // 背景パネルは最初非表示
    }

    // 番号の表示を更新
    private void UpdateNumberDisplay()
    {
        numberText.text = number.ToString(); // 数字をテキストに変換して表示
    }

    public void OnClickThis()
    {
        // フラグが有効で、ボタンが選択されている時のみ処理を実行
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.GasCamera0) && isSelected)
        {
            IncrementNumber(); // 数字をインクリメント
        }
    }

    // 数値をインクリメントし、最大値を超えたら0にリセット
    private void IncrementNumber()
    {
        number = (number + 1) % (MaxNumber + 1); // 最大値を超えたら0にリセット
        UpdateNumberDisplay(); // 表示を更新
    }

    // 数字をリセットし、表示を更新
    public void ResetNumber()
    {
        number = 0; // 数字をリセット
        UpdateNumberDisplay(); // 表示を更新
    }

    // 背景パネルを表示し、選択状態にする
    public void ShowBGPanel()
    {
        DialbgPanel.SetActive(true); // 背景パネルを表示
        SetSelectedState(true); // 選択状態にする
    }

    // 背景パネルを非表示にし、選択状態を解除する
    public void HideBGDialPanel()
    {
        DialbgPanel.SetActive(false); // 背景パネルを非表示
        SetSelectedState(false); // 選択状態を解除
    }

    // 選択状態を管理
    private void SetSelectedState(bool state)
    {
        isSelected = state; // 選択状態を設定
    }
}
