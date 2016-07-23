using UnityEngine;
using System.Collections;

public class BoardingParty : MonoBehaviour {

	/*
	 * FOR NOW:
	 * So here's my rough draft of Marine boarding party logic.
	 * Step 1: The chances of 10 marines boarding a ship and killing a crew of 100 is 10%. 90% a marine will die.
	 * Step 2: recharge transporters -> takes about 5 seconds.
	 * Step 3: The chances of 20 marines killing another crew member is now 20 %. 80% marines will die.
	 * ....
	 * Step 10 + N: chances are no 100% unless more marines are introduced
	 * 
	 * TODO:
	 * Step 10 + N - M: M is when new marines are introduced, so for 10 friendly and 10 enemy marines will kill each other.
	 * 
	 * Example case: 100 crew remaining, and 100 marines have boarded the ship, 100 % kill on next cycle.
	 * Enemy inject 10 marines, the next cycle will be 90 % marines will kill crew, 10 % a marine will be killed, 90% enemy marine will be killed.
	 */

	public int Marines = 1000;

	private GameObject _target;

	public float _range = 20;

	private bool _boardingInProgress;

	private float _timeToBoard = 30; // We will take 30 seconds to board the enemy.


	private float _timePassed;


	// Use this for initialization
	void Start()
	{
		_timePassed = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (_target != null)
		{
			var shipController = _target.GetComponent<BasicShipControl>();

			if (shipController != null && Vector3.Distance(_target.transform.position, this.transform.position) <= _range)
			{
				_boardingInProgress = true;
			}
			else
			{
				_boardingInProgress = false;
				_timePassed = 0;
			}

			if(_boardingInProgress)
			{
				_timePassed += Time.deltaTime;

				if (_timePassed >= _timeToBoard)
				{
					Marines -= shipController.InitialCrew;
					shipController.ShipTransponder = ShipTag.Friendly;
					_target = null;
					_boardingInProgress = false;
					_timePassed = 0;
				}
			}
		}
	}

	public void SetTarget(GameObject target)
	{
		if (_target == null)
		{
			_target = target;
		}
		else
		{
			_timePassed = 0;
			_target = null;
		}
	}

	public GameObject Target
	{
		get
		{
			return _target;
		}
	}

	public float TimeToBoard
	{
		get
		{
			return _timeToBoard;
		}
	}

	public float TimePassed
	{
		get
		{
			return _timePassed;
		}
	}

	public bool BoardingInProgress
	{
		get
		{
			return _boardingInProgress;
		}
	}
}
