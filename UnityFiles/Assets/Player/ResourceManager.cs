using UnityEngine;
using System.Collections;

namespace RTS{
	public class ResourceManager
	{
	 	public static float ScrollSpeed{ get{ return 25;}}
	 	public static float RotateSpeed{ get{ return 40;}}
	 	public static float ScrollWidth{ get{ return 15;}}
	 	public static float MinCameraHeight{ get{ return 10;}}
		public static float MaxCameraHeight{ get{ return 40;}}
		public static float RotateAmount{ get{ return 10;}}
		public static float RotateScale{ get{ return 5;}}
		
		private static bool _GameMenu = false;
		
		public static bool GameMenu
		{
			get	{ return _GameMenu;}
			set { _GameMenu = value;}
		}
		
		private static Vector3 _rotation;
		
		public static Vector3 baseRotation
		{
			get { return _rotation;}
			set { _rotation = value;}
		}
	}
}