using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI talkText; // TMPのテキストフィールド
    private Dictionary<FlagManager.FlagType, string> textDictionary;
    [SerializeField] private GameObject TextBox;

    // 会話を進めるためのキー Enter
    public Key PushActionKey { get => Key.Enter; }

    // 現在表示中のテキストとその行インデックス
    private string[] currentTextLines;
    private int currentLineIndex = 0;

    private void Start()
    {
        TextBox.SetActive(false);

        // テキストの内容を初期化
        textDictionary = new Dictionary<FlagManager.FlagType, string>
        {
            { FlagManager.FlagType.capsuleA,
                "a1\n" +
                "a2\n" +
                "a3\n" +
                "a4\n" +
                "fin" },
            { FlagManager.FlagType.capsuleB, "BBBBBBBBBB" },
            { FlagManager.FlagType.bluekey, "bluekeyyyyyyyy" },
            // 他のフラグとテキストを追加
        };
    }

    // 1文字にずつにかかる秒数 0.02秒
    public float CharIntervalSec { get => 0.02f; }

    // フラグに対応するテキストを表示する
    public void DisplayTextForFlag(FlagManager.FlagType flag)
    {
        if (textDictionary.ContainsKey(flag) && FlagManager.Instance.GetFlag(flag))
        {
            // テキストを行ごとに分割して保存
            currentTextLines = textDictionary[flag].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
        }
    }

    // FlagManager.FlagType 型のフラグに対応するテキストが存在するか確認する
    public bool HasTextForFlag(FlagManager.FlagType flag)
    {
        return textDictionary.ContainsKey(flag);
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
        if (Keyboard.current[PushActionKey].wasPressedThisFrame && currentTextLines != null)
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
