using UnityEngine;
using System.Collections;

public class BasicAiControl : MonoBehaviour {

	AIBehaviorState _behaviorState;
	
	bool _active;

	bool _pendingOrders;

	BasicShipControl _shipControl;

	SystemScheduler _scheduler;

	// Use this for initialization
	void Start () {
		_active = true;
		//in tutorial ships are triggerd via box, fix this later.
		//_pendingOrders = false;
		_pendingOrders = true;
		_shipControl = this.gameObject.GetComponent<BasicShipControl>();
		_scheduler = SystemScheduler.CreateRepeatingScheduler(0.1f, 0.1f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		_scheduler.RunScheduler(ContinuousMove);
	}

	void ContinuousMove()
	{
		if (_pendingOrders)
		{
			float randomXOffset = Random.Range(-10f, 10f);
			float randomZOffset = Random.Range(-10f, 10f);

			// Lets attack a friendly ship for fun first.
			BasicShipControl[] ships = GameObject.FindObjectsOfType<BasicShipControl>();
			
			//for (int i = 0; i < ships.Length; i++)
			//{
				int randomShip = Random.Range(0, ships.Length - 1);
				if (ships[randomShip].ShipTransponder == ShipTag.Friendly)
				{
					MoveAI(ships[randomShip].transform.position + Vector3.forward * randomXOffset + Vector3.right * randomZOffset);
					
					//Set cannon! you must evade :)
					var cannon = this.GetComponent<ProjectileCannon>();
					if(cannon != null)
					{
						cannon.Fire(ships[randomShip].gameObject);
					}

					//_pendingOrders = false;
				}
			//}
		}
	}

	void MoveAI(Vector3 position)
	{
		_shipControl.SetDestination(position, 0);
	}

	void OrderAttack(GameObject target)
	{

	}

	public void GiveOrder()
	{
		_pendingOrders = true;
	}
}

public enum AIBehaviorState
{
	Idle,
	Gaurd,
	Defensive,
	Attack
}