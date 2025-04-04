using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomObj : MonoBehaviour
{
    [SerializeField] public Camera zoomCamera;
    [SerializeField] Collider triggerCollider; // ここでコライダーを公開
    [SerializeField] GameObject targetImage; // 表示・非表示を制御するImage


    private CameraManeger cameraManeger;
    private bool playerInsideCollider = false;
    private bool inputCooldown = false; // クールダウンフラグ


    private void Start()
    {
        cameraManeger = CameraManeger.instance;

        if (targetImage == null)
        {
            return;
        }
        else
        {
            targetImage.SetActive(false);
        }
    }

    void Update()
    {
        bool isAllRequiredFlagsOff =
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.itembox) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Option) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Itemgetpanel);

        if (isAllRequiredFlagsOff &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            playerInsideCollider &&
            Input.GetButtonDown("Fire2") &&
            !inputCooldown)
        {
            OnClickThis();
            targetImage.SetActive(false);
            StartCoroutine(InputCooldown()); // クールダウン開始
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
                targetImage.SetActive(false);
            }
            ResetCameraFlag();
        }
    }

    public void OnClickThis()
    {
        if (zoomCamera != null)
        {
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj))
            {

                cameraManeger.SetZoomCamera(zoomCamera);
                SetCameraFlag();
              
            }
        }
        else
        {
            Debug.LogWarning("zoomCamera is null. It might have been destroyed.");
        }
    }

    private void SetCameraFlag()
    {
        var flagToSet = GetFlagTypeFromCameraName(zoomCamera.name);
        if (flagToSet.HasValue)
        {
            FlagManager.Instance.SetFlag(flagToSet.Value, true);
        }
    }

    private void ResetCameraFlag()
    {
        var flagToReset = GetFlagTypeFromCameraName(zoomCamera.name);
        if (flagToReset.HasValue)
        {
            FlagManager.Instance.SetFlag(flagToReset.Value, false);
        }
    }

    private FlagManager.FlagType? GetFlagTypeFromCameraName(string cameraName)
    {
        if (string.IsNullOrEmpty(cameraName))
        {
            return null;
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
            case "StandCamera":
                return FlagManager.FlagType.StandCamera;
            case "OdoguCamera":
                return FlagManager.FlagType.OdoguCamera;
            case "BlueBoxCamera":
                return FlagManager.FlagType.BlueBoxCamera;
            case "JugCamera":
                return FlagManager.FlagType.JugCamera;
            case "BTBCamera":
                return FlagManager.FlagType.BTBCamera;
            case "NoteBCamera":
                return FlagManager.FlagType.NoteBCamera;
            case "NoteCCamera":
                return FlagManager.FlagType.NoteCCamera;
            case "BatteryCamera1":
                return FlagManager.FlagType.BatteryCamera1;
            case "BatteryCamera2":
                return FlagManager.FlagType.BatteryCamera2;
            case "BatteryCamera3":
                return FlagManager.FlagType.BatteryCamera3;
            case "BatteryCamera4":
                return FlagManager.FlagType.BatteryCamera4;
            case "ElectronicCamera":
                return FlagManager.FlagType.ElectronicCamera;
            case "ToyboxCamera":
                return FlagManager.FlagType.ToyboxCamera;
            case "GasCamera2":
                return FlagManager.FlagType.GasCamera2;
            case "robotCamera":
                return FlagManager.FlagType.robotCamera;
            case "LeverCamera":
                return FlagManager.FlagType.LeverCamera;
            case "ToolCamera":
                return FlagManager.FlagType.ToolCamera;
            case "cupCCamera":
                return FlagManager.FlagType.cupCCamera;
            case "NabeCamera":
                return FlagManager.FlagType.NabeCamera;
            case "MainMeterCamera":
                return FlagManager.FlagType.MainMeterCamera;
            case "yappiCamera":
                return FlagManager.FlagType.yappiCamera;
            default:
                return null;
        }
    }

    private IEnumerator InputCooldown()
    {
        inputCooldown = true;
        yield return new WaitForSeconds(0.2f); // 0.2秒のクールダウン
        inputCooldown = false;
    }
}
