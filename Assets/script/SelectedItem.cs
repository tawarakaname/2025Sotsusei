using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [SerializeField] private Image image;


    // アイテムを更新するメソッドを追加
    public void UpdateSelectedItem(Item item)
    {
        if (item != null)
        {
            image.sprite = item.sprite;  // アイテムに対応するスプライトを表示
        }
       
    }
}
