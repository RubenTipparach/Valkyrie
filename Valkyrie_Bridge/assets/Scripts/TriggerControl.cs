using UnityEngine;
using System.Collections;

public class TriggerControl : MonoBehaviour {

	[SerializeField]
	GameObject parentShip;

	[SerializeField]
	Transform spawnPoint;

	// Use this for initialization
	void Start () {
		parentShip = transform.parent.parent.parent.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log (other.gameObject.name + " has entered " + parentShip.gameObject.name);

		var meshCharacter = other.GetComponent<ShipController> ().characterPilot;

		if (meshCharacter != null)
		{
			meshCharacter.SetShipParent(parentShip.GetComponent<ShipController>(), spawnPoint);
		}
	}

}
