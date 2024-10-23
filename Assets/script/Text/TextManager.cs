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

    public GameObject DTextBox; // D君のTextBox
    public TextMeshProUGUI DtalkText; // D君のテキスト
    [SerializeField] private Image Dicon0; // D君のアイコン（複数ある場合は他のアイコンも追加可能）


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
        DTextBox.SetActive(false);

        textDictionary = new Dictionary<Item.Type, string>
        {
                { Item.Type.capsuleA, "H: なんかおちてたよ！\nD: なんだろうそれ" },
                { Item.Type.capsuleB, "H: これ何かなー？\nD: これはめずらしいものだね" },
                { Item.Type.bluekey, "H: ふしぎなことが起きてるね\nD: そうだね、青いカギかも" },
        };

        keywordTextDictionary = new Dictionary<string, string>
        {
                { "smell1", "H: hallo\nD: You smell something strange." },
                { "smell2", "H: すっぱいにおいがする！\nD: そうだね、少し匂うよね" },
                { "smell3", "H: くさくない\nふんふん\nD: これ何だろう？" },
                { "NoteA", "H:ぼくのノートだ!!!!!\nD: これ、君のノートだったの？" },
                { "Miss", "H: これじゃないみたい...\nD: 何か違うみたいだね" },
                { "BalloonStand", "H: 何かはさめそうだね\nD: スタンド、何かできるかも" },
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
            { "BalloonStand", Hicon1},
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
            DTextBox.SetActive(false);
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
            DisplayImageForKeyword(keyword); // 画像も表示
        }
        else
        {
            TextBox.SetActive(false);
            DTextBox.SetActive(false); // DTextBoxも確実に非表示にする
            Debug.Log("Textboxがfalse");
        }
    }

    // 現在の行を表示するメソッド
    public void DisplayCurrentLine()
    {
        if (currentTextLines != null)
        {
            Debug.Log($"currentLineIndex: {currentLineIndex}, totalLines: {currentTextLines.Length}");

            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                // テキストボックスの表示を開始
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Textbox, true);
                DisplayLineWithFlag(currentTextLines[currentLineIndex]);
                currentLineIndex++;
                Debug.Log($"Index incremented to: {currentLineIndex}");
            }
            else
            {
                if (currentLineIndex < currentTextLines.Length)
                {
                    DisplayLineWithFlag(currentTextLines[currentLineIndex]);
                    currentLineIndex++;
                    Debug.Log($"Index incremented to: {currentLineIndex}");
                }
                else
                {
                    currentLineIndex = 0;

                    if (currentLineIndex == currentTextLines.Length)
                    {
                        // 最後の行を超えたら両方のTextBoxを非表示にする
                        TextBox.SetActive(false);
                        DTextBox.SetActive(false);
                      
                        FlagManager.Instance.SetFlag(FlagManager.FlagType.Textbox, false);
                        Debug.Log("Textboxがfalse");
                    }
                }
            }
        }
    }

    private void DisplayLineWithFlag(string currentLine)
    {
        Debug.Log($"Current line: {currentLine}");

        // H: か D: で表示を切り替える
        if (currentLine.StartsWith("H:"))
        {
            TextBox.SetActive(true);
            DTextBox.SetActive(false); // DTextBoxを非表示
            talkText.text = currentLine.Substring(2).Trim(); // "H:"を除去して表示
            Debug.Log("H: TextBox表示, DTextBox非表示");
        }
        else if (currentLine.StartsWith("D:"))
        {
            DTextBox.SetActive(true);
            TextBox.SetActive(false); // TextBoxを非表示
            DtalkText.text = currentLine.Substring(2).Trim(); // "D:"を除去して表示
            Debug.Log("D: DTextBox表示, TextBox非表示");
        }
        else
        {
            // 何も H: でも D: でもない場合の処理
            TextBox.SetActive(true);
            DTextBox.SetActive(false);
            talkText.text = currentLine.Trim();
            Debug.Log("通常のテキスト表示, DTextBox非表示");
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
