using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody theRB;
    public Animator animator;
    private Vector3 movement;
    private FlagManager flagManager;

    [SerializeField] private GameObject playersheet; // playersheetをインスペクターから設定可能にする

    // Start is called before the first frame update
    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスを取得
    }

    // Update is called once per frame
    void Update()
    {
        // CameraZoomObj フラグが true の場合、playersheet を非表示
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj))
        {
            playersheet.SetActive(false); // playersheetを非表示に設定
        }
        else
        {
            playersheet.SetActive(true); // playersheetを表示に設定
        }

        // itemboxフラグ、Camerazoomobjフラグ、または UICanvas フラグが true の場合、プレイヤーの移動とアニメーションを無効化
        if (flagManager.GetFlag(FlagManager.FlagType.itembox) ||
            flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) ||
            flagManager.GetFlag(FlagManager.FlagType.UICanvas) ||
            flagManager.GetFlag(FlagManager.FlagType.Textbox)||
            flagManager.GetFlag(FlagManager.FlagType.Nowanim)||
            flagManager.GetFlag(FlagManager.FlagType.Setanim) ||
            flagManager.GetFlag(FlagManager.FlagType.wipe) && !flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj)) // wipe時の特例処理

        {
            movement = Vector3.zero; // 移動量を0に設定
            animator.SetFloat("Speed", 0f); // アニメーションを停止
            return; // ここでUpdateの処理を終了
        }

        // 通常の移動処理
        movement.x = Input.GetAxisRaw("Horizontal Stick-L");
        movement.z = Input.GetAxisRaw("Vertical Stick-L");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // wipe中かつCameraZoomObjがまだfalseの場合は待機（移動を無効化するがreturnしない）
        if (flagManager.GetFlag(FlagManager.FlagType.wipe) &&
            !flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj))
        {
            return; // wipe中の特別処理（CameraZoomObjを待機）
        }


        // 通常の移動処理
        theRB.MovePosition(theRB.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
