using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomObj : MonoBehaviour
{
    [SerializeField] Camera zoomCamera;
    [SerializeField] Collider triggerCollider;

    private CameraManeger cameraManeger;
    private bool playerInsideCollider = false;

    private void Start()
    {
        cameraManeger = CameraManeger.instance;
    }

    void Update()
    {
        // CameraZoomObjFlagがfalseかつプレイヤーがコライダー内にいて、ボタンが押されたときのみ処理
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) && playerInsideCollider && Input.GetButtonDown("Fire2"))
        {
            OnClickThis();
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
        }
    }

    // クリックしたら、用意してあるカメラに切り替える
    public void OnClickThis()
    {
        // CameraZoomObjFlagがまだfalseの場合のみフラグをセット
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj))
        {
            cameraManeger.SetZoomCamera(zoomCamera);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.CameraZoomObj, true);
        }
    }
}
