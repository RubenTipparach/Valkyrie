using UnityEngine;
using System.Collections;
using System.Threading;

public class T_01_Objective01 : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Ship_Scout")
		{
			// generate asteroid field to naviagte in, change tutorial text.
			
			this.gameObject.SetActive(false);
		}
	}
}
