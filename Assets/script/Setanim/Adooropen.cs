using UnityEngine;

public class Adooropen : MonoBehaviour
{
    public GameObject objWithAnimation; // アニメーションがついているオブジェクト
    private Animator animator;
    [SerializeField] private Collider Adoorcollider;
    private bool playerInsideCollider = false;

    [SerializeField] Item.Type useItem;

    private bool isDoorOpened = false; // ドアが開いたかどうかのフラグ

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
        if (playerInsideCollider && !isDoorOpened) // ドアが開いていない場合のみ処理
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
        if (isDoorOpened) return; // ドアが開いていれば処理をスキップ

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
                isDoorOpened = true; // ドアが開いたことを記録
                animator.SetTrigger("Adooropen"); // トリガーを設定してアニメーション再生
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Adooropen, true);
               
            }
        }
    }
}
