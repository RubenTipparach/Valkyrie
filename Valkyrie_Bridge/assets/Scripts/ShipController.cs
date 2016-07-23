using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var rigidbody = this.GetComponent<Rigidbody>();
		
		if(Input.GetKey(KeyCode.A))
		{
			rigidbody.AddRelativeTorque(0, -10000, 0);
		}
		if (Input.GetKey(KeyCode.W))
		{
			rigidbody.AddRelativeTorque(10000, 0, 0);
		}
		if (Input.GetKey(KeyCode.S))
		{
			rigidbody.AddRelativeTorque(-10000, 0, 0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			rigidbody.AddRelativeTorque(0,10000, 0);
		}

		//translations
		if (Input.GetKey(KeyCode.R))
		{
			rigidbody.AddRelativeForce(0, 100, 0);
		}
		if (Input.GetKey(KeyCode.F))
		{
			rigidbody.AddRelativeForce(0, -100, 0);
		}
		if (Input.GetKey(KeyCode.Z))
		{
			rigidbody.AddRelativeForce(-500, 0, 0);
		}
		if (Input.GetKey(KeyCode.X))
		{
			rigidbody.AddRelativeForce(500, 0, 0);
		}



		//forward & back
		if (Input.GetKey(KeyCode.LeftControl))
		{
			rigidbody.AddRelativeForce(0, 0, -500);
		}

		if (Input.GetKey(KeyCode.Space))
		{
			rigidbody.AddRelativeForce(0, 0, 1000);
		}
	}
}
