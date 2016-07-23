using UnityEngine;
using System.Collections;

public class ProjectileCannon : Cannon
{

	/*
	 * Fires a projectile at the enemy.
	 * We will still have to work on the cool down process later.
	*/

	//public int SpawnMaxNumber;

	private GameObject _target;

	private TacticalStance _tacticalStance;

	private GameObject _shellObject;

	private SystemScheduler _scheduler;

	private float LifeTime = 1; // each particle lasts for 1f.


	public override void WeaponStart()
	{
		_scheduler = SystemScheduler.CreateRepeatingScheduler(2.5f, 0.5f, 0);
		_tacticalStance = TacticalStance.Neutral;

		// Create bullet object;
		_shellObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		_shellObject.transform.localScale = Vector3.one * .4f;
		_shellObject.GetComponent<Renderer>().material = Resources.Load("Projectile") as Material;
		_shellObject.AddComponent<Rigidbody>();
		_shellObject.AddComponent<BulletLife>();
		_shellObject.name = "PBullet";
		_shellObject.SetActive(false);
		_shellObject.layer = LayerMask.NameToLayer("fragments");
	}

	public override void WeaponUpdate()
	{
		if (_target != null && _target != this.gameObject)
		{
			_scheduler.RunScheduler(CreateBullet);
			Debug.DrawLine(this.transform.position, _target.transform.position);

		}
	}

	public void Fire(GameObject target)
	{
		_target = target;
		_scheduler.ForceEnableFiring();
		
	}

	private void CreateBullet()
	{
		// Need to fix this!!!!!! It doesn't work right now...
		// Check for friendly collisionsdw, avoiding shooting in such case!
		RaycastHit hit;
		Physics.SphereCast(this.transform.position, 1f, _target.transform.position - transform.position, out hit);
		//Physics.Raycast(this.transform.position, _target.transform.position, out hit);
		if (hit.collider != null)
		{
			if (hit.collider.GetComponent<BasicShipControl>() != null)
			{
				var targetInfo = hit.transform.gameObject.GetComponent<BasicShipControl>();
				if (targetInfo.ShipTransponder != this.GetComponent<BasicShipControl>().ShipTransponder)
				{
					print("cool I can shoot!");
					ActuallyCreateBullet();
				}
				else
				{
					print(hit.collider.gameObject.name + "ship is blocking!");
				}
			}
			else
			{
				ActuallyCreateBullet();
				print(hit.collider.gameObject.name + "some other object in the way");
			}
		}
		else
		{
			ActuallyCreateBullet();
			print("nothing to collide!");
		}
	}

	private void ActuallyCreateBullet()
	{
		GameObject clone = GameObject.Instantiate(
						_shellObject, transform.position,
						Quaternion.LookRotation(_target.transform.position - transform.position, Vector3.up)) as GameObject;

		clone.gameObject.SetActive(true);
		Physics.IgnoreCollision(clone.GetComponent<Collider>(), this.GetComponent<Collider>());

		//Strictly no subsystems right now.
		//foreach(Transform t in this.transform)
		//{
		//	Physics.IgnoreCollision(clone.GetComponent<Collider>(), t.GetComponent<Collider>());
		//}

		clone.GetComponent<Rigidbody>().AddForce(clone.transform.forward * 1000);
	}
}

public enum TacticalStance
{
	Passive,
	Neutral,
	Aggressive
}
