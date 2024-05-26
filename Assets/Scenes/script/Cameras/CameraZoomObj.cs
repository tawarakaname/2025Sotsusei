using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomObj : MonoBehaviour

{
    [SerializeField] Camera zoomCamera;
    [SerializeField] Camera mainCamera;

    void Start()
    {
        // CameraManagerにメインカメラを設定
        CameraManeger.instance.SetMainCamera(mainCamera);
        Debug.Log("CameraManagerにメインカメラを設定しました: " + mainCamera.name);
    }

    // クリックしたら、用意してあるカメラに切り替える
    public void OnClickObj()
    {   
        Debug.Log("カメラ切り替え");
        CameraManeger.instance.SetZoomCamera(zoomCamera);
    }

    // もう一度クリックしたらメインカメラに戻る
    public void OnClickReturnMainCamera()
    {
        Debug.Log("メインカメラに戻ります");
        CameraManeger.instance.ReturnToMainCamera(mainCamera);
    }
}

