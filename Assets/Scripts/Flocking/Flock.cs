using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : Movable
{
    public List<GameObject> friends;
    public Vector3 seperation_accel = new Vector3(0f, 0f, 0f),
        cohesion_accel = new Vector3(0f, 0f, 0f),
        seperation_velocity = new Vector3(0f, 0f, 0f),
        cohesion_velocity = new Vector3(0f, 0f, 0f),
        move_velocity = new Vector3();
    
    //max_prediction must be nonzero
    public float max_velocity = 10f, max_accel = 5f;
    public Vector3 target_position;
    public float current_velocity;
    public float strength, threshhold = 4.25f, target_radius = 0.25f, slow_radius = 0.75f, time_to_target = 2.5f;
    private Vector3 flock_center, flock_velocity, seperation_velocity_tmp = Vector3.zero;

    private void Update()
    {
        GetFlockCenter();
        GetFlockVelocity();
        Seperation();
        Cohesion();
        Move();
    }

    private void GetFlockCenter()
    {
        flock_center = Vector3.zero;
        for(int i=0;i<friends.Count;i++)
        {
            flock_center += friends[i].transform.position;
        }
        if(flock_center != Vector3.zero)
            flock_center /= friends.Count;
    }

    private void GetFlockVelocity()
    {
        flock_velocity = Vector3.zero;
        for (int i = 0; i < friends.Count; i++)
        {
            flock_velocity += friends[i].GetComponent<Movable>().velocity;
        }
        if (flock_velocity != Vector3.zero)
            flock_velocity /= friends.Count;
    }

    private void Seperation()
    {
        seperation_velocity_tmp = Vector3.zero;
        for (int i=0;i < friends.Count; i++)
        {
            Seperation(i);
        }
        seperation_velocity = seperation_velocity_tmp;
        if(seperation_velocity!=Vector3.zero)
            seperation_velocity /= friends.Count;
        
    }

    private void Seperation(int pos)
    {
        Vector3 direction = -friends[pos].transform.position + transform.position;
        float distance = direction.magnitude;
       // distance = Mathf.Clamp(distance, 0, threshhold);
        strength = max_velocity * (threshhold - distance) / threshhold;
        target_position = friends[pos].transform.position;
     
        //Increase the velocity based on acceleration.
        seperation_velocity_tmp += strength * direction;

        

    }

    //Move towards the target.
    private void Move()
    {

        //velocity = seperation_velocity + running_from.GetComponent<Flock>().velocity + cohesion_velocity;
        //end_velocity /= 3;

        velocity = seperation_velocity + cohesion_velocity + flock_velocity;
        //velocity = cohesion_velocity;// + flock_velocity;
        velocity /= 3;

        //Vector3 end_velocity = cohesion_velocity;
        //Cap velocity and acceleration.
        if (velocity.magnitude > max_velocity)
        {
            velocity = velocity.normalized;
            velocity *= max_velocity;
        }

        print(transform.name+" "+ seperation_velocity + " " + cohesion_velocity + " " + flock_velocity+" "+velocity);
         
        //Move
        transform.position = transform.position +
            velocity * Time.deltaTime;

        move_velocity = velocity;
        current_velocity = velocity.magnitude;
    }

    //Move towards the target.
    private void Cohesion()
    {
        if(friends.Count == 0)
        {
            cohesion_velocity = Vector3.zero;
            return;
        }
        Vector3 direction = -flock_center + transform.position;
        float target_speed;
        float distance = direction.magnitude;
        if (distance < target_radius)
            target_speed = 0;

        else if (distance > slow_radius)
            target_speed = max_velocity;

        else
            target_speed = max_velocity * distance / slow_radius;
        
        cohesion_velocity = direction;
        cohesion_velocity = cohesion_velocity.normalized;
        cohesion_velocity *= -target_speed;
        
    }

    
}
