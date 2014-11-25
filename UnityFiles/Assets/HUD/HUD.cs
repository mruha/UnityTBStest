using UnityEngine;
using System.Collections;
using RTS;

public class HUD : MonoBehaviour {
	
	private void Update()
	{
		// register Escape key only once during pressing it down
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!ResourceManager.GameMenu)
			{
				ResourceManager.GameMenu = true;
				Time.timeScale = 0;
				Debug.Log("DrawMenu on ESC");
			}
			else
			{
				ResourceManager.GameMenu = false;
				Time.timeScale = 1;
			}
		}
	}
	
	// draw the menu
	private void DrawMenu()
	{
		int MenuWidth = 200;
		int MenuHeight = 400;
		
		// Menu group => menu group makes Box, button coordinates start from virtual 0,0 
		GUI.BeginGroup(new Rect(Screen.width/2 - MenuWidth/2,Screen.height/2 - MenuHeight/2,MenuWidth,MenuHeight));
		
		// Menu buttons
		GUI.Box (new Rect(0,0,MenuWidth,MenuHeight),"Menu");
		
		if(GUI.Button(new Rect(10,40,MenuWidth - 20,30),"Resume"))
		{
			ResourceManager.GameMenu = false;
			Time.timeScale = 1;
		}
		
		GUI.EndGroup();
	}
	
	// draw the HUD
	private void DrawHUD()
	{
		float HUDheight = 0.8F;
	
		GUI.Box(new Rect(50,(HUDheight * Screen.height),Screen.width - 100,(HUDheight * Screen.height)- 5), "HUD");
	}
	
	// OnGui Draw menus or HUD
	void OnGUI () 
	{
		if(ResourceManager.GameMenu)
		{
			DrawMenu();
		}
		else
		{
			DrawHUD();
		}
	}
}