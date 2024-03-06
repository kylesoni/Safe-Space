using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Prefabs
    public Transform pfItemWorld;
    public Transform star;
    public Transform battery;
    public Transform thruster;
    public Transform control_panel;
    public Transform spaceship;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "blocc")
        {
            
            Rigidbody2D itemRB = pfItemWorld.GetComponent<Rigidbody2D>(); // star stays on sky // spaceship prefabs are somehow automatically handled 
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
    public Sprite UpickaxeSprite;
    public Sprite healthPotionSprite;
    public Sprite jumpPotionSprite;
    public Sprite guardianPotionSprite;   
    public Sprite BombSprite;
    public Sprite keySprite;
    public Sprite lanternSprite;
    public Sprite starSprite;
    public Sprite SandSprite;
    public Sprite RedsandSprite;
    public Sprite WoodSprite;
    public Sprite GlassSprite;
    public Sprite RedstoneSprite;
    public Sprite RedwoodSprite;
    public Sprite BrickSprite;

    // Materials
    public Sprite RubySprite;

    // Blocks
    public Sprite dirtSprite;
    public Sprite stoneSprite;
    public Sprite ironSprite;
    public Sprite goldSprite;

    // Spaceship
    public Sprite batterySprite;
    public Sprite thrusterSprite;
    public Sprite controlPanelSprite;
    public Sprite spaceshipSprite;
}
