using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	#region Singleton
	private static GameManager s_gameManager = null;
	public static GameManager Instance { get { return s_gameManager; } }
	#endregion

	private enum GAME_STATE
	{
		INIT,
		RUNNING,
		END,

		MAX
	}
		
	private GAME_STATE 		m_gameState 		= GAME_STATE.INIT;

	private GameObject 		m_playerMgrObj 		= null;
	private GameObject 		m_UIMgrObj 			= null;
	private GameObject 		m_inputMgrObj 		= null;
	private GameObject 		m_stageMgrObj		= null;
	private GameObject		m_soundMgrObj		= null;
	private GameObject 		m_spriteMgrObj 		= null;

	private PlayerManager 	m_playerManager 	= null;
	private UIManager 		m_UIManager 		= null;
	private InputManager 	m_inputManager 		= null;
	private StageManager	m_stageManager		= null;
	private SoundManager	m_soundManager		= null;
	private SpriteManager   m_spriteManager		= null;

	#region MonoBehavior
	private void Awake () 
	{
		s_gameManager = this;
	}

	// Use this for initialization
	private void Start () 
	{
	}
	
	// Update is called once per frame
	private void Update () 
	{
		switch (m_gameState) 
		{
		case GAME_STATE.INIT:
			_Initialize();
			break;
		case GAME_STATE.RUNNING:
			m_inputManager.ManualUpdate();
			m_soundManager.ManualUpdate();
			m_UIManager.ManualUpdate();
			m_stageManager.ManualUpdate();
			m_playerManager.ManualUpdate();
			m_spriteManager.ManualUpdate();
			break;
		case GAME_STATE.END:
			break;
		default:
			break;
		}
	}
	#endregion

	private void _Initialize ()
	{
		if (!_InitializeManagerObjs())
		{
			return;
		}

		if (!_InitalizeManagers())
		{
			return;
		}
			
		m_gameState = GAME_STATE.RUNNING;
	}

	private bool _InitializeManagerObjs ()
	{
		if (m_playerMgrObj == null) 
		{
			m_playerMgrObj = new GameObject();
			m_playerMgrObj.name = "PlayerManager";
			m_playerMgrObj.transform.parent = transform;
		}

		if (m_UIMgrObj == null)
		{
			m_UIMgrObj = new GameObject();
			m_UIMgrObj.name = "UIManager";
			m_UIMgrObj.transform.parent = transform;
		}

		if (m_inputMgrObj == null) 
		{
			m_inputMgrObj = new GameObject();
			m_inputMgrObj.name = "InputManager";
			m_inputMgrObj.transform.parent = transform;
		}

		if (m_stageMgrObj == null)
		{
			m_stageMgrObj = new GameObject();
			m_stageMgrObj.name = "StageManager";
			m_stageMgrObj.transform.parent = transform;
		}

		if (m_soundMgrObj == null)
		{
			m_soundMgrObj = new GameObject();
			m_soundMgrObj.name = "SoundManager";
			m_soundMgrObj.transform.parent = transform;
		}

		if (m_spriteMgrObj == null) 
		{
			m_spriteMgrObj = new GameObject();
			m_spriteMgrObj.name = "SpriteManager";
			m_spriteMgrObj.transform.parent = transform;
		}
			
		return (m_playerMgrObj && m_UIMgrObj && m_inputMgrObj && m_stageMgrObj && m_soundMgrObj && m_spriteMgrObj);
	}

	private bool _InitalizeManagers ()
	{
		if (m_playerManager == null) 
		{
			m_playerManager = m_playerMgrObj.AddComponent<PlayerManager>();

			if (!m_playerManager.Initialize())
				return false;
		}

		if (m_UIManager == null) 
		{
			m_UIManager = m_UIMgrObj.AddComponent<UIManager>();
		}

		if (m_inputManager == null) 
		{
			m_inputManager = m_inputMgrObj.AddComponent<InputManager>();
		}

		if (m_stageManager == null)
		{
			m_stageManager = m_stageMgrObj.AddComponent<StageManager>();
		}

		if (m_soundManager == null)
		{
			m_soundManager = m_soundMgrObj.AddComponent<SoundManager>();
		}

		if (m_spriteManager == null)
		{
			m_spriteManager = m_spriteMgrObj.AddComponent<SpriteManager>();
		}

		return (m_playerManager && m_UIManager && m_inputManager && m_stageManager && m_soundManager && m_spriteManager);
	}

	#region Accessors

	#endregion
}
