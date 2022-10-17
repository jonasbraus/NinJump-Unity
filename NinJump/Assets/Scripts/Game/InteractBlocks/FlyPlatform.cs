using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlatform : MonoBehaviour
{
    private Vector2 defaultPos;
    [SerializeField] private Vector2 targetPoint;
    [SerializeField] private float speed;

    private bool move = false;

    private float i = 0;

    private Animator animator;

    [SerializeField] private bool needToStepOn = false;
    
    private int direction = 1;
    
    private void Start()
    {
        defaultPos = transform.localPosition;
        animator = GetComponent<Animator>();
        
        if(!needToStepOn)
        {
            animator.SetInteger("value", 1);
        }
    }

    private void FixedUpdate()
    {
        if(needToStepOn)
        {
            if (move)
            {
                transform.position = Vector2.Lerp(defaultPos, targetPoint, i);
                i += Time.deltaTime * speed;
            }
            else
            {
                transform.position = Vector2.Lerp(defaultPos, targetPoint, i);
                i -= Time.deltaTime * speed;
            }

            if (i > 1) i = 1;
            if (i < 0) i = 0;
            
            if (i == 1 || i == 0)
            {
                animator.SetInteger("value", 0);
            }
        }
        else
        {
            transform.position = Vector2.Lerp(defaultPos, targetPoint, i);
            i += Time.deltaTime * speed * direction;

            if (i > 1 || i < 0)
            {
                direction *= -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(needToStepOn)
        {
            move = true;
            animator.SetInteger("value", 1);
        }
        
        if(col.tag.Equals("Player"))
        {
            col.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(needToStepOn)
        {
            move = false;
            animator.SetInteger("value", 1);
        }
        
        if(other.tag.Equals("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
