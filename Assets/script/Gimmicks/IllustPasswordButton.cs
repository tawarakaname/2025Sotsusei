using UnityEngine;
using TMPro;

public class IllustPasswordButton : MonoBehaviour
{
    [SerializeField] TMP_Text numberText;
    public int number { get; private set; } // 外部から数値を変更できないようにプロパティ化

    // 数字に対応するスプライト
    [SerializeField] Sprite[] sprites;
    [SerializeField] Renderer objectRenderer; // QuadのRendererコンポーネント
    [SerializeField] GameObject IllustbgPanel; // 背景パネル

    private FlagManager flagManager;
    public GameObject BgPanel => IllustbgPanel; // 背景パネルの参照を公開

    private bool isSelected = false; // 選択状態を管理するフラグ

    private void Start()
    {
        InitializeButton();
        flagManager = FlagManager.Instance;
    }

    // ボタンの初期化処理を分離
    private void InitializeButton()
    {
        number = 0;
        UpdateNumberDisplay();
        UpdateSprite();
        HideBGIllustPanel(); // 背景パネルは最初非表示
    }

    // 番号の表示を更新
    private void UpdateNumberDisplay()
    {
        numberText.text = number.ToString();
    }

    public void OnClickThis()
    {
        // フラグが無効な場合は処理を終了
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BoxACamera) &&
            isSelected)
        {
            number++;
            if (number > 3)
            {
                number = 0;
            }
            UpdateNumberDisplay();
            UpdateSprite(); // スプライトの更新を実行
        }
    }

    // 背景パネルを表示し、選択状態にする
    public void ShowBGPanel()
    {
        IllustbgPanel.SetActive(true);
        SetSelectedState(true);
    }

    // 背景パネルを非表示にし、選択状態を解除する
    public void HideBGIllustPanel()
    {
        IllustbgPanel.SetActive(false);
        SetSelectedState(false);
    }

    // 選択状態を管理
    private void SetSelectedState(bool state)
    {
        isSelected = state;
    }

    // スプライトをQuadに反映
    private void UpdateSprite()
    {
        if (objectRenderer != null && sprites != null && sprites.Length > 0)
        {
            // 現在の数値に対応するスプライトを適用
            int spriteIndex = Mathf.Clamp(number, 0, sprites.Length - 1);
            Sprite selectedSprite = sprites[spriteIndex];

            // スプライトをQuadのマテリアルに設定
            Material material = new Material(Shader.Find("Sprites/Default"));
            material.mainTexture = selectedSprite.texture;
            objectRenderer.material = material;
        }
    }
}
