using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAutoDestroy : MonoBehaviour
{
    public float lifeTime = 2f;
    public string[] allowedTags;

    private float timeElapsed = 0f;

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (System.Array.Exists(allowedTags, element => element == collision.gameObject.tag))
        {
            return;
        }

        Destroy(this.gameObject);
    }
}
