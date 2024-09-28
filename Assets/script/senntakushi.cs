using UnityEngine;
using UnityEngine.UI;

public class SimpleButtonSelector : MonoBehaviour
{
    public Button element1; // ボタン1
    public Button element2; // ボタン2

    void Start()
    {
        // ボタンにクリックイベントを直接設定
        element1.onClick.AddListener(OnElement1Selected);
        element2.onClick.AddListener(OnElement2Selected);
    }

    void Update()
    {
        // "ばつ"ボタンで element1 を選択
        if (Input.GetButtonDown("Fire1")) // ばつボタン
        {
            OnElement1Selected();
        }
        // "しかく"ボタンで element2 を選択
        else if (Input.GetButtonDown("Fire3")) // しかくボタン
        {
            OnElement2Selected();
        }
    }

    // element1 が選択された時の処理
    private void OnElement1Selected()
    {
        Debug.Log("Element1 のアクションが実行されました");
        // Element1の処理をここに記述
    }

    // element2 が選択された時の処理
    private void OnElement2Selected()
    {
        Debug.Log("Element2 のアクションが実行されました");
        // Element2の処理をここに記述
    }
}
