using System.Collections;
using UnityEngine;

public class Pillaropen : MonoBehaviour
{
    public GameObject pillar;
    private Animator pillarAnimator;
    private FlagManager flagManager;
    [SerializeField] private Collider pillarcollider;
    [SerializeField] GameObject Monitorpasswordobj;
    [SerializeField] GameObject Mixpasswordobj;
    [SerializeField] Canvas jouroget;
    private bool playerInsideCollider = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private MonoBehaviour playerScript;
    private bool controlsDisabled = false;
    private bool canvasEnabled = false;
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jouroget.gameObject.SetActive(false);
        if (pillar != null) pillarAnimator = pillar.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInsideCollider)
        {
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MonitorPasswordclear))
            {
                Monitorpasswordobj.SetActive(false);
                Mixpasswordobj.SetActive(true);
            }
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear))
            {
                DisablePlayerControls();
                OnClickpillar();
            }
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear) && !canvasEnabled)
            {
                // すでにコルーチンが実行されていない場合のみ開始
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
            }


            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.JugCamera) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                jouroget.gameObject.SetActive(false);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
                playerScript.enabled = true;
            }
        }

        if (controlsDisabled)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }

    public void OnClickpillar()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen))
        {
            StartCoroutine(PlayAnimationsSimultaneously());
            Mixpasswordobj.SetActive(false);
            controlsDisabled = true;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (pillarAnimator != null)
            pillarAnimator.SetTrigger("pillaropen");

        yield return new WaitForSeconds(animatedTime); // アニメーションの再生時間を待機
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Pillaropen, true); // フラグを設定
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        yield return new WaitForSeconds(1f);
        jouroget.gameObject.SetActive(true);

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
        controlsDisabled = true;
    }
}
