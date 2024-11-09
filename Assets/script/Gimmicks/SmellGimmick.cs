using UnityEngine;

public class SmellGimmick : MonoBehaviour
{
    private TextManager textManager; // TextManagerへの参照
    [SerializeField] private Collider AquariumCollider1;
    [SerializeField] private Collider AquariumCollider2;
    [SerializeField] private Collider AquariumCollider3;
    [SerializeField] private GameObject TextBox; // TextBoxの参照
    [SerializeField] private GameObject DTextBox;

    private string currentKeyword; // 現在のコライダーに対応するキーワード

    private void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがコライダーに接触した場合
        if (other.CompareTag("Player"))
        {
            // どのコライダーに接触したかを確認してキーワードを設定
            if (AquariumCollider1.bounds.Intersects(other.bounds))
            {
                currentKeyword = "smell1";
                Debug.Log("Player entered AquariumCollider1");
            }
            else if (AquariumCollider2.bounds.Intersects(other.bounds))
            {
                currentKeyword = "smell2";
                Debug.Log("Player entered AquariumCollider2");
            }
            else if (AquariumCollider3.bounds.Intersects(other.bounds))
            {
                currentKeyword = "smell3";
                Debug.Log("Player entered AquariumCollider3");
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

    private void Update()
    {
        // 丸ボタン(Fire2)が押され、かつコライダーに対応するキーワードが存在し、Textboxフラグがfalseの場合
        if (Input.GetButtonDown("Fire2") && currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            OnClicksmellThis();
        }
        else if(Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            textManager.DisplayCurrentLine();
        }
    }


    public void OnClicksmellThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }

}
