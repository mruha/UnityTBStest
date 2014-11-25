﻿using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour 
{

	// private methods
	private void MoveCamera()
	{
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		
		Vector3 movement = new Vector3(0,0,0);
		
		// horizontal camera movement
		if(xpos >= 0 && xpos < ResourceManager.ScrollWidth)
		{
			movement.x -= ResourceManager.ScrollSpeed;
			Debug.Log("Movement <- X");
		}
		else if( xpos <= Screen.width && xpos > (Screen.width - ResourceManager.ScrollWidth) )
		{
			movement.x += ResourceManager.ScrollSpeed;
			Debug.Log("Movement X ->");
		}
		// vertical camera movement
		if(ypos >= 0 && ypos < ResourceManager.ScrollWidth)
		{
			movement.z -= ResourceManager.ScrollSpeed;
			Debug.Log("Movement <- Z ");
		}
		else if( ypos <= Screen.width && ypos > (Screen.height - ResourceManager.ScrollWidth) )
		{
			movement.z += ResourceManager.ScrollSpeed;
			Debug.Log("Movement Z ->");
		}
		
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;
		
		// vertical movement
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
		
		// Calculate the desired camera position
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;
		
		// restrict y with min and max
		if(destination.y < ResourceManager.MinCameraHeight)
		{
			destination.y = ResourceManager.MinCameraHeight;
		}
		else if(destination.y > ResourceManager.MaxCameraHeight)
		{
			destination.y = ResourceManager.MaxCameraHeight;
		}
		
		// move the Camera
		if(destination != origin)
		{
			Camera.main.transform.position = Vector3.MoveTowards(origin,destination,Time.deltaTime * ResourceManager.ScrollSpeed);
		}
	}
	
	private void RotateCamera()
	{
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;
		
		// ALT + mouse to rotate the camera
		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
		{
			destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
			destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
		}
/*
		// alternative 90 degree solid rotations - in 90 degree steps
		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKey(KeyCode.LeftArrow))
		{
			destination.x -= ResourceManager.RotateLeft;
		}		
*/		
		// if destination != origin => rotate
		if(destination != origin)
		{
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveCamera ();
		RotateCamera();
	}
}
