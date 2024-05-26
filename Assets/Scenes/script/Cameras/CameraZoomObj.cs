using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomObj : MonoBehaviour

{
    [SerializeField] Camera zoomCamera;

    private CameraManeger cameraManeger;
    private void Start()
    {
        cameraManeger = CameraManeger.instance;

    }


   
    // クリックしたら、用意してあるカメラに切り替える
    public void OnClickObj()
    {   
        Debug.Log("カメラ切り替え");
        cameraManeger.SetZoomCamera(zoomCamera);
    }


}

