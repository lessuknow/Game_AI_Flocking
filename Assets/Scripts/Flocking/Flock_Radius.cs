using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock_Radius : MonoBehaviour {

    public Flock flk;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name != "boundbox")
            flk.friends.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != "boundbox")
            flk.friends.Remove(other.gameObject);
    }
}
