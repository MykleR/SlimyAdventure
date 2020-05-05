using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Camera2D cam;
    public Chrono chrono;
    public float lerpDuration = 1.0f;

    private bool played;
    private bool finished;
    private float lerpStart;
    private CanvasGroup grp;

    private void Awake()
    {

        grp = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        if (!played) {
            cam.followY = false;
            Controller.instance.rg.simulated = false;
        }
    }

    private void Update()
    {
        if (played && !finished)
        {
            Controller.instance.rg.simulated = true;
            var progress = Time.time - lerpStart;
            grp.alpha = Mathf.Lerp(1.0f, 0.0f, progress / lerpDuration);
            if (lerpDuration < progress)
            {
                cam.followY = true;
                chrono.trigger();
                finished = true;
            }
        }
        else if (!played)
        {
            cam.followY = false;
            Controller.instance.rg.simulated = false;
        }
    }

    public void PlayGame()
    {
        lerpStart = Time.time;
        played = true;
        grp.interactable = false;
        Controller.instance.rg.simulated = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
