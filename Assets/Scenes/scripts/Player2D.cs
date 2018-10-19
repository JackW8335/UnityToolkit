using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum playerState { idle, falling, Grounded, dead };
public class Player2D : MonoBehaviour {
    public float walkingSpeed, fallForce;

    playerState state;

    // Use this for initialization
    void Start()
    {
        walkingSpeed = 3.0f;
        fallForce = 0.5f;
        state = playerState.idle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        switch (state)
        {
            case playerState.Grounded:
                {
                    Walking();
                    break;
                }
            case playerState.falling:
                {
                    Falling();
                    break;
                }
            case playerState.dead:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void Walking()
    {
        Vector3 walk = Vector3.zero;
        float h = Input.GetAxis("Horizontal");
        walk = new Vector3(h, 0, 0);
        this.transform.position += walk * walkingSpeed * Time.deltaTime;
    }

    void Falling()
    {
        float h = Input.GetAxis("Horizontal");
        Vector3 fall = new Vector3(h, 0, 0);
        this.transform.position += fall * fallForce * Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            state = playerState.Grounded;
        }

    }
}
