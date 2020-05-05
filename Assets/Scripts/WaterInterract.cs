using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInterract : MonoBehaviour
{
    public Sound splash;
    public string playerTag="Player";
    public CameraShake cameraShake;
    public float timeBeforeReload = 0.3f;
    private bool reload = false;
    private float starttime;

    private void Update()
    {
        if (reload)
        {
            if(Time.time-starttime>timeBeforeReload)
                Scene_Manager.instance.LoadScene(Scene_Manager.instance.GetActiveIndex());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == playerTag)
        {
            AudioManager.instance.Play(splash);
            starttime = Time.time;
            collision.GetComponent<Controller>().onGroundParticles.Emit(20);
            reload = true;
            if (cameraShake != null) StartCoroutine(cameraShake.Shake(0.3f, 0.15f));
        }
    }
}
