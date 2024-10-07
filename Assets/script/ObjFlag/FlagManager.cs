using System.Collections.Generic;
using UnityEngine;
using System;

public class FlagManager : MonoBehaviour
{
    // 列挙型：種類を列挙する
    public enum FlagType
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
        Motosen,
        HintA,
        ColorPasswordclear,
        FireParticleON,
        BoxB,
        hasActivatedWalltrace,
        IllustPasswordclear,
        DialPasswordclear,
        Bluetureasure,
        itembox,
        zoompanel,
        CameraZoomObj,
        BoxACamera,
        GasCamera0,
        GasCamera1,
        GasCamera2,
        GasCamera3,
        BoxBCamera,
        UICanvas,
        ThreePasswordclear,
        BdeskCamera,
        capsuleclear,

    }

    // シングルトンインスタンス
    private static FlagManager _instance;

    public static FlagManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // シーンに存在しない場合、新たにオブジェクトを作成する
                GameObject obj = new GameObject("FlagManager");
                _instance = obj.AddComponent<FlagManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    // フラグを管理する辞書
    private Dictionary<object, bool> flagDictionary = new Dictionary<object, bool>();

    private void Start()
    {
        // FlagTypeの全ての値を取得し、辞書に追加
        foreach (FlagType flagType in FlagType.GetValues(typeof(FlagType)))
        {
            flagDictionary.Add(flagType, false);
        }
        // Item.Typeの全ての値を取得し、辞書に追加
        foreach (Item.Type itemType in Enum.GetValues(typeof(Item.Type)))
        {
            flagDictionary.Add(itemType, false);
        }
    }

    private void Awake()
    {
        // シングルトンのインスタンスが重複しないようにする
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // FlagTypeのフラグの取得
    public bool GetFlag(FlagType flagType)
    {
        if (flagDictionary.ContainsKey(flagType))
        {
            return flagDictionary[flagType];
        }
        return false;
    }

    // FlagTypeのフラグの設定
    public void SetFlag(FlagType flagType, bool value)
    {
        if (flagDictionary.ContainsKey(flagType))
        {
            Debug.Log($"フラグ: {flagType}, 新しい値: {value}");
            flagDictionary[flagType] = value;
        }
        else
        {
            Debug.LogWarning("フラグが見つかりません: " + flagType);
        }
    }

    // Item.Typeのフラグの取得
    public bool GetFlagByType(Item.Type itemType)
    {
        if (flagDictionary.ContainsKey(itemType))
        {
            return flagDictionary[itemType];
        }
        return false;
    }

    // Item.Typeのフラグの設定
    public void SetFlagByType(Item.Type itemType, bool value)
    {
        if (flagDictionary.ContainsKey(itemType))
        {
            Debug.Log($"フラグ: {itemType}, 新しい値: {value}");
            flagDictionary[itemType] = value;
        }
        else
        {
            Debug.LogWarning("フラグが見つかりません: " + itemType);
        }
    }
}
