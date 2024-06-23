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

    private Camera currentCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ReturnToMainCamera();
        }
    }


    public void SetZoomCamera(Camera camera)
    {
        currentCamera = camera;
        Camera.main.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);
    }

    // メインカメラに戻る
    public void ReturnToMainCamera()
    {
        if(currentCamera != null)
            currentCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        Debug.Log("メインカメラに戻ります: " + mainCamera.name);
      
    }
}
