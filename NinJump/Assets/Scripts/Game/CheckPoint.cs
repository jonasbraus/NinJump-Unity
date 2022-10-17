using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Anim()
    {
        animator.SetInteger("value", 1);
    }
}
