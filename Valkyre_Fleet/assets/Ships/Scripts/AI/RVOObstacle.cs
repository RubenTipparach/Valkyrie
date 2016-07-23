using UnityEngine;
using System.Collections;

public class RVOObstacle {

	public RVOObstacle next;
	public RVOObstacle previous;
	public Vector3 direction;
	public Vector3 point;
	public int id;
	public bool convex;

}
