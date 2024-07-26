using System;
using UnityEngine;

[Serializable]
public class Item
{
    //列挙型：種類を列挙する
    public enum Type
    {
        capsuleA,
        capsuleB,
        capsuleC,
        capsuleD,
        balloon,
        BTB,
        key1,
        bluekey,
        allcup,
        diacup,
        heartcup,
        starcup,
        Dkey1,
        Dkey2,
        dry,
        watercan,
        Skey,
        Mheartcup,
        batteryA,
        batteryB,
        batteryC,
        Mheart,
        Dkey,
        Dkeys,
        dryber,
        HintA,


    }


    public Type type;          // 種類
    public Sprite sprite;      // Slotに表示する画像
    public Sprite zoomObj;     // 拡大表示する画像
    public Sprite zoomsprite;  // 拡大表示する画像(裏)

    // コンストラクタ
    public Item(Type type, Sprite sprite, Sprite zoomObj, Sprite zoomsprite)
    {
        this.type = type;
        this.sprite = sprite;
        this.zoomObj = zoomObj;
        this.zoomsprite = zoomsprite;
    }
}


