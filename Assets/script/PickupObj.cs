using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    [SerializeField] Item.Type itemType;
    [SerializeField] Collider triggerCollider;
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

    // クリックしたらobjを消す
    public void OnClickObj()
    {
        // アイテムを設定
        Itembox.instance.SetItem(item);

        // アイテムが設定された後の処理
        gameObject.SetActive(false);

        // アイテムのタイプに対応するテキストを表示
        TextManager textManager = FindObjectOfType<TextManager>();
        if (textManager != null)
        {
            // テキストが存在しない場合には、TextBoxは表示されない
            textManager.DisplayTextForItemType(item.type);
        }
    }
}
