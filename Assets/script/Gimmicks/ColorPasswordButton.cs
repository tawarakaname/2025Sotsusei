using UnityEngine;
using TMPro;

public class ColorPasswordButton : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText; // ボタンの数値を表示するテキスト
    [SerializeField] private Sprite[] images;  // 5択のスプライト配列
    [SerializeField] private SpriteRenderer spriteRenderer; // ボタンのスプライトを表示するSpriteRenderer
    [SerializeField] private SpriteRenderer bgPanel; // 背景パネルとしてのSpriteRenderer

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
        UpdateSprite();  // スプライトを更新
        HideBGColorPanel(); // 背景パネルは最初非表示
    }

    private void UpdateNumberDisplay()
    {
        numberText.text = number.ToString(); // 数値をテキストに反映
    }

    public void OnClickThis()
    {
        // カメラズームと選択状態のフラグが有効な場合にのみ実行
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BoxBCamera) &&
            isSelected)
        {
            number++;
            if (number > 4)  // スプライトと数値が5択であることを反映
            {
                number = 0;
            }
            Debug.Log($"Button clicked, new number: {number}");

            UpdateNumberDisplay();  // 数値表示を更新
            UpdateSprite();  // スプライトを更新
        }
    }

    // 背景パネルを表示する処理
    public void ShowBGPanel()
    {
        bgPanel.color = new Color(1f, 1f, 1f, 1f); // 背景を不透明にして表示
        SetSelectedState(true);
    }

    // 背景パネルを非表示にする処理
    public void HideBGColorPanel()
    {
        bgPanel.color = new Color(1f, 1f, 1f, 0f); // 背景を透明にして非表示
        SetSelectedState(false);
    }

    // ボタンの選択状態を設定
    private void SetSelectedState(bool state)
    {
        isSelected = state;
    }

    // スプライトを現在の数値に基づいて更新
    private void UpdateSprite()
    {
        if (spriteRenderer != null && images != null && images.Length > 0)
        {
            // 現在の数値に対応するスプライトを取得
            int spriteIndex = Mathf.Clamp(number, 0, images.Length - 1);
            Sprite selectedSprite = images[spriteIndex];

            // スプライトをSpriteRendererに設定
            spriteRenderer.sprite = selectedSprite;
        }
    }
    public bool IsButtonActive()
    {
        return spriteRenderer != null && spriteRenderer.enabled; // スプライトが有効かどうかを確認
    }

}
