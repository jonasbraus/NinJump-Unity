using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.contacts[0].normal.x < -0.8f || col.contacts[0].normal.x > 0.8f)
        {
            Player player;

            if ((player = col.gameObject.GetComponent<Player>()) == true)
            {
                player.TakeDamage(1);
            }
        }
    }
}
