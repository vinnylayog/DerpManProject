using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	#region Singleton
	private static UIManager s_uiManager = null;
	public static UIManager Instance { get { return s_uiManager; } }
	#endregion

	#region MonoBehavior
	// Use this for initialization
	private void Awake ()
	{
		s_uiManager = this;
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
}