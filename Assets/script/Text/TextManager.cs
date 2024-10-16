using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI talkText; // TMPのテキストフィールド
    public GameObject TextBox; // TextBoxへの参照を公開

    private Dictionary<Item.Type, string> textDictionary; // Item.Type に基づくテキスト辞書
    private Dictionary<string, string> keywordTextDictionary; // キーワードに基づくテキスト辞書
    private FlagManager flagManager;

    // 現在表示中のテキストとその行インデックス
    private string[] currentTextLines;
    private int currentLineIndex = 0;

    private void Start()
    {
        TextBox.SetActive(false);

        // Item.Typeに基づくテキストの内容を初期化
        textDictionary = new Dictionary<Item.Type, string>
        {
            { Item.Type.capsuleA, "a1\na2\na3\na4\nfin" },
            { Item.Type.capsuleB, "BBBBBBBBBB" },
            { Item.Type.bluekey, "bluekeyyyyyyyy" },
        };

        // キーワードに基づくテキストの内容を初期化
        keywordTextDictionary = new Dictionary<string, string>
        {
           
            { "smell1", "hallo/You smell something strange." },
            { "smell2", " /nioiarimasu" },
            { "smell3", " /kusakunai\aaaaa" },
        };
    }

    // Item.Typeに対応するテキストを表示する
    public void DisplayTextForItemType(Item.Type itemType)
    {
        if (textDictionary.ContainsKey(itemType))
        {
            currentTextLines = textDictionary[itemType].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
        }
        else
        {
            TextBox.SetActive(false);
            Debug.Log("Textboxがfalse");
        }
    }

    public void DisplayTextForKeyword(string keyword)
    {
        if (keywordTextDictionary.ContainsKey(keyword))
        {
            currentTextLines = keywordTextDictionary[keyword].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
        }
        else
        {
            TextBox.SetActive(false);
            Debug.Log("Textboxがfalse");
        }
    }


    // 現在の行を表示する

    public void DisplayCurrentLine()
    {
        if (currentTextLines != null)
        {
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                TextBox.SetActive(true); // ここでテキストボックスを表示
                Debug.Log("Textboxがtrue");
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Textbox, true);
                talkText.text = currentTextLines[currentLineIndex];
                currentLineIndex++;
            }
            else
            {
                if (currentLineIndex < currentTextLines.Length)
                {
                    talkText.text = currentTextLines[currentLineIndex];
                    currentLineIndex++;
                }
                else
                {
                    TextBox.SetActive(false);
                    currentLineIndex = 0;
                    FlagManager.Instance.SetFlag(FlagManager.FlagType.Textbox, false);
                    Debug.Log("Textboxがfalse");
                }
            }
            
        }
    }


    private void Textlineskip()
    {
        // テキストが表示されている場合にのみ、丸ボタンで進める
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            //if (Input.GetButtonDown("Fire2"))
            {
                currentLineIndex++;
                if (currentLineIndex < currentTextLines.Length)
                {
                    DisplayCurrentLine();
                }
                else
                {
                    // テキストが全て表示された場合、TextBoxを非表示にする
                    TextBox.SetActive(false);
                    Debug.Log("Textboxがfalse");
                }
            }
        }
    }
}
