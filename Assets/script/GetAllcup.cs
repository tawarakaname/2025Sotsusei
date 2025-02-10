using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllcup : MonoBehaviour
{
    [SerializeField] private Collider AllcupDCollider; 
    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool isTextboxActive; // Textboxが現在アクティブかどうか
    private bool isPlayerInCollider; // プレイヤーがコライダー内にいるかどうか
    [SerializeField] private PickupObj pickupObj;
    [SerializeField] private GameObject UI;

    private static Dictionary<Item.Type, bool> pickeditem = new Dictionary<Item.Type, bool>();

    // Start is called before the first frame update
    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        if (pickupObj != null)
        {
            pickupObj.enabled = false; // 初期状態でスクリプトを無効化
        }
    }

    private void Update()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt2move))
        {
            return;
        }
        if (FlagManager.Instance.GetFlagByType(Item.Type.Allcupwater))
        {
            return;
        }

        // BatteryDが取得済みならコライダーを無効化
        if (pickeditem.ContainsKey(Item.Type.batteryD) && pickeditem[Item.Type.allcup])
        {
            AllcupDCollider.enabled = false;
            pickupObj.enabled = false; // 初期状態でスクリプトを無効化

        }


        // 指定のフラグが全てtrueの場合、Textboxを呼び出す
        if (isPlayerInCollider &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt2move) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.cupCCamera) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.getallcup))
        {

            // Textboxが表示されていない場合、キーワードに基づいて表示
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                currentKeyword = "getallcup";  // 現在のキーワードを設定
                OnClickgetallcupThis();        // Textbox表示処理を実行
            }
        }

        // Fire2入力がある場合にテキストを表示
        if (Input.GetButtonDown("Fire2") && currentKeyword != null &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.getbatterD) && isPlayerInCollider)
        {
            textManager.DisplayCurrentLine();
        }

        // Textboxが終了したらフラグを更新
        if (isPlayerInCollider &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt1move) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.BatteryCamera1) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) == false &&
            isTextboxActive)
        {
            isTextboxActive = false; // Textboxの終了を検知
            FlagManager.Instance.SetFlag(FlagManager.FlagType.getallcup, true); // Tu_05clearフラグを設定
            if (pickupObj != null)
            {
                pickupObj.enabled = true; // 初期状態でスクリプトを無効化
            }
            UI.gameObject.SetActive(true);
        }

        // Textboxがアクティブなら状態を記録
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            isTextboxActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // "Player"タグを持つオブジェクトのみ処理する
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = true; // プレイヤーがコライダー内にいる状態
        }
    }

    public void OnClickgetallcupThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }

}
