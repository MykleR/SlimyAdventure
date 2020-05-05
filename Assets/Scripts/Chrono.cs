using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chrono : MonoBehaviour
{

    private float time;
    private float startTime;
    private bool start;
    private bool pause;
    private string milliseconds;
    private string seconds;
    private string txt;

    public bool startOnAwake;
    public TextMeshProUGUI txtElement;
    public int lastLvlIndex = 2;

    private void Awake()
    {
        start = startOnAwake;
        if (start) startTime = Time.time;
        txtElement = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(start && !pause)
            txtElement.text = txt;
    }

    private void FixedUpdate()
    {
        if (Scene_Manager.instance.GetActiveIndex() == lastLvlIndex) pause = true;
        if (pause) return;
        time = Time.time-startTime;
        string seconds = time.ToString("f0");
        string milliseconds = ((time % 60) % 1 *100).ToString("f0");
        txt = (seconds + ":" + milliseconds);

    }

    public void trigger()
    {
        start = true;
        startTime = Time.time;
    }
}
