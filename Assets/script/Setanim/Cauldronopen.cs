using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldronopen : MonoBehaviour
{

    public GameObject Cauldronanim;  
    private Animator CauldronAnimator;  
    [SerializeField] private Collider Cauldroncollider;
    private bool playerInsideCollider = false;

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    [SerializeField] private float animatedTime;
    private Coroutine CauldronCoroutine;


    void Start()
    {
        // ueとshitaのAnimatorコンポーネントを取得
        if (Cauldronanim != null) CauldronAnimator = Cauldronanim.GetComponent<Animator>();

    }


    void Update()
    {

        // プレイヤーがコライダー内にいる場合のみ処理を行う
        if (playerInsideCollider)
        {
            if (FlagManager.Instance.GetFlagByType(Item.Type.caudlonkey) && CauldronCoroutine == null &&
                !FlagManager.Instance.GetFlag(FlagManager.FlagType.Cauldronopen))
            {
                OnClickCauldron();
                DisablePlayerControls();
                CauldronCoroutine = StartCoroutine(CauldronAnimCompleted());
            }


            if (playerInsideCollider && FlagManager.Instance.GetFlag(FlagManager.FlagType.Cauldronopen) && controlsDisabled)
            {
                EnablePlayerControls();
            }


        }


        if (controlsDisabled)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                return;
            }
            return; // 早期リターンで他の処理をスキップ
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
          // プレイヤーがコライダーに接触した場合
          if (other.CompareTag("Player"))
          {
              if (Cauldroncollider.bounds.Intersects(other.bounds))
              {
                  playerInsideCollider = true;
              }
          }
        
    }

    private void OnTriggerExit(Collider other)
    {
        // dryberフラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.caudlonkey))
        {
            // プレイヤーがコライダーから出た場合、キーワードをリセット
            if (other.CompareTag("Player"))
            {
                playerInsideCollider = false;
            }
        }
    }


    public void OnClickCauldron()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Cauldronopen))
        {
            StartCoroutine(PlayAnimationsSimultaneously());
            controlsDisabled = true;
        }

    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (CauldronAnimator != null) CauldronAnimator.SetTrigger("Cauldronopen");
        yield return null;

    }



    private void DisablePlayerControls()
    {
        if (playerScript != null) playerScript.enabled = false;
        controlsDisabled = true;
    }

    private IEnumerator CauldronAnimCompleted()
    {
        // アニメーションが完了するまで待機
        yield return new WaitForSeconds(animatedTime);

        // フラグを更新
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Cauldronopen, true);

        // プレイヤーの操作を再有効化
        EnablePlayerControls();
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }
        controlsDisabled = false; // フラグをリセット
    }

}
