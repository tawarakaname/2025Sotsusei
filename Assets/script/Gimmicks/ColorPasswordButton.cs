using UnityEngine;
using TMPro;

public class ColorPasswordButton : MonoBehaviour
{
    [SerializeField] TMP_Text numberText;
    public int number { get; private set; } // 外部から数値を変更できないようにプロパティ化

    // 数字に対応するマテリアル
    [SerializeField] Material[] materials;
    [SerializeField] Renderer objectRenderer;
    [SerializeField] GameObject ColorbgPanel; // 背景パネル


    private FlagManager flagManager;
    public GameObject BgPanel => ColorbgPanel; // 背景パネルの参照を公開

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
        UpdateMaterial();
        HideBGColorPanel(); // 背景パネルは最初非表示
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
            flagManager.GetFlag(FlagManager.FlagType.BoxBCamera) &&
            isSelected)
        {
            number++;
            if (number > 3)
            {
                number = 0;
            }
            UpdateNumberDisplay();
            UpdateMaterial();
        }
    }
    // 背景パネルを表示し、選択状態にする
    public void ShowBGPanel()
    {
        ColorbgPanel.SetActive(true);
        SetSelectedState(true);
    }
    // 背景パネルを非表示にし、選択状態を解除する
    public void HideBGColorPanel()
    {
        ColorbgPanel.SetActive(false);
        SetSelectedState(false);
    }

    // 選択状態を管理
    private void SetSelectedState(bool state)
    {
        isSelected = state;
    }

    private void UpdateMaterial()
    {
        if (objectRenderer != null && materials != null && materials.Length > 0)
        {
            // 現在の数値に対応するマテリアルを適用
            int materialIndex = Mathf.Clamp(number, 0, materials.Length - 1);
            objectRenderer.material = materials[materialIndex];
        }
    }
}
