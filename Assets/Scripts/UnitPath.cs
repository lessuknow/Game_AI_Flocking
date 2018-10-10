using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPath : MonoBehaviour
{
	public GameObject pathGoal;
	private ArrayList pathGoals;
	private int pathIndex;
	private int lookIndex;
	public GameObject head;
	public float moveSpeed = 1;
	private Vector3 movement;
	private float t = 0;
	public GameObject radius;
	public GameObject lineA;
	public GameObject lineB;
	public GameObject lineC;
	public GameObject lineD;
	public Material greenMat;
	public Material redMat;
	public string unitName;
	public UIHandler uiReceive;
	public Vector3 velocity;
	public float acceleration;
	public float rayDistance = 1f;
	public float goalRotatePower = .05f;
	public float avoidRotatePower = .5f;

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
		lookIndex = 0;
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

		velocity += transform.forward * acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude(velocity, moveSpeed);
		transform.position += velocity * Time.deltaTime;

		//y axis

		transform.position = new Vector3(transform.position.x, 0 + .35f, transform.position.z);

		//pathing logic

		if (pathGoals.Count > pathIndex && Vector3.Distance(((GameObject)pathGoals[pathIndex]).transform.position, transform.position) < rayDistance)
			pathIndex++;
	}

	void steer()
	{
		if (pathGoals.Count <= pathIndex)
			return;

		Quaternion goalRotation = Quaternion.LookRotation(((GameObject)pathGoals[pathIndex]).transform.position - transform.position);
		float rr = rayResult();
		Quaternion avoidRotation = transform.rotation * Quaternion.AngleAxis(rr * 5, Vector3.up);

		transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, goalRotatePower * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, avoidRotation, avoidRotatePower * Time.deltaTime);
	}

	float rayResult()	//returns negative for turn power left, positive for turn power right, zero for no change of course
	{
		LayerMask selfMask = 1 << 0;

		Ray rayA = new Ray(transform.position - transform.right * .25f, transform.forward);
		RaycastHit hitA = new RaycastHit();
		bool a = Physics.Raycast(rayA, out hitA, rayDistance, selfMask);
		rayDisplay(lineA, rayA.origin, rayA.origin + rayA.direction * rayDistance, a);

		Ray rayB = new Ray(transform.position + transform.right * .25f, transform.forward);
		RaycastHit hitB = new RaycastHit();
		bool b = Physics.Raycast(rayB, out hitB, rayDistance, selfMask);
		rayDisplay(lineB, rayB.origin, rayB.origin + rayB.direction * rayDistance, b);

		Ray rayC = new Ray(transform.position, (transform.forward - transform.right * .15f));
		RaycastHit hitC = new RaycastHit();
		bool c = Physics.Raycast(rayC, out hitC, rayDistance, selfMask);
		rayDisplay(lineC, rayC.origin, rayC.origin + rayC.direction * rayDistance, c);

		Ray rayD = new Ray(transform.position, (transform.forward + transform.right * .15f));
		RaycastHit hitD = new RaycastHit();
		bool d = Physics.Raycast(rayD, out hitD, rayDistance, selfMask);
		rayDisplay(lineD, rayD.origin, rayD.origin + rayD.direction * rayDistance, d);

		//decision logic

		if (!a && !b && !c && !d)
			return 0;

		float distanceA = Vector3.Distance(hitA.point, rayA.origin);
		float distanceB = Vector3.Distance(hitB.point, rayB.origin);
		float distanceC = Vector3.Distance(hitC.point, rayC.origin);
		float distanceD = Vector3.Distance(hitD.point, rayD.origin);

		if (c && d)
		{
			if (distanceC < distanceD)
				return rayDistance / distanceC;
			else
				return rayDistance / -distanceD;
		}
		if(c)
			return rayDistance / distanceC;
		if (d)
			return rayDistance / -distanceD;

		if (a && b)
		{
			if (distanceA < distanceB)
				return rayDistance / distanceA;
			else
				return rayDistance / -distanceB;
		}
		if (a)
			return rayDistance / distanceA;
		if (b)
			return rayDistance / -distanceB;

		return 0;
	}

	void rayDisplay(GameObject line, Vector3 origin, Vector3 end, bool didHit)
	{
		line.transform.position = (origin + end) / 2;
		line.transform.rotation = Quaternion.LookRotation(end - origin);
		line.transform.localScale = new Vector3(.05f, .05f, Vector3.Distance(origin, end));
		if (!didHit)
			line.GetComponent<MeshRenderer>().material = greenMat;
		else
			line.GetComponent<MeshRenderer>().material = redMat;
	}

	void radiusDisplay(float rad, Vector3 pos)
	{
		radius.transform.position = pos;
		radius.transform.localScale = new Vector3(rad, .05f, rad);
	}

	void chaseFleeSet()
	{
		/*this section has raycast lines in it
		if (Vector3.Distance(fleeGoal.transform.position, transform.position) < 20 && Vector3.Distance(fleeGoal.transform.position, transform.position) > 16)
		{
			fleeDisplay.transform.position = transform.position + (fleeGoal.transform.position - transform.position).normalized * 16;
		}
		else
		{
			fleeDisplay.transform.position = Vector3.zero;
		}
		if (Vector3.Distance(chaseGoal.transform.position, transform.position) < 25 && Vector3.Distance(chaseGoal.transform.position, transform.position) > 20)
		{
			chaseDisplay.transform.position = transform.position + (chaseGoal.transform.position - transform.position).normalized * 20;
		}
		else
		{
			chaseDisplay.transform.position = Vector3.zero;
		}
		if (Vector3.Distance(chaseGoal.transform.position, transform.position) < 25)
		{
			LayerMask groundMask = 1 << 9;
			Ray ray;
			RaycastHit hit;
			ray = new Ray(transform.position, (chaseGoal.transform.position - transform.position).normalized);
			hit = new RaycastHit();
			if (!Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, chaseGoal.transform.position), groundMask))
			{
				seeLine.transform.position = (transform.position + chaseGoal.transform.position) / 2;
				seeLine.transform.rotation = Quaternion.LookRotation((transform.position - chaseGoal.transform.position).normalized);
				seeLine.transform.localScale = new Vector3(.1f, .1f, Vector3.Distance(transform.position, chaseGoal.transform.position));
				blindLine.transform.position = Vector3.zero;
			}
			else
			{
				blindLine.transform.position = (transform.position + chaseGoal.transform.position) / 2;
				blindLine.transform.rotation = Quaternion.LookRotation((transform.position - chaseGoal.transform.position).normalized);
				blindLine.transform.localScale = new Vector3(.1f, .1f, Vector3.Distance(transform.position, chaseGoal.transform.position));
				seeLine.transform.position = Vector3.zero;
			}
		}
		else
		{
			blindLine.transform.position = Vector3.zero;
			seeLine.transform.position = Vector3.zero;
		}
		*/
	}
}
