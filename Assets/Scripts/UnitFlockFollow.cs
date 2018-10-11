using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFlockFollow : MonoBehaviour
{
	public GameObject pathGoal;
	private ArrayList pathGoals;
	public int pathIndex;
	public GameObject head;
	public float moveSpeed = 1;
	private Vector3 movement;
	private float t = 0;
	public GameObject radius;
	public string unitName;
	public UIHandler uiReceive;
	public Vector3 velocity;
	public float acceleration;
	public float goalRotatePower = .05f;
	public bool coneChecking = true;
	public float visionDegrees = 30;
	public bool collisionPredicting = true;
	public float collisionDistance = .6f;
	private float shortestTime = 100;

	void Start()
	{
		Physics.IgnoreLayerCollision(9, 0);
		Transform[] pathTemp = pathGoal.GetComponentsInChildren<Transform>();
		pathGoals = new ArrayList();
		for (int i = 0; i < pathTemp.Length; i++)
		{
			if (pathTemp[i].gameObject.tag == "Pathing")
			{
				pathGoals.Add(pathTemp[i].gameObject);
			}
		}
		pathIndex = 0;
	}

	void Update()
	{
		head.transform.position = transform.position + new Vector3(0, .3f, 0);
	}

	void FixedUpdate()
	{
		//rotation

		steer();
		head.transform.rotation = transform.rotation;

		//movement

		if (Vector3.Distance(((GameObject)pathGoals[pathIndex]).transform.position, transform.position) < .3f)
		{
			velocity = Vector3.Lerp(velocity, Vector3.zero, .1f);
			transform.position += velocity * Time.deltaTime;
		}
		else
		{
			velocity += transform.forward * acceleration * Time.deltaTime;
			velocity = Vector3.ClampMagnitude(velocity, moveSpeed);
			transform.position += velocity * Time.deltaTime;
		}

		//y axis

		transform.position = new Vector3(transform.position.x, 0 + .35f, transform.position.z);
	}

	void steer()
	{
		if (pathGoals.Count <= pathIndex)
			return;

		Quaternion goalRotation = Quaternion.LookRotation(((GameObject)pathGoals[pathIndex]).transform.position - transform.position);

		transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, goalRotatePower * Time.deltaTime);
	}

	public void pathContinue()  //call this when flock has reached current path goal
	{
		if (pathGoals.Count > pathIndex)
			pathIndex++;
	}

	bool inVision(GameObject unit)
	{
		if (Vector3.Angle((unit.transform.position - transform.position).normalized, transform.forward) < visionDegrees)
			return true;
		return false;
	}

	float predictCollision(GameObject unit)		//this should work, but I'm not sure. collision prediction is weird
	{
		Vector3 relativePos = unit.transform.position - transform.position;
		Vector3 relativeVel = unit.GetComponent<UnitFlockFollow>().velocity - velocity;
		float relativeSpeed = relativeVel.magnitude;
		float timeToCollide = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
		float collision = Vector3.Distance(unit.transform.position + timeToCollide * unit.GetComponent<UnitFlockFollow>().velocity, transform.position + timeToCollide * velocity);

		if (timeToCollide <= 0 || collision >= collisionDistance)
			return 0;

		float distance = relativePos.magnitude;

		return Vector3.SignedAngle(transform.forward, (unit.transform.position - transform.position).normalized, Vector3.up) / distance;	//should be positive if unit is on the left, negative if on the right
	}

	void radiusDisplay(float rad, Vector3 pos)
	{
		radius.transform.position = pos;
		radius.transform.localScale = new Vector3(rad, .05f, rad);
	}

	void OnTriggerEnter(Collider other)
	{
		if ((tag == "A" && other.tag == "B") || (tag == "B" && other.tag == "A"))
		{
			if (coneChecking)
			{
				if (inVision(other.gameObject))
				{
					//add to in vision list
				}
			}
			float turn = 0;
			if (collisionPredicting)
			{
				turn = predictCollision(other.gameObject);
			}

			//from here use vision list and turn to alter steering and movement to prevent collision
		}
	}
}
