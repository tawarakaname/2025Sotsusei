using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManeger : MonoBehaviour
{
    public static CameraManeger instance;

    // メインカメラの参照をキャッシュ
    [SerializeField] private Camera mainCamera;
    public GameObject wipe; // アニメーション対象のオブジェクト
    private Animator wipeAnimator; // Animatorコンポーネント
    [SerializeField] private GameObject wipeobj;

    private Camera currentCamera;
    private FlagManager flagManager;


    private void Awake()
    {
        wipeobj.SetActive(false);

        // シングルトンの実装を安全にする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 重複するインスタンスを破棄
            return;
        }

        // MainCameraの参照を明示的にセットする
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        flagManager = FlagManager.Instance;

        if (wipe != null)
            wipeAnimator = wipe.GetComponent<Animator>();
    }

    private void Update()
    {
        // Itemgetpanel フラグが false の場合のみ処理を実行
        if (!flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel) &&
           (!flagManager.GetFlag(FlagManager.FlagType.Textbox) &&
           (!flagManager.GetFlag(FlagManager.FlagType.Nowanim) &&
           (!flagManager.GetFlag(FlagManager.FlagType.Setanim) &&
            flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj)))))
        {
            // Fire1 ボタンが押された場合のみカメラを戻す処理を実行
            if (Input.GetButtonDown("Fire1"))
            {
                ReturnToMainCamera();
            }
        }

        // wipeの表示非表示をNowanimの状態に依存させる
        if (flagManager.GetFlag(FlagManager.FlagType.wipe))
        {
            bool nowanim = flagManager.GetFlag(FlagManager.FlagType.Nowanim);
            wipeobj.SetActive(!nowanim);
        }
    }



    public void SetZoomCamera(Camera camera)
    {
        if (wipeAnimator != null)
        {
            flagManager.SetFlag(FlagManager.FlagType.wipe, true);
            wipeobj.SetActive(true);
            wipeAnimator.SetTrigger("wipein");
        }


        if (camera == null)
        {
            return;
        }

        // コルーチンを使って遅延切り替えを実現
        StartCoroutine(SwitchCameraWithDelay(camera));
    }

    private IEnumerator SwitchCameraWithDelay(Camera camera)
    {
        // 0.2秒待機
        yield return new WaitForSeconds(0.2f);

        if (camera != currentCamera)
        {
            if (currentCamera != null)
            {
                currentCamera.gameObject.SetActive(false);
            }

            currentCamera = camera;
            mainCamera.gameObject.SetActive(false);
            currentCamera.gameObject.SetActive(true);
            flagManager.SetFlag(FlagManager.FlagType.CameraZoomObj, true);

        }
    }


    // メインカメラに戻る
    public void ReturnToMainCamera()
    {
        Debug.Log("wipeout1");
        if (currentCamera != null && currentCamera.gameObject.activeSelf)
        {
            Debug.Log("wipeout2");
            if (wipeAnimator != null)
            {
                wipeAnimator.SetTrigger("wipeout");
            }

            // アニメーション後の処理をコルーチンで遅延実行
            StartCoroutine(HandleWipeOut());
        }
    }

    private IEnumerator HandleWipeOut()
    {
        // 0.1秒待機して currentCamera を無効化
        yield return new WaitForSeconds(0.3f);
        if (currentCamera != null)
        {
            currentCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            flagManager.SetFlag(FlagManager.FlagType.CameraZoomObj, false);
        }

        // さらに0.5秒待機して他の処理を実行（合計0.3秒）
        yield return new WaitForSeconds(0.2f);

        // アニメーション再生後に実行する処理
        flagManager.SetFlag(FlagManager.FlagType.wipe, false);
        wipeobj.SetActive(false);
        currentCamera = null; // メインカメラに戻ったので、currentCameraをクリア
    }


}
