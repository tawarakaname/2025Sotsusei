using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnim : MonoBehaviour
{
    public Camera mainCamera;
    public Camera[] cameras;
    public GameObject[] objects;

    public Player playerController; // プレイヤーのコントローラースクリプト
    private int currentCameraIndex = 0;
    private bool isSwitching = false;
    private float switchInterval = 2f;
    private float switchTimer = 0f;
    private bool hasReturnedToMainCamera = false;

    private FlagManager flagManager;

    private void Start()
    {
        flagManager = FlagManager.Instance;
        mainCamera.gameObject.SetActive(true);
        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
        playerController.enabled = true; // ゲーム開始時にはプレイヤー操作を有効
    }

    private void Update()
    {
        if (hasReturnedToMainCamera) return;

        if (flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear) && !isSwitching)
        {
            isSwitching = true;
            switchTimer = switchInterval;
            playerController.enabled = false; // カメラ切り替え開始時にプレイヤー操作を無効化
        }

        if (isSwitching)
        {
            switchTimer -= Time.deltaTime;

            if (switchTimer <= 0f)
            {
                SwitchCamera();
                switchTimer = switchInterval;
            }
        }
    }



    private void SwitchCamera()
    {
        // Deactivate the current camera and object
        cameras[currentCameraIndex].gameObject.SetActive(false);
        objects[currentCameraIndex].SetActive(false);

        if (currentCameraIndex == cameras.Length - 1)
        {
            // If returning to the main camera
            mainCamera.gameObject.SetActive(true);
            isSwitching = false;
            hasReturnedToMainCamera = true;
            playerController.enabled = true; // Enable player controls after switching back
        }
        else
        {
            // Switch to the next camera
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            cameras[currentCameraIndex].gameObject.SetActive(true);
            objects[currentCameraIndex].SetActive(true);

            // Get the Animator component from the new camera and trigger the animation
            Animator cameraAnimator = cameras[currentCameraIndex].GetComponent<Animator>();
            if (cameraAnimator != null)
            {
                cameraAnimator.SetTrigger("YourTriggerName"); // Replace "YourTriggerName" with the actual trigger name you use in the Animator
            }
        }
    }
}
