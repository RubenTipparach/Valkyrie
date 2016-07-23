using UnityEngine;
using System.Collections;

public abstract class Cannon : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		WeaponStart();
	}
	
	// Update is called once per frame
	void Update ()
	{
		WeaponUpdate();	
	}

	public abstract void WeaponStart();

	public abstract void WeaponUpdate();
}
