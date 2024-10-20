using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteA : MonoBehaviour
{
    private string currentKeyword;
    public TextManager textManager; // TextManagerへの参照
    [SerializeField] private GameObject TextBox;
    [SerializeField] private Collider Notecollider;

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがコライダーに接触した場合
        if (other.CompareTag("Player"))
        {
            // どのコライダーに接触したかを確認してキーワードを設定
            if (Notecollider.bounds.Intersects(other.bounds))
            {
                currentKeyword = "NoteA";
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // プレイヤーがコライダーから出た場合、キーワードをリセット
        if (other.CompareTag("Player"))
        {
            currentKeyword = null;
            TextBox.SetActive(false); // コライダーを出た時にTextBoxを非表示にする
        }
    }

    void Update()
    {
        // FlagManagerのフラグ状態を考慮したテキスト表示ロジック
        if (Input.GetButtonDown("Fire2") && currentKeyword != null)
        {
            // Textboxが非表示の場合
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.NoteCamera) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj))
            {
                OnClickNoteThis(); // テキストを表示
            }
            // Textboxが表示されている場合
            else if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                textManager.DisplayCurrentLine(); // 次のテキストラインを表示
                Debug.Log("Displaying next line");
            }
        }

    }
    public void OnClickNoteThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }
}
