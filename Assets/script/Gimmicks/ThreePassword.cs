using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePassword : MonoBehaviour
{
    [SerializeField] private int[] correctNumbers;
    [SerializeField] private ThreePasswordButton[] ThreepasswordButtons;

    private int currentPosition = 0;
    private float nextMoveTime = 0f;
    private bool isFireButtonPressed = false;

    private FlagManager flagManager;
    private int lastSelectedPosition = -1;

    private void Start()
    {
        flagManager = FlagManager.Instance;
        SelectThreeButton(currentPosition);
    }

    public void CheckClear()
    {
        if (IsClear())
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.ThreePasswordclear, true);
            Debug.Log("ThreePasswordclearFlagON");
        }
    }

    private void Update()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) ||
            !flagManager.GetFlag(FlagManager.FlagType.GasCamera0)) return;

        HandleHorizontalInput();
        HandleFireButtonInput();
    }

    private void HandleHorizontalInput()
    {
        if (Time.time >= nextMoveTime)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal Stick-L");

            if (horizontalInput != 0)
            {
                MoveSelection(horizontalInput);
                nextMoveTime = Time.time + 0.2f; // Delay to prevent rapid switching
            }
        }
    }

    private void HandleFireButtonInput()
    {
        if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
        {
            var currentButton = ThreepasswordButtons[currentPosition];
            if (currentButton.BgPanel.activeSelf)
            {
                currentButton.OnClickThis();
                CheckClear();
            }
            isFireButtonPressed = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isFireButtonPressed = false;
        }
    }

    private void SelectThreeButton(int position)
    {
        if (lastSelectedPosition == position) return;

        if (lastSelectedPosition >= 0)
        {
            ThreepasswordButtons[lastSelectedPosition].HideBGThreePanel();
        }

        ThreepasswordButtons[position].ShowBGPanel();
        lastSelectedPosition = position;
    }

    private void MoveSelection(float input)
    {
        if (input > 0)
        {
            ShiftSlotRight();
        }
        else
        {
            ShiftSlotLeft();
        }
    }

    private void ShiftSlotRight()
    {
        currentPosition = (currentPosition + 1) % ThreepasswordButtons.Length;
        SelectThreeButton(currentPosition);
    }

    private void ShiftSlotLeft()
    {
        currentPosition = (currentPosition - 1 + ThreepasswordButtons.Length) % ThreepasswordButtons.Length;
        SelectThreeButton(currentPosition);
    }

    private bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (ThreepasswordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }
}
