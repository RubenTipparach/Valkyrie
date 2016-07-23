using UnityEngine;
using System.Collections;

public class T_02_Objective01 : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name.Contains("PBullet"))
		{
			// Destroy trigger, enable the enemy ship
			GameObject enemyShip = GameObject.Find("En_Ship_Assualt");
			enemyShip.GetComponent<BasicAiControl>().GiveOrder();
			this.gameObject.SetActive(false);
		}
	}
}
