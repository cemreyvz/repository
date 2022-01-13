using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LifeTimerKariyer : MonoBehaviour
{
    [SerializeField]
    private Text textenergy;
    [SerializeField]
    private Text texttimer;
    [SerializeField]
    private int maxenergy;

    private int totalenergy = 0;
    private DateTime nextenergytime;
    private DateTime lastaddedtime;
    private int restoreduration = 25;
    public GameObject kariyerpanel;
    public GameObject panel2;
    private bool restoring = false;
    public string scene;
    public Color loadToColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        Load();
        StartCoroutine(restoreroutine());


    }

    public void useenergy()
    {
        if (totalenergy == 0)
            return;
        totalenergy--;
        updateenergy();

        if (!restoring)
        {
            if (totalenergy + 1 == maxenergy)
            {
                nextenergytime = addduration(DateTime.Now, restoreduration);
            }
            StartCoroutine(restoreroutine());
        }
    }

    public void panelview()
    {

        { 
            if (totalenergy == 0)
            {
                panel2.gameObject.SetActive(true);
            }
            else if (totalenergy >= 1)
            {
                 Initiate.Fade(scene, loadToColor, 1.0f);
            }
        }
    }
    

    private IEnumerator restoreroutine()
    {
        updatetimer();
        updateenergy();
        restoring = true;

        while (totalenergy < maxenergy)
        {
            DateTime currentTime = DateTime.Now;
            DateTime counter = nextenergytime;
            bool isadding = false;
            while (currentTime > counter)
            {
                if (totalenergy < maxenergy)
                {
                    isadding = true;
                    totalenergy++;
                    DateTime timetoadd = lastaddedtime > counter ? lastaddedtime : counter;
                    counter = addduration(timetoadd, restoreduration);
                }
                else
                    break;

            }
            if (isadding)
            {
                lastaddedtime = DateTime.Now;
                nextenergytime = counter;
            }
            updatetimer();
            updateenergy();
            Save();
            yield return null;

        }
        restoring = false;
    }
    private void updatetimer()
    {
        if (totalenergy >= maxenergy)
        {
            texttimer.text = "Full";
            return;
        }
        TimeSpan t = nextenergytime - DateTime.Now;
        string value = string.Format("{0}:{1:D2}:{2:D2}", (int)t.TotalHours, t.Minutes, t.Seconds);
        texttimer.text = value;
    }
    private void updateenergy()
    {
        textenergy.text = totalenergy.ToString();
    }
    private DateTime addduration(DateTime time, int duration)
    {
        return time.AddSeconds(duration);
    }
    private void Load()
    {
        totalenergy = PlayerPrefs.GetInt("totalenergy");
        nextenergytime = StringToDate(PlayerPrefs.GetString("nextenergytime"));
        lastaddedtime = StringToDate(PlayerPrefs.GetString("lastaddedtime"));
    }
    private void Save()
    {
        PlayerPrefs.SetInt("totalenergy", totalenergy);
        PlayerPrefs.SetString("nextenergytime", nextenergytime.ToString());
        PlayerPrefs.SetString("lastaddedtime", lastaddedtime.ToString());
    }
    private DateTime StringToDate(string date)
    {
        if (string.IsNullOrEmpty(date))
            return DateTime.Now;
        return DateTime.Parse(date);
    }
}
