using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI talkText;
    public GameObject TextBox;
    [SerializeField] private Image Hicon0;
    [SerializeField] private Image Hicon1;
    [SerializeField] private Image Hicon2;
    [SerializeField] private Image Hicon3;

    public GameObject DTextBox;
    public TextMeshProUGUI DtalkText;
    [SerializeField] private Image Dicon0;

    private Dictionary<Item.Type, string> textDictionary;
    private Dictionary<string, string> keywordTextDictionary;
    private Dictionary<Item.Type, Image> imageDictionary;
    private Dictionary<string, Image> keywordImageDictionary;

    private string[] currentTextLines;
    private int currentLineIndex = 0;

    private bool isInputAllowed = false; // Fire2の入力許可フラグ

    private void Start()
    {
        TextBox.SetActive(false);
        DTextBox.SetActive(false);

        textDictionary = new Dictionary<Item.Type, string>
        {
            { Item.Type.capsuleA, "H: なんかおちてたよ！\nD: なんだろうそれ！" },
            { Item.Type.capsuleB, "H: これ何かなー？\nD: これは…！" },
            { Item.Type.bluekey, "H: ふしぎなことが起きてるね\nD: そうだね、青いカギかも" },
        };

        keywordTextDictionary = new Dictionary<string, string>
        {
            { "smell1", "H: hallo\nD: You smell something strange." },
            { "smell2", "H: すっぱいにおいがする！\nD: そうだね、少し匂うよね" },
        };

        imageDictionary = new Dictionary<Item.Type, Image>
        {
            { Item.Type.capsuleA, Hicon0 },
            { Item.Type.capsuleB, Hicon1 },
            { Item.Type.bluekey, Hicon2 },
        };

        keywordImageDictionary = new Dictionary<string, Image>
        {
            { "smell1", Hicon0 },
            { "smell2", Hicon1 },
            { "smell3", Hicon2 },
            { "NoteA", Hicon3 },
        };

        HideAllImages();
    }

    public void DisplayTextForItemType(Item.Type itemType)
    {
        if (textDictionary.ContainsKey(itemType))
        {
            currentTextLines = textDictionary[itemType].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
            DisplayImageForItemType(itemType);
        }
        else
        {
            TextBox.SetActive(false);
            DTextBox.SetActive(false);
        }
    }

    public void DisplayTextForKeyword(string keyword)
    {
        if (keywordTextDictionary.ContainsKey(keyword))
        {
            currentTextLines = keywordTextDictionary[keyword].Split('\n');
            currentLineIndex = 0;
            DisplayCurrentLine();
            DisplayImageForKeyword(keyword);
        }
        else
        {
            TextBox.SetActive(false);
            DTextBox.SetActive(false);
        }
    }

    public void DisplayCurrentLine()
    {
        if (currentTextLines != null)
        {
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
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
                    TextBox.SetActive(false);
                    DTextBox.SetActive(false);
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
            DTextBox.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(TypeTextWithDelay(talkText, currentLine.Substring(2).Trim()));
        }
        else if (currentLine.StartsWith("D:"))
        {
            DTextBox.SetActive(true);
            TextBox.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(TypeTextWithDelay(DtalkText, currentLine.Substring(2).Trim()));
        }
        else
        {
            TextBox.SetActive(true);
            DTextBox.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(TypeTextWithDelay(talkText, currentLine.Trim()));
        }
    }

    private IEnumerator TypeTextWithDelay(TextMeshProUGUI textField, string fullText)
    {
        textField.text = "";
        foreach (char c in fullText)
        {
            textField.text += c;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.8f); // Fire2入力を許可するまでの待機時間
        isInputAllowed = true;
    }

    public bool IsInputAllowed()
    {
        return isInputAllowed;
    }

    private void HideAllImages()
    {
        Hicon0.gameObject.SetActive(false);
        Hicon1.gameObject.SetActive(false);
        Hicon2.gameObject.SetActive(false);
        Hicon3.gameObject.SetActive(false);
    }

    private void DisplayImageForItemType(Item.Type itemType)
    {
        HideAllImages();
        if (imageDictionary.ContainsKey(itemType))
        {
            imageDictionary[itemType].gameObject.SetActive(true);
        }
    }

    private void DisplayImageForKeyword(string keyword)
    {
        HideAllImages();
        if (keywordImageDictionary.ContainsKey(keyword))
        {
            keywordImageDictionary[keyword].gameObject.SetActive(true);
        }
    }
}
