using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    public GameObject target;
    [Range(3,50)]
    public float minSpeed = 5.0f;
    [Range(0,2)]
    public float speedMultiplier=0.3f;
    public Vector2 offset;
    public bool followX=true;
    public bool followY=true;
    public GameObject holder;
    public float stopFollowY = -3.25f;

    Rigidbody2D targetRg;

    [HideInInspector]
    public float speed;

    private void Awake(){
        if (holder != null) DontDestroyOnLoad(holder);
        speed = minSpeed;
        targetRg = target.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target.transform.position.y < stopFollowY) followY = false;
        if (targetRg != null)
            speed = speedMultiplier * Mathf.Abs(targetRg.velocity.y) + minSpeed;
        if (speed < 2) speed = 2;
        
        float interpolation = speed * Time.deltaTime;
        Vector3 position = transform.position;
        if (followX)
            position.x = Mathf.Lerp(this.transform.position.x, target.transform.position.x, interpolation) + offset.x;
        if(followY)
            position.y = Mathf.Lerp(this.transform.position.y, target.transform.position.y, interpolation) + offset.y;

        this.transform.position = position;
    }
}
