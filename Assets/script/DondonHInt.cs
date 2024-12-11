using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DondonHInt : MonoBehaviour
{
    private string currentKeyword;
    private TextManager textManager; // TextManagerへの参照
    //[SerializeField] private GameObject TextBox;
    [SerializeField] private Collider Dondoncollider;
    [SerializeField] GameObject targetImage; // 表示・非表示を制御するImage

    private void Start()
    {
        targetImage.SetActive(false);
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Dondoncollider.bounds.Intersects(other.bounds))
            {
                currentKeyword = Textkeyword.GetKeywordBasedOnFlags();
            }

            if (targetImage != null)
            {
                targetImage.SetActive(true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // プレイヤーがコライダーから出た場合、キーワードをリセット
        if (other.CompareTag("Player"))
        {
            currentKeyword = null;
            //TextBox.SetActive(false); // コライダーを出た時にTextBoxを非表示にする
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Dondon, false);

            if (targetImage != null)
            {
                targetImage.SetActive(false);
            }
        }
    }

    void Update()
    {
        bool isAllRequiredFlagsOff =
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.itembox) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Itemgetpanel);

        // FlagManagerのフラグ状態を考慮したテキスト表示ロジック
        if (isAllRequiredFlagsOff && Input.GetButtonDown("Fire2") && currentKeyword != null)
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Dondon, true);

            // Textboxが非表示の場合
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Dondon))
            {
                OnClickDondon(); // テキストを表示
                targetImage.SetActive(false);

                // Hint1が表示されたらDondon1kaiwaをtrueにする
                if (currentKeyword == "Hint1")
                {
                    FlagManager.Instance.SetFlag(FlagManager.FlagType.Dondon1kaiwa, true);
                }
                // Hint11が表示されたらDondon1kaiwaをtrueにする
                if (currentKeyword == "Hint11")
                {
                    FlagManager.Instance.SetFlag(FlagManager.FlagType.Funfun1kaiwa, true);
                }
            }
            // Textboxが表示されている場合
            else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                textManager.DisplayCurrentLine(); // 次のテキストラインを表示
            }
        }
    }

    public void OnClickDondon()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }
}
