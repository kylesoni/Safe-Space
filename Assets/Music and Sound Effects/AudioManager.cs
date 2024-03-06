using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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
    public AudioSource backgroundMusicUnder;

    public bool Day = true;
    public bool Under = false;
    private Day_Night daynight;
    private EnemySpawner spawner;

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
        daynight = FindObjectOfType<Day_Night>();
        spawner = FindObjectOfType<EnemySpawner>();
        Day = true;
        Under = false;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<EnemySpawner>();
        }
        else
        {
            if (Under != spawner.isUnderground)
            {
                Under = !Under;
                SwapUnderground();
            }
        }
        if (daynight == null)
        {
            daynight = FindObjectOfType<Day_Night>();
        }
        else
        {
            if (Day == daynight.isNight)
            {
                Day = !Day;
                if (!Under)
                {
                    SwapBackground();
                }
            }
        }   
    }

    public void Reload()
    {
        backgroundMusicDay.Play();
        backgroundMusicNight.Stop();
        backgroundMusicUnder.Stop();
        backgroundMusicDay.volume = 1;
        daynight = null;
        spawner = null;
        Under = false;
        Day = true;
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
        StopAllCoroutines();
        if (!Day)
        {
            StartCoroutine(FadeTrack(backgroundMusicDay, backgroundMusicNight));
        }
        else
        {
            StartCoroutine(FadeTrack(backgroundMusicNight, backgroundMusicDay));
        }
    }

    public void SwapUnderground()
    {
        StopAllCoroutines();
        if (Day)
        {
            if (Under)
            {
                StartCoroutine(FadeTrack(backgroundMusicDay, backgroundMusicUnder));
            }
            else
            {
                StartCoroutine(FadeTrack(backgroundMusicUnder, backgroundMusicDay));
            }
        }
        else
        {
            if (Under)
            {
                StartCoroutine(FadeTrack(backgroundMusicNight, backgroundMusicUnder));
            }
            else
            {
                StartCoroutine(FadeTrack(backgroundMusicUnder, backgroundMusicNight));
            }
        }
    }

    private IEnumerator FadeTrack(AudioSource Track1, AudioSource Track2)
    {
        float timeToFade = 5f;
        float timeElapsed = 0;
        Track2.Play();

        while (timeElapsed < timeToFade)
        {
            Track2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            Track1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
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
