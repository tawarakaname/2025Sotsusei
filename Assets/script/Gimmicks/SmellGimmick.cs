using UnityEngine;

public class SmellGimmick : MonoBehaviour
{
    private TextManager textManager; // TextManagerへの参照
    [SerializeField] private Collider AquariumCollider1;
    [SerializeField] private Collider AquariumCollider2;
    [SerializeField] private Collider AquariumCollider3;
    [SerializeField] private GameObject TextBox; // TextBoxの参照
    [SerializeField] private GameObject DTextBox;
    [SerializeField] private GameObject sankakuUI1; // コライダー1に対応するUI
    [SerializeField] private GameObject sankakuUI2; // コライダー2に対応するUI
    [SerializeField] private GameObject sankakuUI3; // コライダー3に対応するUI

    private string currentKeyword; // 現在のコライダーに対応するキーワード

    private void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear)) return;

        // プレイヤーがコライダーに接触した場合
        if (other.CompareTag("Player"))
        {
            if (AquariumCollider1.bounds.Intersects(other.bounds))
            {
                currentKeyword = "smell1";
            }
            else if (AquariumCollider2.bounds.Intersects(other.bounds))
            {
                currentKeyword = "smell2";
            }
            else if (AquariumCollider3.bounds.Intersects(other.bounds))
            {
                currentKeyword = "smell3";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear)) return;

        // プレイヤーがコライダーから出た場合、キーワードをリセット
        if (other.CompareTag("Player"))
        {
            currentKeyword = null;
            TextBox.SetActive(false); // コライダーを出た時にTextBoxを非表示にする
            HideAllSankakuUI(); // UIも非表示にする
        }
    }

    private void Update()
    {
        // SmellPasswordClearフラグがtrueの場合、UIを非表示にしてスクリプトを無効化
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear))
        {
            HideAllSankakuUI();
            this.enabled = false; // スクリプト自体を無効化
            return;
        }

        // カメラフラグとコライダーフラグが合致する場合にsankakuUIを表示する処理
        if (currentKeyword == "smell1" &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Aquariumcamera0))
        {
            sankakuUI1.SetActive(true);
        }
        else if (currentKeyword == "smell2" &&
                 FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                 FlagManager.Instance.GetFlag(FlagManager.FlagType.Aquariumcamera1))
        {
            sankakuUI2.SetActive(true);
        }
        else if (currentKeyword == "smell3" &&
                 FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                 FlagManager.Instance.GetFlag(FlagManager.FlagType.Aquariumcamera2))
        {
            sankakuUI3.SetActive(true);
        }

        // 三角UI表示中にFire3が押された場合、OnClicksmellThisを実行
        if (Input.GetButtonDown("Fire3") && currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            OnClicksmellThis();

        }
        else if (Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            textManager.DisplayCurrentLine();
        }
    }

    // すべてのsankakuUIを非表示にするメソッド
    private void HideAllSankakuUI()
    {
        sankakuUI1.SetActive(false);
        sankakuUI2.SetActive(false);
        sankakuUI3.SetActive(false);
    }

    public void OnClicksmellThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }
}
