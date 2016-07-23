using UnityEngine;
using System.Collections;

public class BulletLife : MonoBehaviour {

	private float LifeTime = 2;

	private float timeLived;

	// Use this for initialization
	void Start () {
		timeLived = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeLived += Time.deltaTime;

		if(timeLived >= LifeTime)
		{
			DestroyBullet();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		DestroyBullet();
		if(other.gameObject.GetComponent<BasicShipControl>() != null)
		{
			other.gameObject.GetComponent<BasicShipControl>().Health -= 4;
		}
	}

	private void DestroyBullet()
	{
		// Basically this code will generate a gian sphere and portray the explosion.
		GameObject explosion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		explosion.GetComponent<Renderer>().material = Resources.Load("Projectile") as Material;

		float colorRange = Random.Range(0f, .5f);
		float colorRange2 = Random.Range(0f, .5f);
		//explosion.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
		explosion.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f - colorRange, .5f - colorRange2, 0));

		//explosion.tag = "Projectile";
		explosion.GetComponent<Collider>().enabled = false;
		explosion.transform.position = this.transform.position;
		explosion.transform.localScale = Vector3.one * 2;
		explosion.AddComponent<ExplosionLife>();
		Destroy(gameObject);
	}
}
