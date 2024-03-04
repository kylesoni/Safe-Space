using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; 
    public AudioSource hitEnemySound;
    public AudioSource takeDamageSound;
    public AudioSource swingSwordSound;
    public AudioSource drinkPotionSound;
    public AudioSource itemPickupSound;
    public AudioSource miningSound;
    public AudioSource gameOverSound;
    public AudioSource placeBlockSound;
    public AudioSource jumpSound;
    public AudioSource placeStarSound;
    public AudioSource switchOnSound;

    public AudioSource backgroundMusic;

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
              
        DontDestroyOnLoad(gameObject);
    }

    
    public void HitEnemySound()
    {      
        hitEnemySound.Play();
    }

    public void TakeDamageSound()
    {
        takeDamageSound.Play();
    }

     public void DrinkPotionSound()
    {
        drinkPotionSound.Play(); 
    }

    public void SwingSwordSound()
    {
        swingSwordSound.Play();
    }
    public void ItemPickupSound()
    { 
        itemPickupSound.Play();
    } 

    public void PlayMiningSound()
    {
        if (miningSound != null && !miningSound.isPlaying)
        {
            miningSound.Play();
        }
    }
    
    public void StopMiningSound()
    {
        if (miningSound != null)
        {
            miningSound.Stop();
        }
    }

    public void GameOverSound()
    {
        gameOverSound.Play();
    }
    public void PlaceBlockSound()
    {
        placeBlockSound.Play();
    }
    public void JumpSound()
    {
        jumpSound.Play();
    }
    public void PlaceStarSound()
    {
        placeStarSound.Play();
    }

    public void SwitchOnSound()
    {
        switchOnSound.Play();
    }


    /*  public void ToggleBackgroundMusic()
      {
          if (backgroundMusic != null)
          {
              if (backgroundMusic.isPlaying)
              {
                  backgroundMusic.Pause();
              }
              else
              {
                  backgroundMusic.Play();
              }
          }
      }*/
}
