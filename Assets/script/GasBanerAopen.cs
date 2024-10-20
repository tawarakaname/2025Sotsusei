using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBannerAOpen : MonoBehaviour
{
    [SerializeField] private GameObject gasBannerDoor;      // アニメーションが付いているオブジェクト
    [SerializeField] private Collider gasBannerACollider;   // プレイヤーが入るコライダー
    [SerializeField] private MonoBehaviour playerScript;    // プレイヤーのスクリプト（操作を無効化するため）

    private Animator gasBannerDoorAnimator;                 // オブジェクトのアニメーター
    private bool playerInsideCollider = false;              // プレイヤーがコライダー内にいるかどうか
    private bool controlsDisabled = false;                  // プレイヤー操作が無効かどうか
    private FlagManager flagManager;                        // フラグマネージャー
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        if (gasBannerDoor != null)
        {
            gasBannerDoorAnimator = gasBannerDoor.GetComponent<Animator>();  // アニメーターを取得
        }
        flagManager = FlagManager.Instance;  // フラグマネージャーのインスタンスを取得
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInsideCollider && flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear) &&
            !flagManager.GetFlag(FlagManager.FlagType.Gasbaneropen))
        {
            DisablePlayerControls();  // プレイヤー操作を無効化
            OpenGasBanner();          // ガスバナーを開く
        }

        // プレイヤー操作が無効な場合はFire1, Fire2の入力を無視する
        if (controlsDisabled && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")))
        {
            return;  // 入力を無効化
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;  // プレイヤーがコライダーに入った
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;  // プレイヤーがコライダーから出た
        }
    }

    private void OpenGasBanner()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Gasbaneropen))
        {
            gasBannerDoorAnimator.SetTrigger("GasbanerAopen");  // アニメーションを再生
            audioSource.Play(); //鳴らしたいタイミングに追加
            flagManager.SetFlag(FlagManager.FlagType.Gasbaneropen, true);  // フラグをセットして1度だけ開くようにする
            playerScript.enabled = true;
        }
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;  // プレイヤーのスクリプトを無効化
        }
        controlsDisabled = true;  // Fire1, Fire2の入力を無効化
    }
}
