using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //アイテムを受け取ったらスロットに表示するようにする
    Item item;
    [SerializeField]Image image;
    [SerializeField] GameObject backgroundPanel; 

    private void Awake()
    {
       // image = GetComponent<Image>();
    }
    private void Start()
    {
        backgroundPanel.SetActive(false);
    }

    //アイテムボックスが空かどうか
    public bool IsEmpty()
    {
        if(item == null)
        {
            return true;
        }
        return false;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        UpdateImage(item);
    }

    public Item GetItem()
    {
        return item;
    }

    //アイテムを受け取ったら"画像"をスロットに表示する
    //アイテムを使用したら、itemboxからアイテムを削除する
    void UpdateImage(Item item)
    {
        if (item == null)
        {
            image.sprite = null;
        }
        else
        {
            image.sprite = item.sprite;
        }
    }

    public bool OnSelected()
    {
        if(item == null)
        {
            return false;
        }
        backgroundPanel.SetActive(true);
        return true;
    }

    public void HideBGPanel()
    {
        backgroundPanel.SetActive(false);
    }

    public bool HasItem()
    {
        return GetItem() != null; // スロットにアイテムが設定されているかどうかを返す
    }

}

