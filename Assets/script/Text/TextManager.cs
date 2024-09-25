using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI talkText; // TMPのテキストフィールド
    private Dictionary<Item.Type, string> textDictionary; // Item.Type に基づくテキスト辞書
    [SerializeField] private GameObject TextBox;

    // 現在表示中のテキストとその行インデックス
    private string[] currentTextLines;
    private int currentLineIndex = 0;

    private void Start()
    {
        TextBox.SetActive(false);

        // テキストの内容を初期化
        textDictionary = new Dictionary<Item.Type, string>
        {
            { Item.Type.capsuleA,
                "a1\n" +
                "a2\n" +
                "a3\n" +
                "a4\n" +
                "fin" },
            { Item.Type.capsuleB, "BBBBBBBBBB" },
            { Item.Type.bluekey, "bluekeyyyyyyyy" },
            // 他のItem.Typeとテキストを追加
        };
    }

    // 1文字にずつにかかる秒数 0.02秒
    public float CharIntervalSec { get => 0.02f; }

    // Item.Typeに対応するテキストを表示する
    public void DisplayTextForItemType(Item.Type itemType)
    {
        if (textDictionary.ContainsKey(itemType))
        {
            currentTextLines = textDictionary[itemType].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
        }
    }

    // 現在の行を表示する
    private void DisplayCurrentLine()
    {
        if (currentTextLines != null && currentLineIndex < currentTextLines.Length)
        {
            talkText.text = currentTextLines[currentLineIndex];
            TextBox.SetActive(true);
        }
    }

    private void Update()
    {
        // PS4コントローラーの丸ボタンで会話を進める
        if (Input.GetButtonDown("Fire2") && currentTextLines != null)
        {
            currentLineIndex++;
            if (currentLineIndex < currentTextLines.Length)
            {
                DisplayCurrentLine();
            }
            else
            {
                // テキストが全て表示された場合、TextBoxを非表示にするなどの処理
                TextBox.SetActive(false);
            }
        }
    }
}
