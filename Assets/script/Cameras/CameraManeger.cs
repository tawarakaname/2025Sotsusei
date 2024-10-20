using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManeger : MonoBehaviour
{
    public static CameraManeger instance;

    // メインカメラの参照をキャッシュ
    [SerializeField] private Camera mainCamera;

    private Camera currentCamera;
    private FlagManager flagManager;

    private void Awake()
    {
        // シングルトンの実装を安全にする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
    }

    private void Update()
    {
        // ボタンが押されたら、フラグを確認
        if (Input.GetButtonDown("Fire1"))
        {
            // Itemgetpanel フラグが false の場合のみ処理を実行
            if (!flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel))
            {
                if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj))
                {
                    ReturnToMainCamera();
                    flagManager.SetFlag(FlagManager.FlagType.CameraZoomObj, false);
                }
            }
        }
    }


    public void SetZoomCamera(Camera camera)
    {
        if (camera == null)
        {
            Debug.LogWarning("Attempted to set a null camera as the zoom camera.");
            return;
        }

        if (camera != currentCamera)
        {
            if (currentCamera != null)
            {
                currentCamera.gameObject.SetActive(false);
            }

            currentCamera = camera;
            mainCamera.gameObject.SetActive(false);
            currentCamera.gameObject.SetActive(true);
        }
    }


    // メインカメラに戻る
    public void ReturnToMainCamera()
    {
        if (currentCamera != null && currentCamera.gameObject.activeSelf)
        {
            currentCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            currentCamera = null; // メインカメラに戻ったので、currentCameraをクリア
        }
    }
}
