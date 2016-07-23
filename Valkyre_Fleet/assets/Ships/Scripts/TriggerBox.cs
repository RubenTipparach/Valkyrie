using UnityEngine;
using System.Collections;

public class TriggerBox : MonoBehaviour {

	public float width = 0;

	public float height = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Can be called remotely, and update remotely too.
	/// </summary>
	public void OnPostRenderRemote()
	{
		Material lineMaterial = Resources.Load<Material>("LineMaterial");
		LineLibrary.BoxDrawing(lineMaterial, Color.white, width, height, this.transform.position);
	}
	
}
