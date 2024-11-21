using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluekeySet : MonoBehaviour
{
    public GameObject BlueBoxanim;  //  (アニメーションがついているオブジェクト)
    private Animator BlueBoxAnimator;  // アニメーター

    [SerializeField] Collider BlueBoxCollider;
    [SerializeField] Canvas Bluekeyget;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool playerInsideCollider = false;
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    private Coroutine BlueBoxCoroutine;
    private TextManager textManager;

    [SerializeField] private SetObj setObj;
    private bool isFreeInteract = true;
    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        audioSource = GetComponent<AudioSource>();
        Bluekeyget.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (BlueBoxanim != null) BlueBoxAnimator = BlueBoxanim.GetComponent<Animator>();
        setObj.IsFreeInteract = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // balloon フラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.bluekey))
        {
            // プレイヤーがコライダーに接触した場合
            if (other.CompareTag("Player"))
            {
                if (BlueBoxCollider.bounds.Intersects(other.bounds))
                {
                    currentKeyword = "BlueBox";
                    playerInsideCollider = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // balloon フラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.bluekey))
        {
            // プレイヤーがコライダーから出た場合、キーワードをリセット
            if (other.CompareTag("Player"))
            {
                currentKeyword = null;
                playerInsideCollider = false;
            }
        }
    }

    private void Update()
    {
        isFreeInteract = !FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen);
        setObj.IsFreeInteract = FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen);

        //  Bluekey フラグが true ならアニメーションを再生
        if (FlagManager.Instance.GetFlagByType(Item.Type.bluekey) && BlueBoxCoroutine == null)
        {
            PlayBlueBoxtriger();
            DisablePlayerControls();
            BlueBoxCoroutine = StartCoroutine(BlueBoxAnimCompleted());
        }

        //  Bluekey フラグが true ならアニメーションを再生
        if (FlagManager.Instance.GetFlagByType(Item.Type.bluekey) && BlueBoxCoroutine == null)
        {
            PlayBlueBoxtriger();
            DisablePlayerControls();
            BlueBoxCoroutine = StartCoroutine(BlueBoxAnimCompleted());
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Bluekeyget) && !canvasEnabled)
        {
            StartCoroutine(EnableCanvasAD());
            canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
        }

        // すべての条件が揃ったときの確認
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.playBlueBox) &&
            playerInsideCollider &&
            Input.GetButtonDown("Fire2"))
        {
            // Fire2が押されたらCanvasをfalseに
            Bluekeyget.gameObject.SetActive(false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            FlagManager.Instance.SetFlagByType(Item.Type.bluekey, false);
            playerScript.enabled = true;
        }

        if (!isFreeInteract) return;

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.BlueBoxCamera))
        {
            //  BlueBox フラグが false の場合のみ、コライダーとFire2に関連した動作を行う
            if (!FlagManager.Instance.GetFlagByType(Item.Type.bluekey))
            {
                // SmellPasswordclear フラグが false の場合のみ Fire2 ボタンと TextBox の処理を行う
                if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear))
                {

                    // Fire2ボタンが押され、かつ currentKeyword が null でない場合
                    if (Input.GetButtonDown("Fire2") && currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
                    {
                        OnClickBlueBox();
                    }
                    else if (Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
                    {
                        textManager.DisplayCurrentLine();
                    }

                }
            }
        }
    }

    private IEnumerator BlueBoxAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.playBlueBox, true);
    }
    public void OnClickBlueBox()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }


    private void PlayBlueBoxtriger()
    {
        /// playBlueBoxopen がまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.playBlueBox))
        {
            PlayanimationBlueBox();
        }

        FlagManager.Instance.SetFlag(FlagManager.FlagType.Bluekeyget, true);
    }

    private void PlayanimationBlueBox()
    {
        BlueBoxAnimator.SetTrigger("BlueBoxopen");
    }

    private IEnumerator EnableCanvasAD()
    {
        // bluekeyフラグがtrueでない場合、処理を終了
        if (!FlagManager.Instance.GetFlagByType(Item.Type.bluekey)) yield break;

        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 0.8秒待機
        yield return new WaitForSeconds(0.8f);
        Bluekeyget.gameObject.SetActive(true);

        if (!itemgetpanelLogged)
        {
            itemgetpanelLogged = true;
        }
        audioSource.PlayOneShot(soundEffect); // 音声クリップを再生
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null) playerScript.enabled = false;
    }
}
