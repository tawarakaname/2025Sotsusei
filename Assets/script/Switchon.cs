using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchon : MonoBehaviour
{
    [SerializeField] private GameObject ButtonUIImage; // 表示・非表示を制御するImage
    private FlagManager flagManager;                               // フラグマネージャー
    private bool isswitch1ON = false; // トリガーが既に実行されたかどうかを管理
    private bool isswitch2ON = false; // トリガーが既に実行されたかどうかを管理


    void Start()
    {
        // フラグマネージャーのインスタンスを取得
        flagManager = FlagManager.Instance;

    }

    void Update()
    {
        // 共通フラグ取得
        bool MainMeterCamera = flagManager.GetFlag(FlagManager.FlagType.MainMeterCamera);

        // LeverCamera フラグが false なら処理をスキップ
        if (!MainMeterCamera) return;

        // その他のフラグ取得
        bool batteryA = flagManager.GetFlagByType(Item.Type.batteryA);
        bool batteryB = flagManager.GetFlagByType(Item.Type.batteryB);
        bool batteryC = flagManager.GetFlagByType(Item.Type.batteryC);
        bool batteryD = flagManager.GetFlagByType(Item.Type.batteryD);
        bool switch1ON = flagManager.GetFlag(FlagManager.FlagType.switch1ON);
        bool switch2ON = flagManager.GetFlag(FlagManager.FlagType.switch2ON);
        bool Belt1move = flagManager.GetFlag(FlagManager.FlagType.Belt1move);
        bool Belt2move = flagManager.GetFlag(FlagManager.FlagType.Belt2move);


        // `Fire2` 押下時の処理
        if (MainMeterCamera && batteryA && batteryB && !switch1ON && !Belt1move)
        {
            if (ButtonUIImage.activeSelf && Input.GetButtonDown("Fire2"))
            {
                BeltTurnon();
            }
            else if (!switch1ON)
            {
                pushswitch();
            }
        }

        // `Fire2` 押下時の処理
        if (MainMeterCamera && batteryC && batteryD && switch1ON && Belt1move && !switch2ON && !Belt2move)
        {
            if (ButtonUIImage.activeSelf && Input.GetButtonDown("Fire2"))
            {
                BeltTurnon2();
            }
            else if (!switch2ON)
            {
                pushswitch2();
            }
        }
    }

    private void pushswitch()
    {
        // 既にトリガーが実行されている場合は処理を終了
        if (isswitch1ON || flagManager.GetFlag(FlagManager.FlagType.switch1ON))
            return;

        // ButtonUIImageを表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(true);
        }

        // トリガー済みフラグを設定
        isswitch1ON = true;

    }

    private void BeltTurnon()
    {
        { 
           flagManager.SetFlag(FlagManager.FlagType.switch1ON, true);
           // フラグを解除
           isswitch1ON = false;
        }
    }

    private void pushswitch2()
    {
        // 既にトリガーが実行されている場合は処理を終了
        if (isswitch2ON || flagManager.GetFlag(FlagManager.FlagType.switch2ON))
            return;

        // ButtonUIImageを表示
        if (ButtonUIImage != null)
        {
            ButtonUIImage.SetActive(true);
        }

        // トリガー済みフラグを設定
        isswitch2ON = true;

    }

    private void BeltTurnon2()
    {
        {
            flagManager.SetFlag(FlagManager.FlagType.switch2ON, true);
            // フラグを解除
            isswitch2ON = false;
        }
    }

}
