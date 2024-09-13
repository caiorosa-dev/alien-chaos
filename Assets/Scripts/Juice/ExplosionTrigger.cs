using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public GameObject explosionLight;

    public delegate void OnEnd();
    public OnEnd onEnd;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("finished"))
        {
            onEnd();
        }
    }

    // Update is called once per frame
    public void TriggerExplosion()
    {
        spriteRenderer.enabled = true;
        explosionLight.SetActive(true);

        this.animator.SetBool("explode", true);
    }
}
