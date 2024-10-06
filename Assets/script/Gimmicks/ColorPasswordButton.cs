using UnityEngine;
using TMPro;

public class ColorPasswordButton : MonoBehaviour
{
    [SerializeField] TMP_Text numberText;
    public int number { get; private set; } // 外部から数値を変更できないようにプロパティ化

    [SerializeField] Sprite[] images;  // 5択のスプライト配列
    [SerializeField] Renderer objectRenderer;  // QuadのRendererコンポーネント
    [SerializeField] GameObject ColorbgPanel; // 背景パネル

    private FlagManager flagManager;
    public GameObject BgPanel => ColorbgPanel; // 背景パネルの参照を公開

    private bool isSelected = false; // 選択状態を管理するフラグ

    private void Start()
    {
        InitializeButton();
        flagManager = FlagManager.Instance;
    }

    private void InitializeButton()
    {
        number = 0;
        UpdateNumberDisplay();
        UpdateSprite();  // スプライトを更新
        HideBGColorPanel(); // 背景パネルは最初非表示
    }

    private void UpdateNumberDisplay()
    {
        numberText.text = number.ToString();
    }

    public void OnClickThis()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BoxBCamera) &&
            isSelected)
        {
            number++;
            if (number > 4)  // 5択に変更
            {
                number = 0;
            }
            Debug.Log($"ShowBGPanel called on button {number}");

            UpdateNumberDisplay();
            UpdateSprite();  // スプライトを更新
        }
    }

    public void ShowBGPanel()
    {
        ColorbgPanel.SetActive(true);
        SetSelectedState(true);
    }

    public void HideBGColorPanel()
    {
        ColorbgPanel.SetActive(false);
        SetSelectedState(false);
    }

    private void SetSelectedState(bool state)
    {
        isSelected = state;
    }

    private void UpdateSprite()
    {
        if (objectRenderer != null && images != null && images.Length > 0)
        {
            // 現在の数値に対応するスプライトを適用
            int spriteIndex = Mathf.Clamp(number, 0, images.Length - 1);
            Sprite selectedSprite = images[spriteIndex];

            // スプライトをQuadのマテリアルに設定
            Material material = new Material(Shader.Find("Sprites/Default"));
            material.mainTexture = selectedSprite.texture;
            objectRenderer.material = material;
        }
    }
}
