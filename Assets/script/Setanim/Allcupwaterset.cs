using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Allcupwaterset : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private GameObject otomo;
    [SerializeField] private GameObject otomodammy;
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private Canvas CkeyUEget;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ
    [SerializeField] private GameObject CkeyUE; // 最初に非表示のオブジェクト
    //[SerializeField] private GameObject itemgeteffect;

    private bool soundPlaying = false;

    private FlagManager flagManager;
    public PlayableDirector director;

    private bool animationCompleted = false;

    void Start()
    {
        flagManager = FlagManager.Instance;
        audioSource = GetComponent<AudioSource>();

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }

        CkeyUEget.gameObject.SetActive(false);
        //itemgeteffect.gameObject.SetActive(false);
    }

    void Update()
    {

        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.getallcup))
        {
            return;
        }

        if (flagManager.GetFlagByType(Item.Type.Allcupwater) &&
            !flagManager.GetFlag(FlagManager.FlagType.yappiclear))
        {
            Openpot();
        }

        if (animationCompleted)
        {
            if (targetCamera != null)
            {
                targetCamera.SetActive(false);
            }
            animationCompleted = false;

            // 0.7秒の遅延を追加
            StartCoroutine(ShowItemGetPanelWithDelay(0.1f));
        }

        if (CkeyUEget.gameObject.activeSelf &&
            Input.GetButtonDown("Fire2") &&
            flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel))
        {
            CkeyUEget.gameObject.SetActive(false);
            flagManager.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            //itemgeteffect.gameObject.SetActive(false);
            if (playerScript != null) playerScript.enabled = true;
        }
    }

    private void Openpot()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.yappiclear) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
            director.Play();
            DisablePlayerControls();
        }
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        animationCompleted = true;
        flagManager.SetFlag(FlagManager.FlagType.yappiclear, true);
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        
        // otomoの座標を指定した位置に移動
        if (otomo != null)
        {
            otomo.transform.position = new Vector3(3.8f, 2.7f, 8.8f); // 変更したい座標を指定
        }
        otomodammy.gameObject.SetActive(true);
        // フラグ Ckeygivefase を true に設定
        flagManager.SetFlag(FlagManager.FlagType.Ckeygivefase, true);

        EnablePlayerControls();
    }

    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }

    private IEnumerator ShowItemGetPanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //yield return new WaitForSeconds(0.2f);
        //itemgeteffect.gameObject.SetActive(true);
       // yield return new WaitForSeconds(0.5f);
        CkeyUE.SetActive(true);
        Itemgetpanel();
    }

    private void Itemgetpanel()
    {
        if (!soundPlaying)
        {
            soundPlaying = true; // フラグを true に設定

            CkeyUEget.gameObject.SetActive(true);
            flagManager.SetFlag(FlagManager.FlagType.Itemgetpanel, true);

            audioSource.PlayOneShot(soundEffect); // soundEffect（音声データ）を再生
        }
    }
}
