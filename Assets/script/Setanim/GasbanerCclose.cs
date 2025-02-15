using System.Collections;
using UnityEngine;

public class GasbanerCclose : MonoBehaviour
{
    public GameObject GasCdoor; // アニメーションがついたオブジェクト
    private Animator GasCdoorAnimator; // GasCdoorのアニメーター
    [SerializeField] private GameObject[] panels; // Panelの配列
    [SerializeField] private Collider GasbanerCcollider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private GameObject[] cup2s; // 各フラグに対応するGameObject
    [SerializeField] private GameObject[] cup3s; // 各フラグに対応するGameObject (アクティブ化用)

    private bool playerInsideCollider = false;
    [SerializeField] private float animatedTime;

    private bool heartUsed = false;
    private bool diaUsed = false;
    private bool starUsed = false;

    private bool burntCup3Set = false; // burntcup3get が設定済みか追跡
    private Coroutine activePanelCoroutine;

    [SerializeField] private GameObject itemgeteffect;

    void Start()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false); // すべてのPanelを非表示にする
        }

        if (GasCdoor != null)
            GasCdoorAnimator = GasCdoor.GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // シーン移動時にフラグをリセット
        heartUsed = false;
        diaUsed = false;
        starUsed = false;
        burntCup3Set = false;
    }


    void Update()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ThreePasswordclear))
        {
            return; // Updateメソッドを早期終了
        }
        // BTBフラグがfalseの場合、一切動作しない
        if (!FlagManager.Instance.GetFlagByType(Item.Type.BTB))
        {
            return; // Updateメソッドを早期終了
        }
        if (!playerInsideCollider) return;

        // いずれかのフラグがtrueで未使用の場合に処理を実行
        if (FlagManager.Instance.GetFlagByType(Item.Type.heartcup2) && !heartUsed)
        {
            if (activePanelCoroutine == null)
                activePanelCoroutine = StartCoroutine(HandlePanel(panels[0], cup2s[0], cup3s[0], () => heartUsed = true));
        }
        else if (FlagManager.Instance.GetFlagByType(Item.Type.starcup2) && !starUsed)
        {
            if (activePanelCoroutine == null)
                activePanelCoroutine = StartCoroutine(HandlePanel(panels[1], cup2s[1], cup3s[1], () => starUsed = true));
        }
        else if (FlagManager.Instance.GetFlagByType(Item.Type.diacup2) && !diaUsed)
        {
            if (activePanelCoroutine == null)
                activePanelCoroutine = StartCoroutine(HandlePanel(panels[2], cup2s[2], cup3s[2], () => diaUsed = true));
        }

        // すべてのフラグがtrueで、まだburntcup3getを設定していない場合に設定
        if (!burntCup3Set && heartUsed && diaUsed && starUsed)
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.burntcup3get, true);
            burntCup3Set = true; // 処理済みと記録
        }
    }

    private IEnumerator HandlePanel(GameObject panel, GameObject associatedCup2, GameObject associatedCup3, System.Action onPanelUsed)
    {
      

        try
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);

            if (associatedCup3 != null)
                associatedCup3.SetActive(true);

            GasCdoorAnimator.SetTrigger("Cdoorclose");

            if (associatedCup2 != null)
                associatedCup2.SetActive(false);

            yield return new WaitForSeconds(1.2f);

            GasCdoorAnimator.SetTrigger("Cdooropen");

            yield return new WaitForSeconds(0.2f);

            FlagManager.Instance.SetFlag(FlagManager.FlagType.DoorAnimComplete, true);

            itemgeteffect.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            audioSource.PlayOneShot(soundEffect);
            panel.SetActive(true);

            // Fire2の入力があるまで無制限に待つ
            while (!(FlagManager.Instance.GetFlag(FlagManager.FlagType.DoorAnimComplete) &&
                     FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                     FlagManager.Instance.GetFlag(FlagManager.FlagType.GasCamera2) &&
                     Input.GetButtonDown("Fire2")))
            {
                yield return null; // 1フレーム待機して再チェック
            }

            itemgeteffect.gameObject.SetActive(false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            panel.SetActive(false);

            onPanelUsed?.Invoke();
            FlagManager.Instance.SetFlag(FlagManager.FlagType.DoorAnimComplete, false);
        }
        finally
        {
            activePanelCoroutine = null; // 確実にリセット
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
}