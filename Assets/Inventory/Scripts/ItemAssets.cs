using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public Transform pfItemWorld;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "blocc")
        {
            Rigidbody2D itemRB = pfItemWorld.GetComponent<Rigidbody2D>();
            if (itemRB != null)
            {
                itemRB.velocity = Vector3.zero;
                itemRB.gravityScale = 0f;
            }
        }
    }

    public Sprite swordSprite;
    public Sprite healthPotionSprite;
    public Sprite oreSprite;
    public Sprite coinSprite;
    public Sprite keySprite;
    // Blocks
    public Sprite dirtSprite;
    public Sprite stoneSprite;
}
