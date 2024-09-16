using UnityEngine;
using TMPro;

public class DialPasswordButton : MonoBehaviour
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
    }

    public void OnClickThis()
    {
        number++;
        if (number > 9)
        {
            number = 0;
        }
        numberText.text = number.ToString();
    }
}
   
