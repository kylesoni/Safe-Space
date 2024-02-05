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
    public GameObject[] lights; 
    public SpriteRenderer[] stars;

    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 0);
        }
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        activateLights = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalcTime();
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
        if (hours >= 17 && hours < 22)
        {
            ppv.weight += (float)1 / 300;
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, ppv.weight);
            }
            if (activateLights == false && hours >= 20)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].SetActive(true);
                }
                activateLights = true;
            }
        }


        if (hours >= 4 && hours < 9)
        {
            ppv.weight -= (float)1 / 300;
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, ppv.weight);
            }
            if (activateLights == true && hours >= 8)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].SetActive(false);
                }
                activateLights = false;
            }
        }
    }
}
