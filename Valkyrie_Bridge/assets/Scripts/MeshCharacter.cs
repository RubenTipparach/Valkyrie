using UnityEngine;
using System.Collections;

public class MeshCharacter : MonoBehaviour {

	[SerializeField]
	GameObject jetpack;

	[SerializeField]
	public ShipController controllingShip;


	// Use this for initialization
	void Start () {
		controllingShip.characterPilot = this;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.J)) 
		{
			Eject ();			
		}
	}

	/// <summary>
	/// Eject this instance. Lol...
	/// </summary>
	void Eject()
	{
		// You're not my real dad!
		transform.SetParent(null, true);
		transform.Translate (Vector3.up * 20, Space.Self);

		// What we're going to do is create an invisible ship. And it'll swoop in and grab
		// our essence that we just ejected from the ship!.....
		GameObject tempship = GameObject.Instantiate(jetpack, transform.position, transform.rotation) as GameObject;
		transform.SetParent (tempship.transform);
		controllingShip.characterPilot = null;

		controllingShip = tempship.GetComponent<ShipController> (); 
		controllingShip.characterPilot = this;
	}

	public void SetShipParent(ShipController shipController, Transform spawnPoint)
	{
		// Suck it titan fall, my game is a space ship FPS!!!!
		controllingShip.characterPilot = null;
		controllingShip = shipController;
		controllingShip.characterPilot = this;

		transform.SetParent (shipController.transform);
		transform.position = spawnPoint.position;

		Debug.Log ("Transfer protocol complete.");
	}

	void FixedUpdate()
	{
		if(controllingShip != null) {
		controllingShip.ShipFixedUpdate ();
		}
	}
}
