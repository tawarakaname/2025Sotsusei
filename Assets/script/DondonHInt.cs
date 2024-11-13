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
        // プレイヤーがコライダーに接触した場合
        if (other.CompareTag("Player"))
        {
            // どのコライダーに接触したかを確認してキーワードを設定
            if (Dondoncollider.bounds.Intersects(other.bounds))
            {
                // FlagManagerのフラグ状態に基づいてcurrentKeywordを設定
                //if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Dondon1kaiwa))
                //{
                //    currentKeyword = "Hint1";
                //}
                //else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Dondon1kaiwa) &&
                //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.Motosen))
                //{
                //    currentKeyword = "Hint2";
                //}
                //else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Motosen) &&
                //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear))
                //{
                //    currentKeyword = "Hint3";
                //}
                //else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear) &&
                //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear))
                //{
                //    currentKeyword = "Hint4";
                //}
                //else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) &&
                //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.capsuleclear))
                //{
                //    currentKeyword = "Hint5";
                //}
                //else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.capsuleclear) &&
                //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear))
                //{
                //    currentKeyword = "Hint6";
                //}
                //else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) &&
                //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget))
                //{
                //    currentKeyword = "Hint7";
                //}
                //else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget) &&
                //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen))
                //{
                //    currentKeyword = "Hint8";
                //}
                //else
                if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) &&
                    !FlagManager.Instance.GetFlag(FlagManager.FlagType.ThreePasswordclear))//仮置き
                {
                    currentKeyword = "Hint9";
                }
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
