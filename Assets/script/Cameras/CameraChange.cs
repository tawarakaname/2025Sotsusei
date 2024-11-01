using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private Camera gascamera0;   // 最初に使用されるカメラ
    [SerializeField] private Camera gascamera0_1; // 切り替え先のカメラ
    [SerializeField] private CameraZoomObj cameraZoomObj; // CameraZoomObjの参照を追加


    private void Update()
    {
        // FlagManagerのフラグがtrueになったかどうかを確認
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Gasbaneropen))
        {
            // カメラの切り替え処理
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {

        // CameraZoomObjのzoomCameraにgascamera0_1を代入
        if (cameraZoomObj != null)
        {
            cameraZoomObj.zoomCamera = gascamera0_1;
        }
        else
        {
            Debug.LogWarning("cameraZoomObj is not assigned in CameraSwitcher.");
        }
    }
}
