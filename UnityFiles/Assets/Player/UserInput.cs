using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour 
{
	private static Vector3 initCoord;
	private static bool initCoordSet;
	private static float initMouseX;
	private static float hypot;
	private static float initAngle;

	// zoom Camera
	private void ZoomCamera ()
	{
		Vector3 camOrigin = Camera.main.transform.position;
		
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
		RaycastHit rayHit;
		
		if(Physics.Raycast(ray,out rayHit,Mathf.Infinity))
		{
			// vertical movement
			float mouseWheel = ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
			
			if(mouseWheel != 0)
			{
				Debug.Log ("mouseWheel: " + mouseWheel);
			
				// restrict y with min and max
				if(Vector3.Distance(camOrigin,rayHit.point) > ResourceManager.CamMinDistance || mouseWheel < 0)
				{
					// move Camera
					Camera.main.transform.position = Vector3.MoveTowards(camOrigin,rayHit.point, mouseWheel * Time.deltaTime * ResourceManager.ScrollSpeed);			
				}
			}
		}		
	}

	// move Camera scrolling
	private void MoveCamera()
	{
		if(!Input.GetMouseButton(1))
		{
			float xpos = Input.mousePosition.x;
			float ypos = Input.mousePosition.y;
			
			Vector3 movement = new Vector3(0,0,0);
			
			// horizontal camera movement
			if(xpos >= 0 && xpos < ResourceManager.ScrollWidth)
			{
				movement.x -= ResourceManager.ScrollSpeed;
				//Debug.Log("Movement <- X");
			}
			else if( xpos <= Screen.width && xpos > (Screen.width - ResourceManager.ScrollWidth) )
			{
				movement.x += ResourceManager.ScrollSpeed;
				//Debug.Log("Movement X ->");
			}
			// vertical camera movement
			if(ypos >= 0 && ypos < ResourceManager.ScrollWidth)
			{
				movement.z -= ResourceManager.ScrollSpeed;
				//Debug.Log("Movement <- Z ");
			}
			else if( ypos <= Screen.width && ypos > (Screen.height - ResourceManager.ScrollWidth) )
			{
				movement.z += ResourceManager.ScrollSpeed;
				//Debug.Log("Movement Z ->");
			}
			
			movement = Camera.main.transform.TransformDirection(movement);
			movement.y = 0;
			
			// vertical movement
			//movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
			
			// Calculate the desired camera position
			Vector3 origin = Camera.main.transform.position;
			Vector3 destination = origin;
			
			destination.x += movement.x;
			destination.y += movement.y;
			destination.z += movement.z;
					
			// move the Camera
			if(destination != origin)
			{
				Camera.main.transform.position = Vector3.MoveTowards(origin,destination,Time.deltaTime * ResourceManager.ScrollSpeed);
			}
		}
	}
	
	private void RotateCamera()
	{
		// detect plane/something hit with raycasting
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
		RaycastHit rayHit;
		Vector3 hitCoord;
		
#if DEBUG		
		// debug ray
		Debug.DrawRay(ray.origin, ray.direction * 400, Color.yellow);
#endif

		// check if ray hits plane
		if(Physics.Raycast(ray,out rayHit,Mathf.Infinity))
		{
			hitCoord = rayHit.point;
			//Debug.Log ("HIT at: " + hitCoord);
			
			// set init coordinate rotate axel
			if(!initCoordSet)
			{
				// enter initial values for next frame to process
				initCoordSet = true;
				initCoord = hitCoord;
				initMouseX = Input.mousePosition.x;
				
				Vector3 camOrigin = Camera.main.transform.position;
				Vector3 temp; // normalize
				temp.x = camOrigin.x - initCoord.x;
				temp.z = camOrigin.z - initCoord.z;
				temp.y = 0;
				
				hypot = Vector3.Distance(temp,new Vector3(0,0,0)); // calculate hyptenuse, y in pboth are the same os diff is only in x,z

				initAngle = Mathf.Atan2(temp.z,temp.x)*(180/Mathf.PI);
#if DEBUG
				Debug.Log("Set inits: " + initCoord + " , " + initMouseX + " , " + initAngle);
				Debug.DrawLine(initCoord, camOrigin, Color.yellow,10);
#endif
			}
			else // start rotate around y axel in init coord 
			{
				float xMouseScreen = Input.mousePosition.x;
				float xDiff = (xMouseScreen - initMouseX)/ResourceManager.RotateScale;
				
				if(xDiff != 0)
				{
					Vector3 camOrigin = Camera.main.transform.position;
					Vector3 camDest;
#if DEBUG					
					Debug.Log("xDiff: " + xDiff + " hypot: " + hypot + " initAngle: " + initAngle);
#endif					
					camDest.x = hypot * Mathf.Cos ((xDiff + initAngle)*Mathf.PI/180) + initCoord.x;
					camDest.z = hypot * Mathf.Sin ((xDiff + initAngle)*Mathf.PI/180) + initCoord.z;
					camDest.y = camOrigin.y;
#if DEBUG
					//Debug.Log ("cos: " + Mathf.Cos ((xDiff + camAngOrig.y)*Mathf.PI/180) + " sin: " + Mathf.Sin ((xDiff + camAngOrig.y)*Mathf.PI/180));
					Debug.Log("InitCoord: " + initCoord + " CamOrig: " + camOrigin + " CamDest: " + camDest);
#endif			
					// move camera to x,z, scaled with mousePointer speed
					Camera.main.transform.position = Vector3.MoveTowards(camOrigin,camDest,Time.deltaTime * ResourceManager.RotateSpeed * Mathf.Abs(xMouseScreen - initMouseX));
#if DEBUG
					Debug.DrawLine(initCoord, camDest, Color.red,10);
#endif			
					// turn camera in new location x,z towards the initCoord/target
					Camera.main.transform.LookAt(initCoord);
				}				
				initMouseX = xMouseScreen;
				initAngle += xDiff;
			}
		}		
	}
	
	void Start()
	{
		initCoordSet = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveCamera ();
		ZoomCamera ();
		
		// ALT + mouse to rotate the camera
		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
		{
			RotateCamera();
		}
		else
		{
			initCoordSet = false;
		}
	}
}
