using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public Transform pfItemWorld;
    public Transform star;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "blocc")
        {
            
            Rigidbody2D itemRB = pfItemWorld.GetComponent<Rigidbody2D>(); // star stays on sky
            // Rigidbody2D itemRB = collision.collider.GetComponent<Rigidbody2D>(); 
            if (itemRB != null)
            {
                itemRB.velocity = Vector3.zero;
                itemRB.gravityScale = 0f;
            }
        }
    }

    // Items
    public Sprite swordSprite;
    public Sprite UswordSprite;
    public Sprite pickaxeSprite;
    public Sprite healthPotionSprite;
    public Sprite jumpPotionSprite;
    public Sprite guardianPotionSprite;   
    public Sprite BombSprite;
    public Sprite keySprite;
    public Sprite lanternSprite;
    public Sprite starSprite;

    // Blocks
    public Sprite dirtSprite;
    public Sprite stoneSprite;
    public Sprite ironSprite;
    public Sprite goldSprite;
}
