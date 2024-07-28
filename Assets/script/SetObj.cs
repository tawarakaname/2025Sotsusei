using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObj : MonoBehaviour
{
    [SerializeField] GameObject setObject;
    [SerializeField] Item.Type useItem;
    [SerializeField] Collider triggerCollider; // 追加したコライダーのフィールド

    private bool playerInsideCollider = false;

    void Update()
    {
        if (playerInsideCollider)
        {
            // PS4コントローラーの⚪︎ボタンは「Fire2」として認識されます
            if (Input.GetButtonDown("Fire2"))
            {
                OnClickThis();
                Debug.Log("⚪︎ボタンが押されました！");
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


    //OBJをクリックしたら
    public void OnClickThis()
    {
        bool hasItem = Itembox.instance.TryUseItem(useItem);
        if(hasItem)

        //アイテムを表示する
        setObject.SetActive(true);
    }
}
