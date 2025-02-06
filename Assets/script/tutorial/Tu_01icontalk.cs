using UnityEngine;
using System.Collections;

public class Tu_01icontalk : MonoBehaviour
{
    [SerializeField] private Animator UIAnimator; // アニメーション用アニメーター

    [SerializeField] private Collider CauldronCollider; // ゴミ箱コライダー
    [SerializeField] private Collider Tu_01iconCollider; // コライダーAを指定する
    [SerializeField] private GameObject targetImage; // 表示・非表示を制御するImage

    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード
    private bool isTextboxActive; // Textboxが現在アクティブかどうか
    private bool isPlayerInCollider; // プレイヤーがコライダー内にいるかどうか
    private bool hasSetAnimFlag = false; // Setanim フラグをセットしたかどうか

    void Start()
    {
      

        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        isTextboxActive = false; // 初期状態は非アクティブ
        isPlayerInCollider = false; // プレイヤーがコライダーにいない状態
        CauldronCollider.enabled = false;
        if (targetImage == null)
        {
            return;
        }
        else
        {
            targetImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Be_Aclear))
        {
            return;
        }

        if (isPlayerInCollider && !hasSetAnimFlag)
        {
            // Setanim を一度だけセットする
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Setanim, true);
            hasSetAnimFlag = true;
        }

        if (isPlayerInCollider)
        {
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                isTextboxActive = true;
            }
            else if (isTextboxActive)
            {
                DisableTu_01collider();
                isTextboxActive = false;
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Tu_01clear, true);
            }
        }

        if (Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            textManager.DisplayCurrentLine();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetImage != null)
        {
            targetImage.SetActive(true);
        }

        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = true;
            currentKeyword = "Tu_01";

            if (currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox)
                && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_01clear))
            {
                OnClickTu_01This();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetImage != null)
        {
            targetImage.SetActive(false);
        }

        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = false;
        }
    }

    public void OnClickTu_01This()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("maru");
        }
    }

    private IEnumerator DisableTu_01colliderWithDelay()
    {
        yield return new WaitForSeconds(1f);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Setanim, false);

        if (Tu_01iconCollider != null)
        {
            Tu_01iconCollider.enabled = false;
        }

        if (CauldronCollider != null)
        {
            CauldronCollider.enabled = true;
        }

        targetImage.SetActive(false);
        this.enabled = false;
    }

    private void DisableTu_01collider()
    {
        if (UIAnimator != null)
        {
            UIAnimator.SetTrigger("marudefault");
        }

        StartCoroutine(DisableTu_01colliderWithDelay());
    }
}
