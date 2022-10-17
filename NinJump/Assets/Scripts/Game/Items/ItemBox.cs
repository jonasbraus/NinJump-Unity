using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private Animator animator;
    private float timeHit = 0;
    private float destroyDelay = 0.4f;
    [SerializeField] private GameObject itemPrefab;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (timeHit != 0)
        {
            if (Time.time - timeHit > destroyDelay)
            {
                if(itemPrefab != null)
                {
                    GameObject instance = Instantiate(itemPrefab);
                    instance.transform.position = transform.position;
                }
                Destroy(gameObject);
            }
        }
    }
    
    public void Hit()
    {
        animator.SetInteger("value", 1);
        timeHit = Time.time;
    }
}
