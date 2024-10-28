using UnityEngine;

public class IllustPassword : MonoBehaviour
{
    private const float MoveCooldown = 0.2f;
    [SerializeField] private int[] correctNumbers;
    [SerializeField] private IllustPasswordButton[] IllustpasswordButtons;
    [SerializeField] private GameObject capsuleD;


    private int currentPosition = 0;
    private float nextMoveTime = 0f;
    private bool isFireButtonPressed = false;
    private FlagManager flagManager;
    private int lastSelectedPosition = -1;

    private void Start()
    {
        flagManager = FlagManager.Instance;
        SelectIllustButton(currentPosition);
        
    
     }

    public void CheckClear()
    {
        if (IsClear())
        {
            
            FlagManager.Instance.SetFlag(FlagManager.FlagType.IllustPasswordclear, true);
            Debug.Log("IllustPasswordclearFlagON");
            capsuleD.SetActive(true);
        }
    }

    private void Update()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.BoxACamera))
            return;

        HandleHorizontalInput();
        HandleFireButtonInput();
    }

    private void HandleHorizontalInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BoxACamera))
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal Stick-L");

            if (Time.time >= nextMoveTime && horizontalInput != 0)
            {
                MoveSelection(horizontalInput);
                nextMoveTime = Time.time + MoveCooldown;
            }
        }
    }

    private void HandleFireButtonInput()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BoxACamera))
        {
            if (Input.GetButtonDown("Fire2") && !isFireButtonPressed)
            {
               

                var currentButton = IllustpasswordButtons[currentPosition];
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
    }

    private void SelectIllustButton(int position)
    {
        if (lastSelectedPosition == position) return;
        if (lastSelectedPosition >= 0)
            IllustpasswordButtons[lastSelectedPosition].HideBGIllustPanel();

        IllustpasswordButtons[position].ShowBGPanel();
        lastSelectedPosition = position;
    }

    private void MoveSelection(float input)
    {
        currentPosition = (currentPosition + (input > 0 ? 1 : -1) + IllustpasswordButtons.Length) % IllustpasswordButtons.Length;
        SelectIllustButton(currentPosition);
    }

    private bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (IllustpasswordButtons[i].number != correctNumbers[i])
                return false;
        }
        return true;
    }
}
