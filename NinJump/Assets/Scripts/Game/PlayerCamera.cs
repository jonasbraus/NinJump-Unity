using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;

    //abstand zwischen kamera und spieler
    private float yOffset;
    private float defaultYOffset;

    //interpolationsfaktor zwischen spieler und kameraposition
    private float i = 0;
    
    private float speedFactor = 50;
    
    //ruckler im spiele vermeiden, aber kamera trotzdem flüssig bewegen
    private const float cameraAccuracy = 100f;

    private void Start()
    {
        //abstand kamera - spieler berechnen
        yOffset = transform.position.y - player.transform.position.y;
        defaultYOffset = yOffset;
    }

    private void FixedUpdate()
    {
        //kamera auf der x-Achse bewegen
        transform.Translate(player.transform.position.x - transform.position.x, 0, 0);
        //kamera werte runden, um bildfehler zu vermeiden
        transform.position = new Vector3(Mathf.Round(transform.position.x * cameraAccuracy) / cameraAccuracy,
            Mathf.Round(transform.position.y * cameraAccuracy) / cameraAccuracy, -10);
            
        //kamera in die niedrigste position snappen
        if (transform.position.y < 0.01f)
        {
            transform.position = new Vector3(transform.position.x, 0,
                -10);
        }
        //kamera schnell nach unten bewegen
        if (player.transform.position.y < -1.8f && transform.position.y > 0)
        {
            yOffset = defaultYOffset;
            speedFactor = 10f;
        }
        //normale kamera steuerung in höherem terrain
        else
        {
            yOffset = 0;
            speedFactor = 50;
        }
        
        //interpolieren zwischen player - kamera um weiches verfolgen des spielers zu ermöglichen
        if ((player.transform.position.y >= 2f && player.transform.position.y <= 10f ||
             transform.position.y > 0 && player.transform.position.y <= 10f) && transform.position.y <= 8.31f)
        {
            transform.position = Vector2.Lerp(transform.position, player.transform.position + Vector3.up * yOffset, i);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            i += Time.deltaTime / speedFactor;
        }
        //keine interpolation ist das niedrigste terrain-level erreicht
        else if (player.transform.position.y < 2f)
        {
            i = 0;
        }

        //fehlerhafte kamera werte ausgleichen (y)
        if (player.transform.position.y < 6.35f && transform.position.y > 8.31f)
        {
            transform.position = new Vector3(transform.position.x, 8.31f, -10);
        }
    }
}
