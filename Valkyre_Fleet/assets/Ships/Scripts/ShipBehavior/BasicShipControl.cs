using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicShipControl : MonoBehaviour {

	private Vector3 _desitination;

	private bool _inMoveState;

	private int _squadronNumber;

	private GameObject _target;

	public ShipTag ShipTransponder;

	private bool _isDestroyed;

	public float Health = 100;

	public int InitialCrew = 100; // use this as defensive ratio.

	private float _initialHealth;

	private int _remainingCrew;

	private List<Vector3> _waypoints;

	public int RemainingCrew
	{
		get
		{
			return _remainingCrew;
		}
	}

	// Use this for initialization
	void Start()
	{
		_desitination = transform.position;
		_inMoveState = false;
		_isDestroyed = false;
		_squadronNumber = 0;
		_remainingCrew = InitialCrew;
		_waypoints = new List<Vector3>();
		_initialHealth = Health;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if (Health <= 0)
		{
			_isDestroyed = true;
		}

		if(_inMoveState)
		{
			if(_waypoints.Count > 0)
			{
				ApproachWaypoints();

			}
			else
			{
				ApproachDestination();
			}
		}

		if(_isDestroyed)
		{
			Destroy(this.gameObject);
		}
	}

	private void ApproachDestination()
	{
		Quaternion rotateTo = Quaternion.LookRotation(_desitination - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, 0.05f);

		float distanceToDestination = Vector3.Distance(transform.position, _desitination);

		if (distanceToDestination > 1f)
		{
			
			RVOAgent agent = GetComponent<RVOAgent>();
			if (agent != null) {
				agent.preferedVelocity = transform.forward * 2f;

				transform.position += agent.velocity * Time.deltaTime;
				agent.position = transform.position;
			} else {
				//transform.GetComponent<Rigidbody>().AddForce (transform.forward * .1f, ForceMode.Impulse);
				transform.Translate(Vector3.forward * .05f, Space.Self);
			}
		}
		else
		{
			//transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	private void ApproachWaypoints()
	{
		var currentQueuedPoint = _waypoints[0];

		Quaternion rotateTo = Quaternion.LookRotation(currentQueuedPoint - transform.position, Vector3.up);
		float distanceToDestination = Vector3.Distance(transform.position, currentQueuedPoint);

		transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, 0.05f);

		if (distanceToDestination > 1f)
		{
			transform.Translate(Vector3.forward * .05f, Space.Self);
		}
		else
		{
			_desitination = currentQueuedPoint;
			_waypoints.RemoveAt(0);
		}
	}


	public void SetDestinations(Vector3 destination, int squadNum)
	{
		float spacing = 5;
		_inMoveState = true;
		_squadronNumber = squadNum;

		_waypoints.Add(destination);

  //      if (squadNum % 2 == 1)
		//{
		//	_waypoints.Add(destination + transform.right * ((squadNum + 1) / 2) * spacing);
		//}
		//else
		//{
		//	_waypoints.Add(destination + transform.right * -(squadNum / 2) * spacing);
		//}
	}

	public void SetDestination(Vector3 destination, int squadNum)
	{
		float spacing = 5;
		_inMoveState = true;
		_squadronNumber = squadNum;

		if (squadNum % 2 == 1)
		{
			_desitination = destination + transform.right * ((squadNum + 1) / 2) * spacing;
		}
		else
		{
			_desitination = destination + transform.right * -(squadNum / 2) * spacing;
		}

		_waypoints.Clear();
	}

	void OnTriggerEnter(Collider other)
	{
		var shipControl = other.gameObject.GetComponent<BasicShipControl>();

		if (shipControl != null && Health > 0)
		{
			if (shipControl.ShipTransponder != this.ShipTransponder)
			{
				float temp = this.Health;
				this.Health -= shipControl.Health;
				shipControl.Health -= temp;
				//_isDestroyed = true;
				DestroyShipEffect();
			}
		}
	}

	public void DestroyShip()
	{
		_isDestroyed = true;
	}

	private void DestroyShipEffect()
	{
		// Basically this code will generate a gian sphere and portray the explosion.
		GameObject explosion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		explosion.GetComponent<Renderer>().material = Resources.Load("Projectile") as Material;
		//explosion.tag = "Projectile";
		explosion.GetComponent<Collider>().enabled = false;
		explosion.transform.position = this.transform.position;
		explosion.transform.localScale = Vector3.one * 2;
		explosion.AddComponent<ExplosionLife>();
	}

	public void SetTarget(GameObject target)
	{
		_target = target;

		if (GetComponent<ProjectileCannon>() != null)
		{
			GetComponent<ProjectileCannon>().Fire(target);
		}

		if(GetComponent<TractorBeam>() != null)
		{
			GetComponent<TractorBeam>().SetTarget(target);
		}

		if(GetComponent<BoardingParty>() != null)
		{
			GetComponent<BoardingParty>().SetTarget(target);
		}
	}

	public int SquadronNumber
	{
		get
		{
			return _squadronNumber;
		}
	}

	public Vector3 Desitination
	{
		get
		{
			return _desitination;
		}
	}

	public List<Vector3> Waypoints
	{
		get
		{
			return _waypoints;
		}
	}

	public bool InMoveState
	{
		get
		{
			return _inMoveState;
		}
	}

	public GameObject Target
	{
		get
		{
			return _target;
		}
	}

	public float PercentHealth
	{
		get
		{
			return Health / _initialHealth;
		}
	}
}
