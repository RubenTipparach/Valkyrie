using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	[SerializeField]
	private float rotationalForce = 10000;

	[SerializeField]
	private float accelerationForce = 1000;

	[SerializeField]
	private float helperThrusterForce = 100;

	public MeshCharacter characterPilot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void ShipFixedUpdate () {

		var rigidbody = this.GetComponent<Rigidbody>();
		
		if(Input.GetKey(KeyCode.A))
		{
			rigidbody.AddRelativeTorque(0, -1 * rotationalForce, 0);
		}
		if (Input.GetKey(KeyCode.W))
		{
			rigidbody.AddRelativeTorque(1 * rotationalForce, 0, 0);
		}
		if (Input.GetKey(KeyCode.S))
		{
			rigidbody.AddRelativeTorque(-1 * rotationalForce, 0, 0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			rigidbody.AddRelativeTorque(0,1 * rotationalForce, 0);
		}
	
		if (Input.GetKey(KeyCode.Q))
		{
			rigidbody.AddRelativeTorque(0, 0, 1 * rotationalForce);
		}
		if (Input.GetKey(KeyCode.E))
		{
			rigidbody.AddRelativeTorque(0,0, -1 * rotationalForce);
		}

		//translations
		if (Input.GetKey(KeyCode.R))
		{
			rigidbody.AddRelativeForce(0, 1 * helperThrusterForce, 0);
		}
		if (Input.GetKey(KeyCode.F))
		{
			rigidbody.AddRelativeForce(0, -1 * helperThrusterForce, 0);
		}
		if (Input.GetKey(KeyCode.Z))
		{
			rigidbody.AddRelativeForce(-5 * helperThrusterForce, 0, 0);
		}
		if (Input.GetKey(KeyCode.X))
		{
			rigidbody.AddRelativeForce(5* helperThrusterForce, 0, 0);
		}



		//forward & back
		if (Input.GetKey(KeyCode.LeftControl))
		{
			rigidbody.AddRelativeForce(0, 0, -.5f * accelerationForce);
		}

		if (Input.GetKey(KeyCode.Space))
		{
			rigidbody.AddRelativeForce(0, 0, 1 * accelerationForce);
		}
	}
}
