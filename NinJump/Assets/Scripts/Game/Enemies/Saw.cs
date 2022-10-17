using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        Player player;

        if ((player = col.gameObject.GetComponent<Player>()) == true)
        {
            player.TakeDamage(1);
        }
    }
}
