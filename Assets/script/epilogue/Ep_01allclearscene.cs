using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Ep_01allclearscene : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;      // プレイヤーのスクリプト（操作を無効化するため）
    //[SerializeField] private GameObject targetCamera;         // アニメーション後に無効化するカメラ
    [SerializeField] private GameObject Ep01_GameObject;     // Telop_Cが入ったeventsceneのgameobj

    private FlagManager flagManager;                          // フラグマネージャー
    public PlayableDirector director;

    private bool hasStarted = false; // 一度だけ実行するためのフラグ

    void Start()
    {
        flagManager = FlagManager.Instance;
    }

    void Update()
    {
        if (hasStarted || flagManager == null)
        {
            return;
        }

        // Cdooropen が false の間は crystalclear のチェックをしない
        if (!flagManager.GetFlag(FlagManager.FlagType.Cdooropen))
        {
            return;
        }

        // すべての crystal フラグが true の場合に crystalclear を true に設定
        CheckAndSetcrystalClear();

        // crystalclearフラグが true になったタイミングで実行
        if (!hasStarted && flagManager.GetFlag(FlagManager.FlagType.crystalclear))
        {
            hasStarted = true; // 二度と実行しないようにフラグを立てる
            ExecuteEvent();
        }
    }


    private void CheckAndSetcrystalClear()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.crystalclear))
        {
            return; // すでに true なら処理不要
        }
        if (!flagManager.GetFlag(FlagManager.FlagType.crystalclear))
        {
            bool iscrystalAOn = flagManager.GetFlagByType(Item.Type.crystalA);
            bool iscrystalBOn = flagManager.GetFlagByType(Item.Type.crystalB);
            bool iscrystalCOn = flagManager.GetFlagByType(Item.Type.crystalC);
           

            if (iscrystalAOn && iscrystalBOn && iscrystalCOn)
            {
                flagManager.SetFlag(FlagManager.FlagType.crystalclear, true);
            }
        }
    }

    private void ExecuteEvent()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.Ep01_clear))
        {
            DisableScript();
            return;
        }

        Ep01();

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    private void Ep01()
    {
        // タイムラインの再生
        if (director != null)
        {
            director.Play();
        }

        // プレイヤー操作を無効化
        DisablePlayerControls();
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;  // プレイヤーのスクリプトを無効化
        }

        // Telop フラグを設定
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;  // プレイヤーのスクリプトを有効化
        }
    }

    // タイムラインが停止したときに呼ばれる
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // フラグを更新
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        flagManager.SetFlag(FlagManager.FlagType.Ep01_clear, true);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();

        //  Ep01_オブジェクトを非表示
        if (Ep01_GameObject != null)
        {
            Ep01_GameObject.SetActive(false);
        }

        // スクリプトを無効化
        DisableScript();
    }

    private void DisableScript()
    {
        // イベントの登録解除
        if (director != null)
        {
            // タイムラインを強制停止
            director.Stop();
        }

        // スクリプトを無効化
        this.enabled = false;
    }
}
