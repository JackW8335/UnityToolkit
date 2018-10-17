using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum playerState { idle, falling, Grounded, climbing, overhangClimbing, dead };

public class playerBehaviour : MonoBehaviour
{
    public float walkingSpeed, climbingSpeed, fallForce;
    public string DebugState;
    public bool FacingRight;
    public bool climbing = false;
    public int health;

    public float DecreaseRate;
    public float IncreaseRate;

    private int collisionCount = 0;

    playerState state;
    private Rigidbody2D Rbody;
    private SpriteRenderer sprite;
    private Animator anim;
    private float damage;
    private float stamina;
    public bool canAttack;
    private string weakSpotName;


    public float fireDelta = 0.3F;
    private float time = 0.0F;
    private float nextFire = 0.3F;


    // Use this for initialization
    void Start()
    {

        walkingSpeed = 3.0f;
        climbingSpeed = 2.0f;
        fallForce = 0.5f;

        damage = 35;
        canAttack = false;

        weakSpotName = "";


        stamina = 100f;
        FacingRight = true;

        state = playerState.idle;//change to enum
        DebugState = "idle";
        Rbody = this.GetComponent<Rigidbody2D>();
        sprite = this.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        time = time + Time.deltaTime;


        if (health <= 0.0f)
        {
            state = playerState.dead;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        switch (state)
        {
            case playerState.Grounded:
                {
                    //setNewSprite("walking1");
                    Walking();
                    DebugState = "ground";
                    break;
                }
            case playerState.falling:
                {
                    // setNewSprite("walking1");
                    anim.SetBool("Climbing", false);
                    DebugState = "falling";
                    Falling();
                    //RecoverGrip(IncreaseRate);
                    climbing = false;
                    break;
                }
            case playerState.dead:
                {
                    break;
                }
            // add faling movement same as wallking but no annimation
            default:
                {
                    break;
                }
        }
    }



    void setNewSprite(string spriteName)
    {
        if (sprite.sprite.name != spriteName)
        {
            sprite.sprite = Resources.Load(spriteName, typeof(Sprite)) as Sprite;
        }
    }

    void Walking()
    {
        Rbody.gravityScale = 1;
        anim.SetBool("Climbing", false);
        Vector3 walk = Vector3.zero;
        float h = Input.GetAxis("Horizontal");
        walk = new Vector3(h, 0, 0);
        this.transform.position += walk * walkingSpeed * Time.deltaTime;

        if (h > 0 && !FacingRight)
        {
            Flip();
        }
        else if (h < 0 && FacingRight)
        {
            Flip();
        }

        if (h != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

    }

    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }




    void Falling()
    {
        Rbody.gravityScale = 1;
        float h = Input.GetAxis("Horizontal");
        Vector3 fall = new Vector3(h, 0, 0);
        this.transform.position += fall * fallForce * Time.deltaTime;

        if (h > 0 && !FacingRight)
        {
            Flip();
        }
        else if (h < 0 && FacingRight)
        {
            Flip();
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ground")
        {
            state = playerState.Grounded;
        }

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "ground")
        {
            if (!climbing)
            {
                state = playerState.Grounded;
            }
        }
    }
}

