using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentBlock : MonoBehaviour
{
    public void Trigger()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<BoxCollider2D>().isTrigger = false;
        gameObject.layer = 0;
    }
}
