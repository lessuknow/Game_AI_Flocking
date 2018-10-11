using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Flock_Head : MonoBehaviour {

    public Vector3 velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += velocity * Time.deltaTime;
	}
}
