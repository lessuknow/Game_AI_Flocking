using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public GameObject running_from;
    public Vector3 accel = new Vector3(0f, 0f, 0f),
        velocity = new Vector3(0f, 0f, 0f),
        target_velocity = new Vector3(0f, 0f, 0f);
    
    //max_prediction must be nonzero
    public float max_velocity = 10f, max_accel = 5f;
    public Vector3 target_position;
    public float current_velocity;
    public float strength, threshhold = 4.25f;

    private void Update()
    {
        Evade();
        Align();
        PostProcessing();

    }
    
    //Own algorithm for rotation; the book algorithm is strange.
    private void Align()
    {
        Vector3 dir = running_from.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private void Evade()
    {
        Vector3 direction = -running_from.transform.position + transform.position;
        float distance = direction.magnitude;
        float dist_clamp = Mathf.Clamp(distance, 0, threshhold);

        strength = max_velocity * (threshhold - dist_clamp) / threshhold;
        //print(strength);
        float cur_speed = velocity.magnitude;

        target_position = running_from.transform.position;


        Seek();
    }

    //Move towards the target.
    private void Seek()
    {
        //Move
        transform.position = transform.position -
            velocity * Time.deltaTime +
            accel * (Time.deltaTime) * (Time.deltaTime) / 2;
        
        //Increase the velocity based on acceleration.
        velocity = velocity + accel * Time.deltaTime;
        
        //Increase acceleration based on the distance between this and the target.
        accel = target_position - transform.position;
        //velocity *= strength;
    }

    //Handles processing everything after calling the functions, for ex. limiting the velocity and stuff.
    private void PostProcessing()
    {

        print(velocity);
        velocity *= strength;
        //Cap velocity and acceleration.
        if (velocity.magnitude > max_velocity)
        {
            velocity = velocity.normalized;
            velocity *= max_velocity;
        }
        if (accel.magnitude > max_accel)
        {
            accel = accel.normalized;
            accel *= max_accel;
        }

        //Print the velocity via public variable.
        current_velocity = velocity.magnitude;

    }

}
