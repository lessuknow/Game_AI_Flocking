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
	public GameObject radiusDisplay;
	public GameObject seeLine;
	public GameObject blindLine;
	public string unitName;
	public UIHandler uiReceive;
	public Vector3 velocity;
	public float acceleration;

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
		//movement

		velocity += (((GameObject)pathGoals[pathIndex]).transform.position - transform.position) * acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude(velocity, moveSpeed);
		transform.position += velocity * Time.deltaTime;

		//y axis

		transform.position = new Vector3(transform.position.x, 0 + .35f, transform.position.z);
	}

	void displayInfo()
	{

	}

	void radiusSet(float rad, Vector3 pos)
	{
		radiusDisplay.transform.position = pos;
		radiusDisplay.transform.localScale = new Vector3(rad, .05f, rad);
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

	void resetDisplay()
	{
		radiusDisplay.transform.position = Vector3.zero;
		radiusDisplay.transform.localScale = new Vector3(1, .05f, 1);
		blindLine.transform.position = Vector3.zero;
		blindLine.transform.localScale = Vector3.zero;
		seeLine.transform.position = Vector3.zero;
		seeLine.transform.localScale = Vector3.zero;
	}
}
