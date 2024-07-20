using UnityEngine;
using TMPro;

public class ColorPasswordButton : MonoBehaviour
{
    [SerializeField] TMP_Text numberText;
    public int number;
    // 実行されたら数値を変える

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
