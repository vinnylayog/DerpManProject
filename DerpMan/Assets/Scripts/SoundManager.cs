using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	#region Singleton
	private static SoundManager s_soundManager = null;
	public static SoundManager Instance { get { return s_soundManager; } }
	#endregion

	#region MonoBehavior
	// Use this for initialization
	private void Awake ()
	{
		s_soundManager = this;
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