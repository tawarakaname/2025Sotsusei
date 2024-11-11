using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // 画像のために必要
using System.Collections;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI talkText; // TMPのテキストフィールド

    public GameObject TextBox; // TextBoxへの参照を公開
    [SerializeField] private Image HIcon;
    [SerializeField] private Sprite[] HSprite;

    public GameObject OtherTextBox; // D君のTextBox
    private Image OtherTextBoxImage;
    public TextMeshProUGUI OtherTalkText; // D君のテキスト
    [SerializeField] private Image OtherIcon; // D君のアイコン（複数ある場合は他のアイコンも追加可能）
    private Dictionary<string, Sprite> targetOtomoDictionary;

    [SerializeField] private Sprite[] otomoTextFlame;
    [SerializeField] private Sprite[] otomo0Sprite;
    [SerializeField] private Sprite[] otomo1Sprite;
    [SerializeField] private Sprite[] otomo2Sprite;
    [SerializeField] private Sprite[] otomo3Sprite;

    private Dictionary<string, Sprite> otomo0SpriteDictionary;
    private Dictionary<string, Sprite> otomo1SpriteDictionary;
    private Dictionary<string, Sprite> otomo2SpriteDictionary;
    private Dictionary<string, Sprite> otomo3SpriteDictionary;

    private Dictionary<Item.Type, Sprite> spriteDictionary; // Item.Type に基づく画像辞書
    private Dictionary<string, Sprite> keywordSpriteDictionary; // キーワードに基づく画像辞書

    // 現在表示中のテキストとその行インデックス
    private string[] currentTextLines;
    private int currentLineIndex = 0;

    [Header("入力無効時間")] [SerializeField] private float buttonOsemasen;
    private float currentTime;
    [SerializeField] private Image inputPredictImage;
    private MessageStrage messageStrage = new();

    private void Awake()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        var target = GameObject.FindGameObjectWithTag(gameObject.tag);
        var checkResult = target != null && target != gameObject;

        if (checkResult)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TextBox.SetActive(false);
        OtherTextBox.SetActive(false);

        OtherTextBoxImage = OtherTextBox.GetComponent<Image>();

        // Item.Typeに基づく画像の初期化
        spriteDictionary = new Dictionary<Item.Type, Sprite>
        {
            { Item.Type.capsuleA, HSprite[0] },
            { Item.Type.capsuleB, HSprite[1] },
            { Item.Type.capsuleC, HSprite[0] },
            { Item.Type.bluekey, HSprite[2] }
        };

        // キーワードに基づく画像の初期化
        keywordSpriteDictionary = new Dictionary<string, Sprite>
        {
            { "smell1", HSprite[0] },
            { "smell2", HSprite[1] },
            { "smell3", HSprite[2] },
            { "NoteA", HSprite[3] },
            { "Miss", HSprite[3] },
            { "BalloonStand", HSprite[1]},
            { "BlueBox", HSprite[3]},
            { "Hint5", HSprite[2]}
        };
        // 使用例
        otomo0SpriteDictionary = new Dictionary<string, Sprite>
        {
            { "smile", otomo0Sprite[0] },
        };
        otomo1SpriteDictionary = new Dictionary<string, Sprite>
        {
            { "smile", otomo1Sprite[0] },
        };
        otomo2SpriteDictionary = new Dictionary<string, Sprite>
        {
            { "smile", otomo2Sprite[0] },
        };
        otomo3SpriteDictionary = new Dictionary<string, Sprite>
        {
            { "smile", otomo3Sprite[0] },
        };
        // 

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "A":
                targetOtomoDictionary = otomo0SpriteDictionary;
                OtherIcon.sprite = otomo0Sprite[0];
                OtherTextBoxImage.sprite = otomoTextFlame[0];
                break;
            case "B":
                targetOtomoDictionary = otomo1SpriteDictionary;
                OtherIcon.sprite = otomo1Sprite[0];
                OtherTextBoxImage.sprite = otomoTextFlame[1];
                break;
            case "C":
                targetOtomoDictionary = otomo2SpriteDictionary;
                OtherIcon.sprite = otomo2Sprite[0];
                OtherTextBoxImage.sprite = otomoTextFlame[2];
                break;
            case "D":
                targetOtomoDictionary = otomo3SpriteDictionary;
                OtherIcon.sprite = otomo3Sprite[0];
                OtherTextBoxImage.sprite = otomoTextFlame[3];
                break;
        }
    }

    // Item.Typeに対応するテキストを表示する
    public void DisplayTextForItemType(Item.Type itemType)
    {
        if (messageStrage.TextDictionary.ContainsKey(itemType))
        {
            currentTextLines = messageStrage.TextDictionary[itemType].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
            DisplayImageForItemType(itemType); // 画像も表示
        }
        else
        {
            TextBox.SetActive(false);
            OtherTextBox.SetActive(false);
        }
    }

    public void DisplayTextForKeyword(string keyword)
    {
        if (messageStrage.KeywordTextDictionary.ContainsKey(keyword))
        {
            currentTextLines = messageStrage.KeywordTextDictionary[keyword].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
            DisplayImageForKeyword(keyword); // 画像も表示
        }
        else
        {
            TextBox.SetActive(false);
            OtherTextBox.SetActive(false); // DTextBoxも確実に非表示にする
        }
    }

    private void Update()
    {
        var inputEnabled = currentTime <= 0;
        inputPredictImage.enabled = inputEnabled;

        var isTalking = FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox);
        if (!isTalking) return;
        if (inputEnabled) return;
        currentTime -= Time.deltaTime;
    }

    // 現在の行を表示するメソッド
    public void DisplayCurrentLine()
    {
        if (currentTextLines != null)
        {
            if (currentTime > 0) return;
            currentTime = buttonOsemasen;
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                // テキストボックスの表示を開始
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Textbox, true);
                DisplayLineWithFlag(currentTextLines[currentLineIndex]);
                currentLineIndex++;
            }
            else
            {
                if (currentLineIndex < currentTextLines.Length)
                {
                    DisplayLineWithFlag(currentTextLines[currentLineIndex]);
                    currentLineIndex++;
                }
                else
                {
                    // 最後の行を超えたら両方のTextBoxを非表示にする
                    TextBox.SetActive(false);
                    OtherTextBox.SetActive(false);
                    currentTime = 0;

                    FlagManager.Instance.SetFlag(FlagManager.FlagType.Textbox, false);
                }
            }
        }
    }

    private void DisplayLineWithFlag(string currentLine)
    {

        if (currentLine.StartsWith("H:"))
        {
            TextBox.SetActive(true);
            OtherTextBox.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(TypeTextCoroutine(talkText, currentLine.Substring(2).Trim()));  // "H:" を除去して表示
        }
        else if (currentLine.StartsWith("D:"))
        {
            OtherTextBox.SetActive(true);
            TextBox.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(TypeTextCoroutine(OtherTalkText, currentLine.Substring(2).Trim()));  // "D:" を除去して表示
        }
        else
        {
            TextBox.SetActive(true);
            OtherTextBox.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(TypeTextCoroutine(talkText, currentLine.Trim()));  // 通常のテキスト表示
        }
    }


    // Item.Typeに対応する画像を表示する
    private void DisplayImageForItemType(Item.Type itemType)
    {
        if (spriteDictionary.TryGetValue(itemType, out var value))
        {
            HIcon.sprite = value;
        }
    }

    // キーワードに対応する画像を表示する
    private void DisplayImageForKeyword(string keyword)
    {
        if (keywordSpriteDictionary.TryGetValue(keyword, out var value))
        {
            HIcon.sprite = value;
        }
    }

    private IEnumerator TypeTextCoroutine(TextMeshProUGUI textField, string fullText)
    {
        textField.text = "";  // 表示をクリア
        foreach (char c in fullText)
        {
            textField.text += c;  // 一文字ずつ追加
            yield return new WaitForSeconds(0.1f);  // 0.1秒待機
        }
    }
}