using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float gravity;
    public Vector2 velocity = new Vector2(0, 0);
    public bool grounded = false;
    private const float rayDownLength = 0.67f;
    private Animator animator;
    private bool jumpRequest = false;
    private bool doubleJumpAvailable = true;
    [SerializeField] private float jumpForce;
    private bool lastJumpPressed = false, jumpPressed = false;
    private Vector2 moveAnchor = new Vector2();
    private bool moveActionHold = false;
    private Vector2 moveDirection = new Vector2();
    [SerializeField] private float moveSpeed;
    private SpriteRenderer spriteRenderer;

    private bool doubleJumpPerformed = false;
    private float timeDoubleJump = 0;

    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite[] heartSprites;

    private int hp = 3;
    private int maxHp = 3;

    private float hitTime = -5;
    private const float inocentTime = .6f;

    private GameObject lastCheckPoint = null;
    [SerializeField] private AudioSource sourceWalk;
    [SerializeField] private AudioSource sourcePlayerDamage;
    [SerializeField] private AudioSource sourceJump;

    private int applesCount = 0;
    private int minApples = 40;

    [SerializeField] private TMP_Text applesText;

    private const float rayHorizontalLength = 0.5f;

    private void Start()
    {
        Application.targetFrameRate = 61;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        UpdateHearts();

        if (PlayerPrefs.HasKey("checkPointX"))
        {
            transform.position =
                new Vector3(PlayerPrefs.GetFloat("checkPointX") - 2, PlayerPrefs.GetFloat("checkPointY"), 5);
        }

        hitTime = Time.time;
        animator.SetInteger("value", 5);
        
        UpdateApples();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("checkPointX");
    }

    private void FixedUpdate()
    {
        //spieler fallen lassen, wenn dieser nicht auf dem boden steht
        if (!grounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        //überprüfen ob der spieler auf dem boden steht
        if (Physics2D.Raycast(transform.position, Vector2.down, rayDownLength))
        {
            grounded = true;
            doubleJumpAvailable = true;
            velocity.y = 0;
        }
        //spieler steht nicht mehr auf dem boden...
        else
        {
            grounded = false;
        }

        //wenn eine jump action ausgeführt wurde (leertaste oder touch auf der rechten bildschirmhälfte)
        if (jumpRequest)
        {
            jumpRequest = false;

            //spieler darf springen falls er auf dem boden steht
            if (grounded)
            {
                grounded = false;
                velocity.y = jumpForce;
            }
            //spieler darf springen falls ein doppelsprung verfügbar ist
            else if (doubleJumpAvailable)
            {
                timeDoubleJump = Time.time;
                velocity.y = jumpForce;
                doubleJumpPerformed = true;
                doubleJumpAvailable = false;
            }
        }

        //bewegung über move actions
        if (moveActionHold)
        {
            velocity.x = moveDirection.x * moveSpeed;
        }
        else
        {
            velocity.x = 0;
        }

        if (Physics2D.Raycast(transform.position, Vector2.left, rayHorizontalLength) && velocity.x < 0)
        {
            if(Physics2D.Raycast(transform.position, Vector2.left, rayHorizontalLength).collider.tag.Equals("Terrain"))
            {
                velocity.x = 0;
            }
        }
        
        if (Physics2D.Raycast(transform.position, Vector2.right, rayHorizontalLength) && velocity.x > 0)
        {
            if(Physics2D.Raycast(transform.position, Vector2.right, rayHorizontalLength).collider.tag.Equals("Terrain"))
            {
                velocity.x = 0;
            }
        }
        
        //bewege den spieler
        transform.Translate(velocity * Time.deltaTime);
    }
    

    private void Update()
    {
        bool leftTouch = false, rightTouch = false;

        //wenn der bildschirm berührt wurde...
        if (Input.touches.Length > 0)
        {
            //für jede berührung...
            foreach (Touch touch in Input.touches)
            {
                //wird die rechte hälfte des bildschirm berührt
                if (touch.position.x > Screen.width / 2)
                {
                    rightTouch = true;
                }


                //wird die linke hälfte des bildschirms berührt
                if (touch.position.x < Screen.width / 2)
                {
                    if (moveActionHold)
                    {
                        moveDirection = (touch.position - moveAnchor) / 100;
                        moveDirection.y = 0;

                        if (Mathf.Abs(moveDirection.x) < 0.2f)
                        {
                            moveDirection.x = 0;
                        }

                        if (Mathf.Abs(moveDirection.x) > 1)
                        {
                            moveDirection.Normalize();
                        }
                    }
                    else
                    {
                        moveAnchor = touch.position;
                    }

                    leftTouch = true;
                }
            }
        }

        jumpPressed = rightTouch;
        moveActionHold = leftTouch;

        if (Input.GetAxis("Horizontal") != 0)
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveActionHold = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpPressed = true;
        }


        //überprüfungen in welchen eingabe fällen der spieler springen darf
        if (jumpPressed && !lastJumpPressed || jumpPressed && grounded)
        {
            jumpRequest = true;
        }

        //wenn keine leben mehr übrig sind...
        if (hp <= 0)
        {
            if(lastCheckPoint != null)
            {
                PlayerPrefs.SetFloat("checkPointX", lastCheckPoint.transform.position.x);
                PlayerPrefs.SetFloat("checkPointY", lastCheckPoint.transform.position.y);
                PlayerPrefs.Save();
            }

            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        //animation
        if (velocity.x > 0)
        {
            spriteRenderer.flipX = false;
            if(grounded && !sourceWalk.isPlaying)
            {
                sourceWalk.Play();
            }
        }
        else if (velocity.x < 0)
        {
            spriteRenderer.flipX = true;
            if(grounded && !sourceWalk.isPlaying)
            {
                sourceWalk.Play();
            }
        }
        else
        {
            sourceWalk.Stop();
        }

        if (!grounded)
        {
            sourceWalk.Stop();
        }

        if (Time.time - hitTime > inocentTime)
        {
            if (!doubleJumpPerformed)
            {
                if (transform.parent == null)
                {
                    if (velocity.y < 0)
                    {
                        animator.SetInteger("value", 1);
                    }
                    else if (velocity.y == 0)
                    {
                        animator.SetInteger("value", 0);
                    }
                    else if (velocity.y > 0)
                    {
                        animator.SetInteger("value", 2);
                    }
                }
                else
                {
                    animator.SetInteger("value", 0);
                }
                
                if (grounded && velocity.x != 0)
                {
                    animator.SetInteger("value", 3);
                    animator.speed = Mathf.Abs(moveDirection.x) * 4;
                }
                else
                {
                    animator.speed = 1;
                }
            }
            else
            {
                if (velocity.y <= 0)
                {
                    doubleJumpPerformed = false;
                }

                animator.SetInteger("value", 4);
            }
        }

        //den aktuellen status der jumpaction speichern
        lastJumpPressed = jumpPressed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y < -0.2f)
        {
            ItemBox box;

            if ((box = col.gameObject.GetComponent<ItemBox>()) == true)
            {
                if(velocity.y > 0)
                {
                    box.Hit();
                    Destroy(col.collider);
                }
            }
            
            velocity.y = -0.5f;
        }

        if (col.gameObject.tag.Equals("Spikes"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        if (Time.time - hitTime > inocentTime)
        {
            sourcePlayerDamage.Play();
            hp -= amount;
            hitTime = Time.time;
            animator.SetInteger("value", 5);
            UpdateHearts();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hp; i++)
        {
            heartImages[i].sprite = heartSprites[0];
        }

        if (hp >= 0)
        {
            for (int i = hp; i < maxHp; i++)
            {
                heartImages[i].sprite = heartSprites[1];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckPoint checkPoint;
        if ((checkPoint = col.GetComponent<CheckPoint>()) == true)
        {
            checkPoint.Anim();
            lastCheckPoint = col.gameObject;
        }
        else if (col.tag.Equals("TransparentBlock"))
        {
            if (velocity.y > 0 && 
                Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(col.transform.position.x, 0)) < 0.325f)
            {
                col.GetComponent<TransparentBlock>().Trigger();
                velocity.y = -0.5f;
            }
        }
        else if (col.tag.Equals("Dead"))
        {
            TakeDamage(5);
        }
        else if (col.tag.Equals("Apple"))
        {
            col.GetComponent<Item>().Collect();
            applesCount += col.GetComponent<AppleValue>().value;
            UpdateApples();
        }
        else if (col.tag.Equals("Heal"))
        {
            col.GetComponent<Item>().Collect();
            hp += 1;
            if (hp > maxHp) hp = maxHp;
            UpdateHearts();
        }
    }

    public void BounceUp()
    {
        velocity.y = jumpForce / 2;
        sourceJump.Play();
    }

    private void UpdateApples()
    {
        applesText.text = applesCount + "/" + minApples;
    }
}