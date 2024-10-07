using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetObj : MonoBehaviour
{
    [SerializeField] GameObject setObject;
    [SerializeField] Item.Type useItem;
    [SerializeField] Collider triggerCollider;

    private bool playerInsideCollider = false;

    void OnEnable()
    {
        UpdateSetObjEnabled(); // 初期化時に有効状態を設定
    }

    void Update()
    {
        // フラグに基づいてこのコンポーネントを有効無効に設定
        UpdateSetObjEnabled();

        // プレイヤーがコライダー内にいて、ボタンが押されたときのみ処理
        if (playerInsideCollider && Input.GetButtonDown("Fire2") && enabled) // enabledがtrueの時のみ処理
        {
            OnClickThis();
        }
    }

    private void UpdateSetObjEnabled()
    {
        // DialPasswordclearフラグも考慮
        enabled =  FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear);
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

    public void OnClickThis()
    {
        // アイテムを使用できるか試み、使用できたら処理を行う
        if (Itembox.instance.TryUseItem(useItem))
        {
            Item selectedItem = Itembox.instance.GetSelectedItem();
            if (selectedItem != null)
            {
                // アイテムのタイプに基づいてフラグをtrueに設定
                FlagManager.Instance.SetFlagByType(selectedItem.type, true);
            }
            setObject.SetActive(true);
        }
    }
}
