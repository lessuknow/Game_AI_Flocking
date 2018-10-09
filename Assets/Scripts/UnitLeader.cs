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
	public Camera cam;
	public GameObject goal;

	void Start()
	{

	}

	void Update()
	{
		LayerMask mask = -1;
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane)), out hit, 100, mask))
		{
			goal.transform.position = hit.point;
		}
		else
		{
			goal.transform.position = new Vector3(50, -10, 50);
		}

		//set y axis height

		transform.position = new Vector3(transform.position.x, .35f, transform.position.z);

		//set head position

		head.transform.position = transform.position + new Vector3(0, .3f, 0);
	}

	void displayInfo()
	{

	}
}
