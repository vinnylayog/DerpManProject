/// <summary>
/// By: David Van Hoven 
/// Last Updated: 11/2/2016
/// Manager base class that extends MonoBehavior. Extend this for a Manager framework sample.
/// Not currently implemented.
/// </summary>

using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour 
{
	#region Singleton
	//private static Manager s_Manager = null;
	//public static Manager Instance { get { return s_Manager; } }
	#endregion

	#region MonoBehavior
	// Use this for initialization
	private void Awake ()
	{
		//s_inputManager = this;
	}

	private void Start () 
	{

	}

	// Update is called once per frame
	private void Update () 
	{

	}
	#endregion // MonoBehavior

	/// <summary>
	/// Update function to be called by GameManager.
	/// </summary>
	public void ManualUpdate ()
	{

	}

	/// <summary>
	/// Initialize this instance.
	/// </summary>
	protected void Initialize ()
	{

	}
}