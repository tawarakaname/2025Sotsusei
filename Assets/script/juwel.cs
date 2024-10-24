using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    // SpriteRenderer を取得
    private SpriteRenderer spriteRenderer;

    // 色の配列 (高彩度のピンクとオレンジ)
    private Color[] colors = {
        Color.yellow,                                  // 黄色
        new Color(0.3f, 1f, 0.5f),                     // ライムグリーン
        new Color(1f, 0.2f, 0.8f),                     // 高彩度ピンク
        new Color(1f, 0.6f, 0f)                        // 高彩度オレンジ
    };

    // 現在の色インデックス
    private int currentColorIndex = 0;

    void Start()
    {
        // SpriteRenderer のコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 初期色を設定
        spriteRenderer.color = colors[currentColorIndex];
    }

    // オブジェクトがクリックされたときに呼ばれる
    void OnMouseDown()
    {
        // 次の色に切り替え
        currentColorIndex = (currentColorIndex + 1) % colors.Length;
        spriteRenderer.color = colors[currentColorIndex];
    }
}
