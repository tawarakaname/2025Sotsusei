using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomObj : MonoBehaviour
{
    [SerializeField] Camera zoomCamera;
    [SerializeField] Collider triggerCollider;
    [SerializeField] GameObject targetImage; // 表示・非表示を制御するImage

    private CameraManeger cameraManeger;
    private bool playerInsideCollider = false;

    private void Start()
    {
        cameraManeger = CameraManeger.instance;
        // Imageが設定されていない場合の警告
        if (targetImage == null)
        {
            Debug.LogWarning("targetImageが設定されていません。Inspectorで設定してください。");
        }
        else
        {
            // Imageを非表示にする
            targetImage.SetActive(false);
        }
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

            if (targetImage != null)
            {
                targetImage.SetActive(true);
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;

            if (targetImage != null)
            {
                targetImage.SetActive(false); // Imageを非表示
            }
            // プレイヤーがコライダーから出たときにフラグをリセット
            ResetCameraFlag();
        }
    }

    // クリックしたら、用意してあるカメラに切り替える
    public void OnClickThis()
    {
        if (zoomCamera != null) // カメラが存在する場合のみ処理を実行
        {
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj))
            {
                cameraManeger.SetZoomCamera(zoomCamera);
                SetCameraFlag();
                FlagManager.Instance.SetFlag(FlagManager.FlagType.CameraZoomObj, true);

            }
        }
        else
        {
            Debug.LogWarning("zoomCamera is null. It might have been destroyed.");
        }
    }


    private void SetCameraFlag()
    {
        // カメラの名前に基づいてフラグを設定
        var flagToSet = GetFlagTypeFromCameraName(zoomCamera.name);
        if (flagToSet.HasValue)
        {
            FlagManager.Instance.SetFlag(flagToSet.Value, true);
        }
    }

    private void ResetCameraFlag()
    {
        // カメラの名前に基づいてフラグをリセット
        var flagToReset = GetFlagTypeFromCameraName(zoomCamera.name);
        if (flagToReset.HasValue)
        {
            FlagManager.Instance.SetFlag(flagToReset.Value, false);
        }
    }

    private FlagManager.FlagType? GetFlagTypeFromCameraName(string cameraName)
    {
        // カメラ名に基づいてフラグタイプを返す
        if (string.IsNullOrEmpty(cameraName))
        {
            return null; // 名前がない場合はnullを返す
        }

        switch (cameraName)
        {
            case "BoxACamera":
                return FlagManager.FlagType.BoxACamera; 
            case "GasCamera0":
                return FlagManager.FlagType.GasCamera0;
            case "BoxBCamera":
                return FlagManager.FlagType.BoxBCamera;
            case "BdeskCamera":
                return FlagManager.FlagType.BdeskCamera;
            case "Aquariumcamera0":
                return FlagManager.FlagType.Aquariumcamera0;
            case "Aquariumcamera1":
                return FlagManager.FlagType.Aquariumcamera1;
            case "Aquariumcamera2":
                return FlagManager.FlagType.Aquariumcamera2;
            case "NoteCamera":
                return FlagManager.FlagType.NoteCamera;
            // 他のカメラの名前に基づくフラグを追加
            default:
                return null; // 名前がない場合や既知でないカメラ名の場合はnullを返す
        }
    }
    private void OnDestroy()
    {
        Debug.Log("Camera has been destroyed.");
    }
    
}
