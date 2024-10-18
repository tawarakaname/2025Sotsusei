using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // 画像のために必要

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI talkText; // TMPのテキストフィールド
    public GameObject TextBox; // TextBoxへの参照を公開

    [SerializeField] private Image Hicon0; // Hicon0 の画像
    [SerializeField] private Image Hicon1; // Hicon1 の画像
    [SerializeField] private Image Hicon2; // Hicon2 の画像
    [SerializeField] private Image Hicon3; // Hicon3 の画像

    private Dictionary<Item.Type, string> textDictionary; // Item.Type に基づくテキスト辞書
    private Dictionary<string, string> keywordTextDictionary; // キーワードに基づくテキスト辞書
    private Dictionary<Item.Type, Image> imageDictionary; // Item.Type に基づく画像辞書
    private Dictionary<string, Image> keywordImageDictionary; // キーワードに基づく画像辞書

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
            { "smell1", "hallo\nYou smell something strange." },
            { "smell2", "nioiarimasu" },
            { "smell3", "kusakunai\naaaa" },
            { "NoteA", "bokunonoteda!!!!!" },
            { "Miss", "korejanaimitai..." },
        };

        // Item.Typeに基づく画像の初期化
        imageDictionary = new Dictionary<Item.Type, Image>
        {
            { Item.Type.capsuleA, Hicon0 },
            { Item.Type.capsuleB, Hicon1 },
            { Item.Type.bluekey, Hicon2 },
        };

        // キーワードに基づく画像の初期化
        keywordImageDictionary = new Dictionary<string, Image>
        {
            { "smell1", Hicon0 },
            { "smell2", Hicon1 },
            { "smell3", Hicon2 },
            { "NoteA", Hicon3 },
            { "Miss", Hicon3 },
        };

        // 最初は画像を全て非表示にする
        Hicon0.gameObject.SetActive(false);
        Hicon1.gameObject.SetActive(false);
        Hicon2.gameObject.SetActive(false);
        Hicon3.gameObject.SetActive(false);
    }

    // Item.Typeに対応するテキストを表示する
    public void DisplayTextForItemType(Item.Type itemType)
    {
        if (textDictionary.ContainsKey(itemType))
        {
            currentTextLines = textDictionary[itemType].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
            DisplayImageForItemType(itemType); // 画像も表示
        }
        else
        {
            TextBox.SetActive(false);
            Debug.Log("Textboxがfalse");
        }
    }

    // キーワードに対応するテキストを表示する
    public void DisplayTextForKeyword(string keyword)
    {
        if (keywordTextDictionary.ContainsKey(keyword))
        {
            currentTextLines = keywordTextDictionary[keyword].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
            DisplayImageForKeyword(keyword); // 画像も表示
        }
        else
        {
            TextBox.SetActive(false);
            Debug.Log("Textboxがfalse");
        }
    }

    // 現在の行を表示する (ここは変えない)
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

    // Item.Typeに対応する画像を表示する
    private void DisplayImageForItemType(Item.Type itemType)
    {
        HideAllImages(); // まずは全ての画像を非表示にする
        if (imageDictionary.ContainsKey(itemType))
        {
            imageDictionary[itemType].gameObject.SetActive(true); // 対応する画像を表示
        }
    }

    // キーワードに対応する画像を表示する
    private void DisplayImageForKeyword(string keyword)
    {
        HideAllImages(); // まずは全ての画像を非表示にする
        if (keywordImageDictionary.ContainsKey(keyword))
        {
            keywordImageDictionary[keyword].gameObject.SetActive(true); // 対応する画像を表示
        }
    }

    // 全ての画像を非表示にする
    private void HideAllImages()
    {
        Hicon0.gameObject.SetActive(false);
        Hicon1.gameObject.SetActive(false);
        Hicon2.gameObject.SetActive(false);
        Hicon3.gameObject.SetActive(false);
    }
}
