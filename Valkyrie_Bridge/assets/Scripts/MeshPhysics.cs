using UnityEngine;
using System.Collections;

/*
 * The Fleet Hackers: Bridge of Broken Contracts
 * Uses a custom mesh, fakey physics system that makes this game totally awesome!
 * 
 *
 */
public static class MeshPhysics {

	/// <summary>
	/// Determines if is point inside the specified mesh localPoint.
	/// 
	/// Usage:
	/// 	if(someMesh.IsPointInside(meshTransform.InverseTransformPoint(yourPoint))) { }
	/// </summary>
	/// <returns><c>true</c> if is point inside the specified mesh localPoint; otherwise, <c>false</c>.</returns>
	/// <param name="mesh">Mesh.</param>
	/// <param name="localPoint">Local point.</param>
	public static bool IsPointInside(this Mesh mesh, Vector3 localPoint)
	{
		var verts = mesh.vertices;
		var tris = mesh.triangles;

		int trianglesCount = tris.Length / 3;

		for (int i = 0; i < trianglesCount; i++) {
			var v1 = verts [tris [i * 3]];
			var v2 = verts [tris [i * 3 + 1]];
			var v3 = verts [tris [i * 3 + 2]];
			var p = new Plane (v1, v2, v3);
			if (p.GetSide (localPoint)) {
				return false;
			}
		}

		return true;
	}
}
