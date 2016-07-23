using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

	bool InSelectState;
	Vector2 startRect;

	List<GameObject> shipsSelected;
	List<ScreenInfo> boardingInformation;
	List<ScreenInfo> shipHealth;

	// Use this for initialization
	void Start ()
	{
		shipsSelected = new List<GameObject>();
		boardingInformation = new List<ScreenInfo>();
		shipHealth = new List<ScreenInfo>();
	}
	
	// Update is called once per frame
	void Update()
	{
		MoveCamera();
		OrderShips();
	}

	void OnGUI()
	{
		MouseSelectProc();

		// Do the selection logic here
		Object[] gb = shipsSelected.ToArray();
		foreach (Object o in gb)
		{
			if (o != null)
			{

				GameObject obj = GameObject.Find(o.name);
				Vector2 screenPos = this.GetComponent<Camera>().WorldToScreenPoint(obj.transform.position);
				//Debug.Log(screenPos);

				if (obj.name != "Main Camera")
				{
					if (obj.GetComponent<Renderer>().isVisible
						&& screenPos.x < Screen.width
						&& Screen.height - screenPos.y < Screen.height)
					{
						GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, obj.name.Length * 10, 20), obj.name);
					}
				}
			}
		}

		foreach (var info in boardingInformation)
		{
			// Be WARNED! We need to block stuff from drawing here.
			GUI.Label(new Rect(info.ScreenPos.x, Screen.height - info.ScreenPos.y, info.Info.Length * 10, 20), info.Info);
		}

		BasicShipControl[] objects = GameObject.FindObjectsOfType<BasicShipControl>();
		foreach (var o in objects)
		{
			Vector3 screenPos = GetComponent<Camera>().WorldToScreenPoint(o.transform.position);
			if (o.ShipTransponder != ShipTag.Environment
				&& screenPos.x < Screen.width
				&& Screen.height - screenPos.y < Screen.height)
			{
				float healthRatio = o.PercentHealth;

				GUI.contentColor = Color.green;

				if (healthRatio < .5f)
				{
					GUI.contentColor = Color.yellow;
				}

				if (healthRatio < .25f)
				{
					GUI.contentColor = Color.red;
				}

				string printHealth = "-";
				for (int i = 0; i <= Mathf.FloorToInt(10 * healthRatio); i++)
				{
					printHealth += "-";
				}

				GUI.Label(new Rect(screenPos.x - 20, Screen.height - screenPos.y - 40, 100 * healthRatio, 30), printHealth);
				//GUI.Box(new Rect(screenPos.x, Screen.height - screenPos.y, 50 * healthRatio, 100), "-------");
			}
		}
	}

	void OnPostRender()
	{
		Material lineMaterial = Resources.Load<Material>("LineMaterial");
		// Maybe in efficient, but whatever.

		if(boardingInformation == null)
		{
			boardingInformation.Clear();
		}
		
		boardingInformation.Clear();
		foreach (var ship in shipsSelected)
		{
			if (ship.gameObject != null)
			{

				BasicShipControl shipControl = ship.GetComponent<BasicShipControl>();
				if (shipControl.InMoveState)
				{
					//LineLibrary.LineDrawing(lineMaterial, Color.yellow, ()
					//	=>
					//{
					//	Vector3 s = transform.position;
					//	Vector3 d = shipControl.Desitination;

					//	GL.Vertex3(s.x, s.y, s.z);
					//	GL.Vertex3(d.x, d.y, d.z);
					//});
					if (shipControl.Waypoints.Count > 0)
					{
						for (int i = 0; i < shipControl.Waypoints.Count; i++ )
						{
							if (i == 0)
							{
								LineLibrary.LineDrawing(lineMaterial, Color.yellow, shipControl.transform.position, shipControl.Waypoints[i]);
							}
							else
							{
								LineLibrary.LineDrawing(lineMaterial, Color.yellow, shipControl.Waypoints[i-1], shipControl.Waypoints[i]);
							}
						}
					}
					else
					{
						LineLibrary.LineDrawing(lineMaterial, Color.yellow, shipControl.transform.position, shipControl.Desitination);
					}
				}

				var targetColor = Color.white;
				// or any weapoon type.
				if (ship.GetComponent<ProjectileCannon>() != null)
				{
					if (shipControl.Target != null)
					{
						LineLibrary.LineDrawing(lineMaterial, Color.red, shipControl.transform.position, shipControl.Target.transform.position);
					}
				}
				
				if (ship.GetComponent<TractorBeam>() != null)
				{
					if (ship.GetComponent<TractorBeam>().Target != null)
					{
						LineLibrary.LineDrawing(lineMaterial, Color.blue, shipControl.transform.position, shipControl.Target.transform.position);
					}
				}

				if (ship.GetComponent<BoardingParty>() != null)
				{
					var boardingParty = ship.GetComponent<BoardingParty>();
					if (boardingParty.Target != null)
					{
						if (boardingParty.BoardingInProgress)
						{
							LineLibrary.LineDrawing(
								lineMaterial,
								Color.green,
								shipControl.transform.position,
								shipControl.Target.transform.position);
							boardingInformation.Add(new ScreenInfo(
								"Boarding: " + (int)((boardingParty.TimePassed/boardingParty.TimeToBoard)*100) + "%",
								GetComponent<Camera>().WorldToScreenPoint(boardingParty.Target.transform.position)));
						}
						else
						{
							LineLibrary.LineDrawing(lineMaterial, Color.red, shipControl.transform.position, shipControl.Target.transform.position);
						}
					}
				}

				if(ship.GetComponent<Sensors>() != null)
				{

				}
			}
		}

		//Find all box triggers, and render them.
		TriggerBox[] triggerBoxes = GameObject.FindObjectsOfType<TriggerBox>();
		foreach(var trigger in triggerBoxes)
		{
			if(trigger.isActiveAndEnabled)
			{
				trigger.OnPostRenderRemote();
			}
		}
	}

	// This procedure is to be used to move the camera around.
	private void MoveCamera()
	{
		if(Input.GetKey(KeyCode.W))
		{
			transform.Translate(Vector3.forward, Space.World);
		}
		if(Input.GetKey(KeyCode.A))
		{
			transform.Translate(Vector3.left, Space.World);
		}
		if(Input.GetKey(KeyCode.S))
		{
			transform.Translate(Vector3.back, Space.World);
		}
		if(Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector3.right, Space.World);
		}

		//implement middle mouse move as secondary method
	}

	Vector3 firstPos = Vector3.zero;

	// This procedure is to be used to select objects in game.
	private void MouseSelectProc()
	{
		Rect t = new Rect(0, 0, Screen.width, Screen.height);
		GUI.BeginGroup(t);
		GUI.contentColor = Color.green;

		Vector2 currentMousePosition = Input.mousePosition;
		List<GameObject> tempSelected = new List<GameObject>();
		bool first = true;
		

		// Step 1 mouse is pressed.
		if(Input.GetMouseButtonDown(0))
		{
			InSelectState = true;
			startRect = Input.mousePosition;

			//Test ray to intersect friendly ships.
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.gameObject.GetComponent<BasicShipControl>().ShipTransponder == ShipTag.Friendly)
				{
					InSelectState = false;
					shipsSelected.Clear();
					shipsSelected.Add(hit.collider.gameObject);
				}
			}
		}

		// Step 2 mouse is being held down.
		if (Input.GetMouseButton(0) && InSelectState)
		{
			Rect selection = GetSelectionRect(startRect, currentMousePosition);
			GUI.Box(selection , string.Empty);

			// Do the selection logic here
			Object[] gb = GameObject.FindObjectsOfType(typeof(BasicShipControl));
			foreach (Object o in gb)
			{
				GameObject obj = GameObject.Find(o.name);
				Vector2 screenPos = GetComponent<Camera>().WorldToScreenPoint(obj.transform.position);
				//Debug.Log(screenPos);
				
				if (obj.name != "Main Camera")
				{
					screenPos.y = Screen.height - screenPos.y;
					// If we hit the plane, set move to that location.
					if (obj.GetComponent<Renderer>().isVisible 
						//&& (Screen.width - screenPos.x) < Screen.width 
						//&& (Screen.height - screenPos.y)< Screen.height
						&& selection.Contains(screenPos)
						&& obj.GetComponent<BasicShipControl>().ShipTransponder == ShipTag.Friendly)
					{
						if (first)
						{
							firstPos = obj.transform.position;
                        }

						tempSelected.Add(obj);
						//GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, obj.name.Length * 10, 20), obj.name);
					}
				}
			}
			shipsSelected = tempSelected;
		}

		// Step 3 on mouse up select objects in rectangle set the select state to false.
		if(Input.GetMouseButtonUp(0))
		{
			InSelectState = false;
		}

		GUI.EndGroup();
	}

	void OrderShips()
	{
		if (Input.GetMouseButtonDown(1))
		{
			if(shipsSelected.Count > 0)
			{
				
				// Point the ray of the mouse into the screen.
				Vector3 rayHitPosition = Vector3.zero;
				Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				Plane plane = new Plane(Vector3.up, 0);
				float rayDistance = 0;
				bool hitEnemy = false;

				if(plane.Raycast(ray, out rayDistance))
				{
					rayHitPosition = ray.GetPoint(rayDistance);
					//print(rayHitPosition);
				}
				
				// Else if it is an enemy attack them.
				if (Physics.Raycast(ray, out hit))
				{
					hitEnemy = true;
				}
				

				int i = 0;
				foreach ( var ship in shipsSelected)
				{
					if (ship != null)
					{
						if (!hitEnemy)
						{
							Vector3 raypos = rayHitPosition;
							if (Input.GetKey(KeyCode.LeftShift))
							{
								
								//make ships stay in form
				
								raypos += ship.transform.position - firstPos;
								
								ship.GetComponent<BasicShipControl>().SetDestinations(raypos, 0);

								i++;
							}
							else
							{
								
								raypos += ship.transform.position - firstPos;
								

								//Debug.Log("r = " + raypos + "f = " + firstPos + "s" + ship.transform.position);
								ship.GetComponent<BasicShipControl>().SetDestination(raypos, 0);
								i++;
							}
						}
						else
						{
							ship.GetComponent<BasicShipControl>().SetTarget(hit.transform.gameObject);
							//print(hit.transform.gameObject.name);
						}
					}
				}
			}
		}
	}

	private Rect GetSelectionRect(Vector2 start, Vector2 end)
	{
		int width = (int)(end.x - start.x);
		int height = (int)( (Screen.height - end.y) - (Screen.height - start.y));

		if(width < 0 && height < 0)
		{
			return (new Rect(end.x, Screen.height - end.y, Mathf.Abs(width), Mathf.Abs(height)));
		}
		else if (width < 0)
		{
			return (new Rect(end.x, Screen.height - start.y,  Mathf.Abs(width), height));
		}
		else if (height < 0)
		{
			return (new Rect(start.x, Screen.height - end.y, width,  Mathf.Abs(height)));
		}
		else
		{
			return (new Rect(start.x, Screen.height - start.y, width, height));
		}
	}

	public struct ScreenInfo
	{
		public string Info;

		public Vector2 ScreenPos;

		public ScreenInfo(string info, Vector2 screenPos)
		{
			Info = info;
			ScreenPos = screenPos;
		}
	}

	public struct ScreenObjectDisplay
	{
		public Object Stuff;

		public Vector3 ScreenPos;

	}
}
