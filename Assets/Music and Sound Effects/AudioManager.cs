using System.Collections;
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
    public AudioSource gameWinSound;

    public AudioSource backgroundMusicDay;
    public AudioSource backgroundMusicNight;

    public bool Day = true;
    public Day_Night daynight;

    private void Awake()
    {
        backgroundMusicDay.Play();
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

    private void Start()
    {
        daynight = FindObjectOfType<Day_Night>();
    }

    private void Update()
    {
        if (Day == daynight.isNight)
        {
            SwapBackground();
        }
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

    public void GameWinSound()
    {
        gameWinSound.Play();
    }

    public void SwapBackground()
    {
        Day = !Day;
        StopAllCoroutines();
        StartCoroutine(FadeTrack());
    }

    private IEnumerator FadeTrack()
    {
        Debug.Log("Hi");
        float timeToFade = 5f;
        float timeElapsed = 0;

        if (!Day)
        {
            backgroundMusicNight.Play();

            while (timeElapsed < timeToFade)
            {
                backgroundMusicNight.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                backgroundMusicDay.volume = Mathf.Lerp(0.2f, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            backgroundMusicDay.Play();

            while (timeElapsed < timeToFade)
            {
                backgroundMusicNight.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                backgroundMusicDay.volume = Mathf.Lerp(0, 0.2f, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
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
