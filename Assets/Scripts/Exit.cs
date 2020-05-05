using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject center;
    public Sound exitSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            Controller player = collision.GetComponent<Controller>();
            if (player != null)
            {
                player.Finish(center);
                AudioManager.instance.Play(exitSound);
            }
        }
    }
}
