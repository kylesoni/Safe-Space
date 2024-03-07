using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Star : MonoBehaviour
{
    private Day_Night dayNightCycle;
    private Light2D starLight;
    void Start()
    {
        dayNightCycle = FindObjectOfType<Day_Night>();
        starLight = GetComponent<Light2D>();
    }

    void Update()
    {
        if (dayNightCycle.hours >= 11 && dayNightCycle.hours <= 19)
        {
            starLight.intensity = 10f;
        }
        else
        {
            starLight.intensity = 3f;
        }
    }
}
