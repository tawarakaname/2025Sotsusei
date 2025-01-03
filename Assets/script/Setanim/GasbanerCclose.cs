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

    private bool burntCup3Set = false; // `burntcup3get` が設定済みか追跡
    private Coroutine activePanelCoroutine;

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
    }

    void Update()
    {
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
        // 対応する cup3 オブジェクトをアクティブ化
        if (associatedCup3 != null)
        {
            associatedCup3.SetActive(true);
        }

        // ドアを閉じるアニメーション
        GasCdoorAnimator.SetTrigger("Cdoorclose");

        // 対応する cup2 オブジェクトを非アクティブ化
        if (associatedCup2 != null)
        {
            associatedCup2.SetActive(false);
        }

        // アニメーションが完了するまで待機
        yield return new WaitForSeconds(animatedTime);

        // ドアを開くアニメーション
        GasCdoorAnimator.SetTrigger("Cdooropen");

        // ドア開くアニメーションの完了待機
        yield return new WaitForSeconds(animatedTime); // ここで開くアニメーションの再生時間を待つ

        // ドアのアニメーション完了フラグを設定
        FlagManager.Instance.SetFlag(FlagManager.FlagType.DoorAnimComplete, true);

        // パネルを表示
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        panel.SetActive(true);

        // 音声を再生
        audioSource.PlayOneShot(soundEffect);

        // Fire2キーの入力待機（条件にアニメーション完了フラグを追加）
        yield return new WaitUntil(() =>
            FlagManager.Instance.GetFlag(FlagManager.FlagType.DoorAnimComplete) && // アニメーション完了フラグ
            FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.GasCamera2) &&
            Input.GetButtonDown("Fire2"));

        // パネルを非表示
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
        panel.SetActive(false);

        // フラグを更新
        onPanelUsed?.Invoke();

        // ドアアニメーション完了フラグをリセット
        FlagManager.Instance.SetFlag(FlagManager.FlagType.DoorAnimComplete, false);

        // コルーチンの最後でリセット
        activePanelCoroutine = null;
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
