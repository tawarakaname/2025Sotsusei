using UnityEngine;
using TMPro;

public class IllustPasswordButton : MonoBehaviour
{
    [SerializeField] TMP_Text numberText;
    public int number;

    // 数字に対応するマテリアル
    [SerializeField] Material[] materials;
    [SerializeField] Renderer objectRenderer;

    private void Start()
    {
        number = 0;
        numberText.text = number.ToString();
        UpdateMaterial();
    }

    public void OnClickThis()
    {
        number++;
        if (number > 3)
        {
            number = 0;
        }
        numberText.text = number.ToString();
        UpdateMaterial();
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
