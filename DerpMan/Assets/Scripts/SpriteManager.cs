using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour 
{
	#region Singleton
	private static SpriteManager s_spriteManager = null;
	public static SpriteManager Instance { get { return s_spriteManager; } }
	#endregion

	#region MonoBehaviour

	void Awake ()
	{
		s_spriteManager = this;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	#endregion //Monobehaviour

	public void ManualUpdate ()
	{
		
	}
}
