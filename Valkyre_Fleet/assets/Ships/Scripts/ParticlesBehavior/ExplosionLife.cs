using UnityEngine;
using System.Collections;

public class ExplosionLife : MonoBehaviour {

	private float _maxScaleSize = 10;
	private float _growthRate = 1.01f;
	// Use this for initialization
	void Start () {
		int r = Random.Range(-5, 5);
		_maxScaleSize += r;
	}
	
	// Update is called once per frame
	void Update () {

		transform.localScale *= _growthRate;
		if (Vector3.Magnitude(transform.localScale) > _maxScaleSize)
		{
			Destroy(gameObject);
		}
	}
}
