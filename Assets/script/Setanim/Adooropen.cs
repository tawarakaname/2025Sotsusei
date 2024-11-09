using UnityEngine;

public class PlayAnimationOnFlag : MonoBehaviour
{
    // FlagManagerを仮定。ここではシンプルなboolで説明。

    public GameObject objWithAnimation; // アニメーションがついているオブジェクト
    private Animator animator;
    private FlagManager flagManager;
    [SerializeField] private Collider Adoorcollider;
    private bool playerInsideCollider = false;

    [SerializeField] Item.Type useItem;

    void Start()
    {
        // Animatorコンポーネントを取得
        if (objWithAnimation != null)
        {
            animator = objWithAnimation.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (playerInsideCollider)
        {
            // PS4コントローラーの⚪︎ボタンは「Fire2」として認識されます
            if (Input.GetButtonDown("Fire2"))
            {
                OnClickAdoor();
            }
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

    public void OnClickAdoor()
    {
        // アイテムを使用できるか試み、使用できたら処理を行う
        if (Itembox.instance.TryUseItem(useItem))
        {
            Item selectedItem = Itembox.instance.GetSelectedItem();
            if (selectedItem != null)
            {
                // アイテムのタイプに基づいてフラグをtrueに設定
                FlagManager.Instance.SetFlagByType(selectedItem.type, true);
            }
            if (FlagManager.Instance.GetFlagByType(Item.Type.key1))
            {
                animator.SetTrigger("Adooropen"); // トリガーを設定してアニメーション再
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Adooropen, true);

            }

        }
    }
}
