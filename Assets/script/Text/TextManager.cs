using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI talkText; // TMPのテキストフィールド
    private Dictionary<FlagManager.FlagType, string> textDictionary;
    [SerializeField] private GameObject TextBox;

    // 会話を進めるためのキー Enter
    public Key PushActionKey { get => Key.Enter; }

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
    public  float CharIntervalSec { get => 0.02f; }

    // フラグに対応するテキストを表示する
    public void DisplayTextForFlag(FlagManager.FlagType flag)
    {
        if (textDictionary.ContainsKey(flag) && FlagManager.Instance.GetFlag(flag))
        {
            talkText.text = textDictionary[flag];
        }
        else
        {
            Debug.LogWarning("フラグが見つからないか、フラグが設定されていません: " + flag);
        }
    }


}
