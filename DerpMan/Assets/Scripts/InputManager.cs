using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	#region Singleton
	private static InputManager s_inputManager = null;
	public static InputManager Instance { get { return s_inputManager; } }
	#endregion

	#region MonoBehavior
	// Use this for initialization
	private void Awake ()
	{
		s_inputManager = this;
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

	public enum DIRECTION
	{
		NEUTRAL,
		LEFT,
		RIGHT,
		UP,
		DOWN,

		JUMP,

		MAX,
	}

	private	float STICKTHRESH 	= 0.1f;

	public DIRECTION GetDirection (int playerID)
	{
		return GetKeyDirection(playerID);
	}

	public DIRECTION GetKey(int playerID)
	{
		return GetKeyDownDirection (playerID);	
	}

	private DIRECTION GetKeyDirection (int playerID)
	{
		switch (playerID) 
		{
		case 1:
			if (Input.GetKey (KeyCode.A)) { return DIRECTION.LEFT; }
			if (Input.GetKey (KeyCode.S)) { return DIRECTION.DOWN; }
			if (Input.GetKey (KeyCode.D)) { return DIRECTION.RIGHT; }
			//if (Input.GetKey (KeyCode.W) || ) { } // jumps should use getkeydown; maybe this can be used for a glide instead
			break;
		case 2:
			/*
			if (Input.GetAxis ("Horizontal") < -STICKTHRESH) { return DIRECTION.LEFT; }
			if (Input.GetAxis ("Horizontal") > STICKTHRESH) { return DIRECTION.RIGHT; }
			if (Input.GetAxis ("Vertical") < -STICKTHRESH) { return DIRECTION.DOWN; }
			*/
			if (Input.GetKey (KeyCode.LeftArrow)) { return DIRECTION.LEFT; }
			if (Input.GetKey (KeyCode.DownArrow)) { return DIRECTION.DOWN; }
			if (Input.GetKey (KeyCode.RightArrow)) { return DIRECTION.RIGHT; }
			break;
		default:
			break;
		}

		//Debug.Log (Input.GetAxis ("Horizontal") + " ");

		return DIRECTION.NEUTRAL;
	}

	private DIRECTION GetKeyDownDirection (int playerID)
	{
		switch (playerID){
		//if (Input.GetKey (KeyCode.S)) { return DIRECTION.DOWN; } // maybe for drop-downs?
		case 1:
			if (Input.GetKeyDown (KeyCode.W)) { return DIRECTION.JUMP; }
			break;
		case 2:
			if(Input.GetKeyDown(KeyCode.UpArrow)) { return DIRECTION.JUMP; }
			break;
		default:
			break;

		//if (Input.GetKeyDown(KeyCode.Joystick1Button0)) { return DIRECTION.JUMP; }
		}
		return DIRECTION.NEUTRAL;
	}
}