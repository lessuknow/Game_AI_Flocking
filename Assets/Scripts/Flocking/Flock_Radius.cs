using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock_Radius : MonoBehaviour {

    public Flock flk;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Movable>() && other.gameObject.name != "Unit Body")
        {
            // print(Vector3.Angle(transform.forward, transform.position - other.transform.position));
            if (other.GetComponent<Movable>() && other.gameObject.name != "Unit Body")
                flk.friends.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Movable>() && other.gameObject.name != "Unit Body")
            if(flk.friends.Count > 1)
                flk.friends.Remove(other.gameObject);
    }
}
