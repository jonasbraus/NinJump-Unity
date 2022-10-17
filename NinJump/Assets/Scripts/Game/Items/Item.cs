using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Animator animator;
    private float timeCollected = 0f;
    private const float delayDestroy = 0.5f;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (timeCollected > 0)
        {
            if (Time.time - timeCollected > delayDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void Collect()
    {
        Destroy(GetComponent<CircleCollider2D>());
        animator.SetInteger("value", 1);
        timeCollected = Time.time;
    }
}
