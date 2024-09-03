using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    [SerializeField] Item.Type itemType;
    [SerializeField] FlagManager.FlagType flagToSet;
    [SerializeField] Collider triggerCollider; // 追加したコライダーのフィールド
    [SerializeField] private GameObject TextBox;
    Item item;

    private bool playerInsideCollider = false;

    private void Start()
    {
        // itemタイプに応じてitemを作成する
        item = ItemGenerater.instance.Spawn(itemType);
    }

    void Update()
    {
        if (playerInsideCollider)
        {
            // PS4コントローラーの⚪︎ボタンは「Fire2」として認識されます
            if (Input.GetButtonDown("Fire2"))
            {
                OnClickObj();
                
            }
        }
    }
            private void OnTriggerEnter(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    playerInsideCollider = true;
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    playerInsideCollider = false;
                }
            }

            // クリックしたらログを出す
            // クリックしたらobjを消す

            public void OnClickObj()
            {

                Itembox.instance.SetItem(item);
                gameObject.SetActive(false);

                // フラグを設定する
                FlagManager.Instance.SetFlag(flagToSet, true);
                Debug.Log("FlagON");


                // フラグに対応するテキストを表示
                TextManager textManager = FindObjectOfType<TextManager>();
                if (textManager != null)
                {
                    // テキストが存在するかを確認
                    if (textManager.HasTextForFlag(flagToSet)) 
                    {
                        // テキストボックスを表示
                        TextBox.SetActive(true);
                        textManager.DisplayTextForFlag(flagToSet);
                    }
                }
            }
}
