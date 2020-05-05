using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    Vector3 startPos;

    public void Stop()
    {
        transform.localPosition = startPos;
        StopAllCoroutines();
        print("Stopped all Coroutines: " + Time.time);
    }

	public IEnumerator Shake(float duration, float magnitude)
    {

        Vector3 originalPos = transform.localPosition;
        startPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = originalPos;
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public IEnumerator Move(float duration,Vector3 direction,float speed)
    {
        Vector3 originalPos = transform.localPosition;
        startPos = transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float step = speed * Time.deltaTime;
            print(direction * 2);
            transform.position = Vector3.MoveTowards(transform.position, transform.position+direction * 2, step);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

}
