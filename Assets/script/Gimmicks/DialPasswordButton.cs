using UnityEngine;
using TMPro;

public class DialPasswordButton : MonoBehaviour
{
    // 定数で最大値・最小値を定義しておく
    private const int MaxNumber = 9;
    private const int MinNumber = 0;

    [SerializeField] TMP_Text numberText;
    public int number { get; private set; } // 外部から数値を変更できないようにプロパティ化

    [SerializeField] Material[] materials;
    [SerializeField] Renderer objectRenderer;

    [SerializeField] GameObject DialbgPanel; // 背景パネル

    private FlagManager flagManager;
    public GameObject BgPanel => DialbgPanel; // 背景パネルの参照を公開

    private bool isSelected = false; // 選択状態を管理するフラグ

    private void Start()
    {
        // 初期化処理
        InitializeButton();

        // FlagManagerのインスタンスを取得
        flagManager = FlagManager.Instance;
    }

    // ボタンの初期化処理を分離
    private void InitializeButton()
    {
        number = MinNumber;
        UpdateNumberDisplay();
        HideBGDialPanel(); // 背景パネルは最初非表示
    }

    // 番号の表示を更新
    private void UpdateNumberDisplay()
    {
        numberText.text = number.ToString();
    }

    // クリックされた時の処理
    public void OnClickThis()
    {
        // カメラズームフラグが有効で、ボタンが選択されている時のみ処理を実行
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) && isSelected)
        {
            IncrementNumber();
        }
    }

    // 数値をインクリメントして、10を超えたら0にリセット
    private void IncrementNumber()
    {
        number++;
        if (number > MaxNumber)
        {
            number = MinNumber;
        }
        UpdateNumberDisplay(); // 表示の更新
    }

    // 背景パネルを表示し、選択状態にする
    public void ShowBGPanel()
    {
        DialbgPanel.SetActive(true);
        SetSelectedState(true);
    }

    // 背景パネルを非表示にし、選択状態を解除する
    public void HideBGDialPanel()
    {
        DialbgPanel.SetActive(false);
        SetSelectedState(false);
    }

    // 選択状態を管理
    private void SetSelectedState(bool state)
    {
        isSelected = state;
    }
}
