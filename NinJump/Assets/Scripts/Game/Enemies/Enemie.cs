using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 velocity = new Vector2();
    [SerializeField] private float rayOffset = 0.6f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float timeHit = 0;
    [SerializeField] private float dieDelay;

    private void Start()
    {
        velocity.x = -speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        transform.Translate(velocity * Time.deltaTime);

        if (timeHit != 0)
        {
            if (Time.time - timeHit > dieDelay)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col != null)
        {
            if (col.tag.Equals("Player"))
            {
                if(col.GetComponent<Player>().velocity.y < 0)
                {
                    col.GetComponent<Player>().BounceUp();
                    Hit();
                }
            }
            else
            {
                velocity.x *= -1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        else
        {
            velocity.x *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    public void Hit()
    {
        Destroy(GetComponent<Collider2D>());
        velocity.x = 0;
        animator.SetInteger("value", 1);
        timeHit = Time.time;
    }

    public void FlipX()
    {
        OnTriggerEnter2D(null);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        OnTriggerEnter2D(null);
    }
}
