using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dryberset : MonoBehaviour
{
    public GameObject ironplateanim;  //  (アニメーションがついているオブジェクト)
    private Animator ironplateAnimator;  // アニメーター

    [SerializeField] GameObject TextBox; // TextBoxへの参照を公開
    [SerializeField] private GameObject DTextBox;
    private TextManager textManager; // TextManagerへの参照を公開
    [SerializeField] Collider robotCollider;
    [SerializeField] Canvas batteryBget;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool playerInsideCollider = false;
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    private Coroutine ironplateCoroutine;

    [SerializeField] private SetObj setObj;
    private bool isFreeInteract = true;
    private bool firstInteract;

    void Start()
    {
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen))
        //{
        //    enabled = false;
        //    return;
        //}

        audioSource = GetComponent<AudioSource>();
        batteryBget.gameObject.SetActive(false);

        if (ironplateanim != null) ironplateAnimator = ironplateanim.GetComponent<Animator>();
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        setObj.IsFreeInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // dryberフラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.dryber))
        {
            // プレイヤーがコライダーに接触した場合
            if (other.CompareTag("Player"))
            {
                if (robotCollider.bounds.Intersects(other.bounds))
                {
                    currentKeyword = "robot";
                    playerInsideCollider = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // dryberフラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.dryber))
        {
            // プレイヤーがコライダーから出た場合、キーワードをリセット
            if (other.CompareTag("Player"))
            {
                currentKeyword = null;
                TextBox.SetActive(false); // コライダーを出た時にTextBoxを非表示にする
                playerInsideCollider = false;
            }
        }
    }

    private void Update()
    {
        // Adooropen フラグが true の場合、このスクリプトを無効化
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen))
        //{
        //    enabled = false;
        //    return;
        //}

        isFreeInteract = !FlagManager.Instance.GetFlag(FlagManager.FlagType.toolPasswordclear);
        setObj.IsFreeInteract = FlagManager.Instance.GetFlag(FlagManager.FlagType.toolPasswordclear);

        if (FlagManager.Instance.GetFlagByType(Item.Type.dryber) && ironplateCoroutine == null)
        {
            Playironplatetriger();
            DisablePlayerControls();
            ironplateCoroutine = StartCoroutine(ironplateAnimCompleted());
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.batteryBget) && !canvasEnabled)
        {
            StartCoroutine(EnableCanvasAD());
            canvasEnabled = true;
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.playironplate) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.batteryBget) &&
            playerInsideCollider &&
            Input.GetButtonDown("Fire2"))
        {
            batteryBget.gameObject.SetActive(false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            playerScript.enabled = true;
        }

        if (!isFreeInteract) return;

        if (firstInteract)
        {
            OnClickrobotThis();
            firstInteract = false;
        }

        if (playerInsideCollider)
        {
            if (!FlagManager.Instance.GetFlagByType(Item.Type.dryber))
            {
                if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.toolPasswordclear)　&& (FlagManager.Instance.GetFlag(FlagManager.FlagType.robotCamera)))
                {
                    if (Input.GetButtonDown("Fire2") && currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
                    {
                        firstInteract = true;
                    }
                    else if (!firstInteract && Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
                    {
                        textManager.DisplayCurrentLine();
                    }
                }
            }
        }
    }

    private IEnumerator ironplateAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.playironplate, true);
    }
    public void OnClickrobotThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }


    private void Playironplatetriger()
    {
        /// playironplateがまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.playironplate))
        {
            Playanimationironplate();
        }

        FlagManager.Instance.SetFlag(FlagManager.FlagType.batteryBget, true);
    }

    private void Playanimationironplate()
    {
        ironplateAnimator.SetTrigger("ironplateanim");
    }

    private IEnumerator EnableCanvasAD()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 6秒待機
        yield return new WaitForSeconds(6f);
        batteryBget.gameObject.SetActive(true);
        // Akeygetをtrueにする

        if (!itemgetpanelLogged)
        {
            itemgetpanelLogged = true;
        }
        audioSource.PlayOneShot(soundEffect); // 音声クリップを再生
        Debug.Log("正解のSEを流します");
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null) playerScript.enabled = false;
    }
}