using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManeger : MonoBehaviour
{

    //カメラの切り替え
    //どこにカメラを有効にするのか
    //どのファイルからでも関数を実行できる
    public static CameraManeger instance;

    // メインカメラの参照
    public Camera mainCamera;

    [SerializeField] Transform[] mainCameraTransform;

    private void Awake()
    {
        instance = this;
    }
   

    // メインカメラの初期位置を設定する
    public void SetMainCamera(Camera camera)
    {
        mainCamera = camera;
    }


    public void SetZoomCamera(Camera camera)
    {
        Camera.main.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);
    }

    // メインカメラに戻る
    public void ReturnToMainCamera(Camera camera)
    {
        mainCamera.gameObject.SetActive(true);
        Debug.Log("メインカメラに戻ります: " + mainCamera.name);
      
    }
}
