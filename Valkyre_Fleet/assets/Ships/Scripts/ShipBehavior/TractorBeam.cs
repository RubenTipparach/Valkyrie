using UnityEngine;
using System.Collections;

public class TractorBeam : MonoBehaviour {

	private GameObject _target;

	public GameObject Target
	{
		get { return _target; }
	}

	public float _range = 20;

	public float _tractorBeamLength = 20;

	private Vector3 _vectorOffset;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (_target != null)
		{
			float distance = Vector3.Distance(_target.transform.position, this.transform.position);
			if (distance <= _range)
			{
			//	_target.transform.Translate(
			//		transform.position
			//		+ (Quaternion.LookRotation(_target.transform.position - this.transform.position, Vector3.up) * Vector3.forward) * distance);

				//_target.transform.Translate(gameObject.transform.position + Vector3.back * 5, Space.World);
				if(_target.GetComponent<BasicShipControl>()!= null)
				{
					_target.GetComponent<BasicShipControl>().SetDestination(
						gameObject.transform.position + gameObject.transform.forward * -10, 0);
				}
			}
		}
	}

	public void SetTarget(GameObject target)
	{
		if(_target != null)
		{
			if(target == _target)
			{
				_target = null;
				return;
			}
		}

		if (Vector3.Distance(target.transform.position, this.transform.position) <= _range)
		{
			_target = target;
			_vectorOffset = _target.transform.position - this.gameObject.transform.position;
		}
		else
		{
			_target = null;
		}
	}
}
