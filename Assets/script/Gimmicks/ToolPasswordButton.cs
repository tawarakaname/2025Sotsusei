using UnityEngine;
using TMPro;

public class ToolPasswordButton : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText; // ボタンの数値を表示するテキスト
    [SerializeField] private GameObject bgPanel; // 背景パネル
    [SerializeField] private GameObject lightPanel; // 背景パネル2

    private FlagManager flagManager;
    private bool isSelected = false; // 選択状態を管理するフラグ
    public int number { get; private set; } // 外部から数値を変更できないようにプロパティ化

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        InitializeButton(); // ボタンの初期化
    }

    private void InitializeButton()
    {
        number = 0;
        UpdateNumberDisplay();  // 数値の表示を更新
        HideBGToolPanel(); // 背景パネルは最初非表示
    }

    private void UpdateNumberDisplay()
    {
        numberText.text = number.ToString(); // 数値をテキストに反映

        // 数値が1のときlightPanelをアクティブにする
        if (lightPanel != null)
        {
            lightPanel.SetActive(number == 1);
        }
    }

    public void OnClickToolButtonThis()
    {
        // カメラズームと選択状態のフラグが有効な場合にのみ実行
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.ToolCamera) &&
            isSelected)
        {
            number++;
            if (number > 1)  // スプライトと数値が2択であることを反映
            {
                number = 0;
            }

            UpdateNumberDisplay();  // 数値表示を更新
        }
    }

    // 背景パネルを表示する処理
    public void ShowBGPanel()
    {
        if (bgPanel != null)
        {
            bgPanel.gameObject.SetActive(true); // bgPanelがnullでないことを確認
        }
        SetSelectedState(true);
    }

    // 背景パネルを非表示にする処理
    public void HideBGToolPanel()
    {
        if (bgPanel != null)
        {
            bgPanel.gameObject.SetActive(false);
        }
        SetSelectedState(false);
    }

    // ボタンの選択状態を設定
    private void SetSelectedState(bool state)
    {
        isSelected = state;
    }
}
