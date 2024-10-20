using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotosenFlagOn : MonoBehaviour
{
    [SerializeField] Collider triggerCollider; // 追加したコライダーのフィールド
    [SerializeField] GameObject maruUI; // 追加したコライダーのフィールド
    private bool playerInsideCollider = false;
    private bool isFireButtonPressed = false; // 丸ボタンの押下を管理
    private bool maruUIDisplayed = false; // maruUIの表示状態を管理
    private FlagManager flagManager; // フラグマネージャーのインスタンスをキャッシュ
    public GameObject hutamawasu;  // 上のオブジェクト (アニメーションがついているオブジェクト)
    private Animator hutamawasuAnimator;  // 上のオブジェクトのアニメーター

    public GameObject huta; // motosenhuta を指定するためのフィールド
    private bool isRotating = false; // 一度だけ回転させるためのフラグ

    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得
        maruUI.SetActive(false);

        hutamawasuAnimator = hutamawasu.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // CameraZoomObjFlagがfalseなら早めに処理を終了
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj)) return;

        if (playerInsideCollider && !maruUIDisplayed) // maruUIがまだ表示されていない場合のみ表示
        {
            maruUI.SetActive(true); // 一度だけmaruUIを表示
            maruUIDisplayed = false; // 表示したことを記録
        }

        if (playerInsideCollider)
        {
            HandleFireButtonInput(); // プレイヤーがコライダー内にいる間はボタン入力を受け付ける
        }
    }

    // 丸ボタンの入力処理
    private void HandleFireButtonInput()
    {
        // 丸ボタンが押された時の処理
        if (Input.GetButtonDown("Fire2") && !isFireButtonPressed && !isRotating)
        {
            maruUI.SetActive(false); // ボタンが押された瞬間にmaruUIを消す
            maruUIDisplayed = true; // UIを非表示にしたことを記録

            OnClickhutaopen ();
            isFireButtonPressed = true; // ボタンが押されたらフラグをセット
        }
        else if (!Input.GetButton("Fire2"))
        {
            // ボタンが離されたらフラグをリセット
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
            maruUIDisplayed = false; // 表示状態もリセット
        }
    }

    public void OnClickhutaopen()
    {

        audioSource.Play(); //鳴らしたいタイミングに追加
        // フラグを設定する
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Motosen, true);

        hutamawasuAnimator.SetTrigger("Motosenopen");
        isRotating = true;
    }
}
