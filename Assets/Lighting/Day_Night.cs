using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Day_Night : MonoBehaviour
{
    public Volume ppv; // this is the post processing volume

    public float tick;
    public float seconds;
    public int mins;
    public int hours;
    public int days = 1;

    public bool activateLights;
    public List<GameObject> lights = new List<GameObject>();
    public SpriteRenderer[] stars;

    public bool isNight = false;

    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 0);
        }
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
        activateLights = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalcTime();
        CheckNight();
        CheckLights();
    }

    public void CalcTime()
    {
        seconds += Time.fixedDeltaTime * tick;

        if (seconds >= 60)
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60)
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24)
        {
            hours = 0;
            days += 1;
        }

        if (seconds == 0)
        {
            ControlPPV();
        }
    }

    public void ControlPPV()
    {
        //ppv.weight = 0;
        if (hours >= 8 && hours < 13)
        {
            ppv.weight += (float)1 / 300;
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, ppv.weight);
            }
            if (activateLights == false && hours >= 9)
            {
                activateLights = true;
            }
        }


        if (hours >= 19)
        {
            ppv.weight -= (float)1 / 300;
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, ppv.weight);
            }
            if (activateLights == true && hours >= 23)
            {
                activateLights = false;
            }
        }
    }

    public void CheckNight()
    {
        if (hours >= 10)
        {
            isNight = true;
        }
        else
        {
            isNight = false;
        }
    }

    public void CheckLights()
    {
        if (activateLights)
        {
            foreach (GameObject light in lights)
            {
                if (light != null)
                {
                    light.SetActive(true);
                }
            }
        }
        else
        {
            foreach (GameObject light in lights)
            {
                if (light != null)
                {
                    light.SetActive(false);
                }
            }
        }
    }
}
