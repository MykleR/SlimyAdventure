using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFood : MonoBehaviour
{
    public float foodValue;
    public string playerTag = "Player";

    public bool eatable=true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == playerTag && eatable)
            collision.gameObject.GetComponent<Controller>().Eat(this.gameObject);
    }
}
