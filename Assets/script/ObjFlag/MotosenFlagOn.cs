using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotosenFlagOn : MonoBehaviour
{
    [SerializeField] Collider triggerCollider;
    [SerializeField] GameObject maruUI;
    private bool playerInsideCollider = false;
    private bool isFireButtonPressed = false;
    private FlagManager flagManager;
    public GameObject hutamawasu;
    private Animator hutamawasuAnimator;
    public GameObject huta;
    private bool isRotating = false;

    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        flagManager = FlagManager.Instance;
        maruUI.SetActive(false);
        hutamawasuAnimator = hutamawasu.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // CameraZoomObjFlagがfalseなら早めに処理を終了
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj)) return;

        // MotosenフラグがtrueならmaruUIを非表示にする
        if (flagManager.GetFlag(FlagManager.FlagType.Motosen))
        {
            maruUI.SetActive(false);
            return; // これ以上の処理をスキップ
        }

        if (playerInsideCollider)
        {
            maruUI.SetActive(true); // プレイヤーがコライダー内にいる場合maruUIを表示
            HandleFireButtonInput(); // ボタン入力を受け付ける
        }
    }

    // 丸ボタンの入力処理
    private void HandleFireButtonInput()
    {
        if (Input.GetButtonDown("Fire2") && !isFireButtonPressed && !isRotating)
        {
            OnClickhutaopen();
            isFireButtonPressed = true;
        }
        else if (!Input.GetButton("Fire2"))
        {
            isFireButtonPressed = false;
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
            maruUI.SetActive(false); // コライダーから出たらUIを非表示にする
        }
    }

    public void OnClickhutaopen()
    {
        audioSource.Play(); // 鳴らしたいタイミングに追加
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Motosen, true);

        hutamawasuAnimator.SetTrigger("Motosenopen");
        maruUI.SetActive(false); // フラグ設定後にmaruUIを非表示にする
        isRotating = true;
    }
}
