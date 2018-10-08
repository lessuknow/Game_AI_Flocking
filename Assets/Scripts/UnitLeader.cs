using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLeader : MonoBehaviour
{
	public GameObject head;
	public float moveSpeed = 1;
	private float t = 0;
	public string unitName;
	public UIHandler uiReceive;

	void Start()
	{

	}

	void Update()
	{
		//set y axis height,

		transform.position = new Vector3(transform.position.x, Mathf.PerlinNoise(transform.position.x * .075f + 1, transform.position.z * .075f + 1) * 5 + .25f, transform.position.z);

		//set head position

		head.transform.position = transform.position + new Vector3(0, .3f, 0);
	}

	void displayInfo()
	{

	}
}
