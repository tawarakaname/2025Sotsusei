using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    //列挙型：種類を列挙する
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
                Debug.Log("FlagManager instance created");
            }
            return _instance;
        }
    }

    // フラグを管理する辞書
    private Dictionary<FlagType, bool> flagDictionary = new Dictionary<FlagType, bool>();

    private void Start()
    {
        // 列挙型の全ての値を取得し、辞書に追加
        foreach (FlagType flagType in FlagType.GetValues(typeof(FlagType)))
        {
            flagDictionary.Add(flagType, false);
        }
        //Debug.Log("Flags initialized");
    }

    private void Awake()
    {
        // シングルトンのインスタンスが重複しないようにする
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
           // Debug.Log("FlagManager Awake: Instance assigned");
        }
        else
        {
            Destroy(gameObject);
            //Debug.Log("FlagManager Awake: Duplicate instance destroyed");
        }
    }


    // フラグの取得
    public bool GetFlag(FlagType flagtype)
    {
        if (flagDictionary.ContainsKey(flagtype))
        { 
            return flagDictionary[flagtype];
        }
        return false;
    }

    // フラグの設定
    public void SetFlag(FlagType flagtype, bool value)
    {
        if (flagDictionary.ContainsKey(flagtype))
        {
            Debug.Log($"{flagtype}");
            flagDictionary[flagtype] = value;
        }
        else
        {
            Debug.LogWarning("フラグが見つかりません: " + flagtype);
        }
    }

    
}
