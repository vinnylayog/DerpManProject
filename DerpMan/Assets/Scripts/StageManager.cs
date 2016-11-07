using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour 
{
	#region Singleton
	private static StageManager s_stageManager = null;
	public static StageManager Instance { get { return s_stageManager; } }
	#endregion

	#region MonoBehavior
	// Use this for initialization
	private void Awake ()
	{
		s_stageManager = this;
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