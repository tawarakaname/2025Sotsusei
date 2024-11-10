using System.Collections;
using UnityEngine;

public class BalloonSet : MonoBehaviour
{
    public GameObject balloonanim;  //  (アニメーションがついているオブジェクト)
    private Animator balloonAnimator;  // アニメーター

    [SerializeField] GameObject TextBox; // TextBoxへの参照を公開
    [SerializeField] private GameObject DTextBox;
    private TextManager textManager; // TextManagerへの参照を公開
    [SerializeField] Collider standCollider;
    [SerializeField] Canvas Akeyget;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool playerInsideCollider = false;
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    private Coroutine balloonCoroutine;

    [SerializeField] private SetObj setObj;
    private bool isFreeInteract = true;
    private bool firstInteract;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Akeyget.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (balloonanim != null) balloonAnimator = balloonanim.GetComponent<Animator>();
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        setObj.IsFreeInteract = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // balloon フラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.balloon))
        {
            // プレイヤーがコライダーに接触した場合
            if (other.CompareTag("Player"))
            {
                if (standCollider.bounds.Intersects(other.bounds))
                {
                    currentKeyword = "BalloonStand";
                    playerInsideCollider = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // balloon フラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.balloon))
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
        isFreeInteract = !FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear);
        setObj.IsFreeInteract = FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear);

        // balloon フラグが true ならアニメーションを再生
        if (FlagManager.Instance.GetFlagByType(Item.Type.balloon) && balloonCoroutine == null)
        {
            Playballoontriger();
            DisablePlayerControls();
            balloonCoroutine = StartCoroutine(balloonAnimCompleted());
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget) && !canvasEnabled)
        {
            StartCoroutine(EnableCanvasAD());
            canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
        }
        // すべての条件が揃ったときの確認
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.playballoon) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget) &&
            playerInsideCollider &&
            Input.GetButtonDown("Fire2"))
        {

            // Fire2が押されたらCanvasをfalseに
            Akeyget.gameObject.SetActive(false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            playerScript.enabled = true;
        }

        if (!isFreeInteract) return;

        if (firstInteract)
        {
            OnClickstandThis();
            firstInteract = false;
        }

        if (playerInsideCollider)
        {
            // balloon フラグが false の場合のみ、コライダーとFire2に関連した動作を行う
            if (!FlagManager.Instance.GetFlagByType(Item.Type.balloon))
            {
                // IllustPasswordclear フラグが false の場合のみ Fire2 ボタンと TextBox の処理を行う
                if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear))
                {
                    // Fire2ボタンが押され、かつ currentKeyword が null でない場合
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

    private IEnumerator balloonAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.playballoon, true);
    }
    public void OnClickstandThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }


    private void Playballoontriger()
    {
        /// playballoonopen がまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.playballoon))
        {
            Playanimationballoon();
        }

        FlagManager.Instance.SetFlag(FlagManager.FlagType.Akeyget, true);
    }

    private void Playanimationballoon()
    {
        balloonAnimator.SetTrigger("balloonanim");
    }

    private IEnumerator EnableCanvasAD()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 6秒待機
        yield return new WaitForSeconds(6f);
        Akeyget.gameObject.SetActive(true);
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
