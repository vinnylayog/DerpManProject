using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	#region Singleton
	private static PlayerManager s_playerManager = null;
	public static PlayerManager Instance { get { return s_playerManager; } }
	#endregion

	private GameObject 	m_playerPrefab 				= null;
	private GameObject 	m_playerOneObj 				= null;
	private GameObject 	m_playerTwoObj 				= null;

	private Transform 	m_playerOneTransform		= null;
	private Transform	m_playerTwoTransform		= null;

	private Player		m_playerOne					= null;
	private Player 		m_playerTwo 				= null;

	private float 		m_gravityStrength 			= 35f;
	private float		m_jumpSpeed					= 10.0f;
	private float		m_moveSpeed					= 15.0f;
	private float 		m_moveSpeedLimitCoefficient	= 3.0f;
	private float		m_inertiaCoeffiecient		= 0.8f;

	#region MonoBehavior
	// Use this for initialization
	private void Awake ()
	{
		s_playerManager = this;
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
		m_playerOne.ManualUpdate();
		//m_playerTwo.ManualUpdate ();
	}

	private void _ApplyGravity ()
	{
		//m_playerOneTransform.Translate(0f, -m_gravityStrength*Time.deltaTime, 0f);
	}

	public bool Initialize ()
	{
		_InitializeResources();	
		_InstantiateResources();

		return IsInitialized;
	}

	private void _InitializeResources ()
	{
		if (m_playerPrefab != null)
			return;
		
		m_playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
	}

	private void _InstantiateResources ()
	{
		if (m_playerPrefab == null)
			return;

		m_playerOneObj = GameObject.Instantiate(m_playerPrefab);
		m_playerOne = m_playerOneObj.AddComponent<Player>();
		m_playerOne.assignID (1);
		m_playerOne.Initialize();

		/*
		m_playerTwoObj = GameObject.Instantiate (m_playerPrefab);
		m_playerTwo = m_playerTwoObj.AddComponent<Player>();
		m_playerTwo.assignID (2);
		m_playerTwo.Initialize ();
		*/
	}

	public bool IsInitialized
	{
		get { return (m_playerOne && m_playerTwo); }
	}

	public float Gravity
	{
		get { return m_gravityStrength; }
	}

	public float JumpSpeed
	{
		get { return m_jumpSpeed; }
	}

	public float MoveSpeed
	{
		get { return m_moveSpeed; }
	}

	public float MoveSpeedLimitCoefficient
	{
		get { return m_moveSpeedLimitCoefficient; }
	}

	public float InertiaCoefficient
	{
		get { return m_inertiaCoeffiecient; }
	}
}
