using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private enum ORIENTATION 
	{
		LEFT,
		RIGHT,

		MAX,
	}

	private ORIENTATION 	m_orientation 		= ORIENTATION.RIGHT;

	private GameObject		m_spriteObj			= null;
	private GameObject		m_colliderObj		= null;

	private	BoxCollider2D	m_playerCollider	= null;
	private Collider2D 		m_lastCollided 		= null;
	private Transform 		m_thisTransform 	= null;
	private Vector2 		m_newPosition 		= Vector2.zero;
	private Vector2			m_prvPosition		= Vector2.zero;

	private int 			m_playerID			= 0;
	private bool			m_bIsFalling		= true;

	// For efficiency
	private int m_layerMask = 0;

	#region MonoBehavior
	private void OnTriggerEnter2D (Collider2D col)
	{
		/*
		// ALERT ALERT FIX FIX NOT WORKING BESTLY OPTIMIZE PLS K THANKS
		if (col.tag == "Stage")
		{
			m_lastCollided = col;
			m_moveDirection.y = 0.0f;

			m_bIsJumping = false;
			m_moveY = 0f;

			float newYPos = (col.bounds.center.y + col.bounds.extents.y + m_playerCollider.bounds.extents.y) * col.transform.localScale.y - m_playerCollider.offset.y ;
			m_thisTransform.position = new Vector2(m_thisTransform.position.x, newYPos);
		}
		*/
	}
	#endregion // MonoBehavior

	public void assignID(int in_ID)
	{
		m_playerID = in_ID;	
		gameObject.name = ("Player " + in_ID);
	}

	public int getPlayerID()
	{
		return m_playerID;
	}

	private void setMyPosition()
	{
		switch (m_playerID)
		{
		case 1:
			transform.position = new Vector3 (-4.0f, 0.0f, 0.0f);
			break;
		case 2:
			transform.position = new Vector3 (4.0f, 0.0f, 0.0f);
			break;
		default:
			break;
		}
	}

	public void Initialize ()
	{
		m_thisTransform = transform;
		m_lastCollided = null;
		setMyPosition();

		m_spriteObj = m_thisTransform.FindChild("sprite").gameObject; 
		m_colliderObj = m_thisTransform.FindChild("collider").gameObject; 
		m_playerCollider = m_colliderObj.GetComponent<BoxCollider2D>();
		m_playerAnim = m_spriteObj.GetComponent<Animator>();

		// Stage Raycasts: Layer Mask. At time of implementation, stage layer is set to 8
		int layerToCheck = 8;

		m_layerMask = 1 << 8;
	}

	public void ManualUpdate ()
	{
		_ManageAnimation();
		_ManageMovement();
	}

	#region Basic Collision Checking - Raycasts


	private bool _CheckGround ()
	{
		if (!m_bIsJumping)
			return true;

		if (!_IsNewLocationValid(InputManager.DIRECTION.DOWN))
		{
			if (m_bIsFalling)
			{
				m_moveY = 0f;
			}
			m_bIsJumping = false;
			return false;
		}

		return false;
	}

	#endregion //Basic Collision Checking - Raycasts


	#region Movement
	public void Jump ()
	{
		
	}

	private static 	float ACCEL_X 		= 2f;
	private static 	float DECEL_X 		= 1.5f;
	private static	float LIMIT_X 		= 0.2f;

	private 		float m_moveX 		= 0f;

	private void _ManageMovement () 
	{
		if (m_prvPosition.y > m_thisTransform.position.y)
		{
			m_bIsFalling = true;
		}

		m_prvPosition = m_thisTransform.position;

		_CalculateX(GetKeyDirection());

		_CheckGround();

		_CalculateY(GetKeyDownDirection());


		m_newPosition.x = m_thisTransform.position.x + m_moveX;
		m_newPosition.y = m_thisTransform.position.y + m_moveY;

		m_thisTransform.Translate(m_moveX, m_moveY, 0f);
	}

	private void _CalculateX (InputManager.DIRECTION input)
	{
		float currFrame_moveX = ACCEL_X * Time.fixedDeltaTime;
		float currFrame_decelX = DECEL_X * Time.fixedDeltaTime;

		// Inline Input
		switch (input) 
		{
		case InputManager.DIRECTION.LEFT:
			if (m_moveX > 0f) 
			{
				m_moveX -= currFrame_decelX;
			} 
			else 
			{ 
				m_moveX -= currFrame_moveX;
			}
			break;
		case InputManager.DIRECTION.RIGHT:
			if (m_moveX < 0f) 
			{
				m_moveX += currFrame_decelX;
			} 
			else 
			{
				m_moveX += currFrame_moveX;
			}
			break;
		default:
			if (m_moveX > 0f) {
				if (m_moveX - currFrame_decelX > 0f) {
					m_moveX -= currFrame_decelX;
				} else {
					m_moveX = 0f;
				}
			} 
			else 
			if (m_moveX < 0f) 
			{
				if (m_moveX < 0f) 
				{
					if (m_moveX + currFrame_decelX < 0f) {
						m_moveX += currFrame_decelX;
					} 
					else 
					{
						m_moveX = 0f;	
					}
				}
			}
			break;
		}

		// speed limit
		m_moveX = Mathf.Clamp(m_moveX, -LIMIT_X, LIMIT_X);
	}

	private bool 	m_bIsJumping 		= true;

	private static float JUMP_STRENGTH 	= 0.25f;
	private static float GRAV_STRENGTH 	= 1f;
	private static float FALL_LIMIT		= 0.8f;
	private	float 		 m_moveY 		= 0f;


	private void _CalculateY (InputManager.DIRECTION input)
	{
		// Receive input and jump
		if (input == InputManager.DIRECTION.JUMP) 
		{
			m_moveY = JUMP_STRENGTH;
			m_bIsJumping = true;
		}
	

		// if Midair
		if (m_bIsJumping) 
		{
			m_moveY -= GRAV_STRENGTH * Time.deltaTime;
		}

		m_moveY = Mathf.Clamp(m_moveY, -FALL_LIMIT, FALL_LIMIT);
	}

	private bool _IsNewLocationValid (InputManager.DIRECTION direction)
	{
		Vector2 rectPosition 	= m_playerCollider.transform.position;
		Vector2 rectSize 		= Vector2.zero; //new Vector2(m_playerCollider.bounds.size.x, m_playerCollider.bounds.size.y);
		Vector2 castDirection 	= Vector2.zero;
		Rect 	newColliderZone = new Rect(rectPosition, rectSize); 
		float 	castDistance	= 0f;

		switch (direction)
		{
		case InputManager.DIRECTION.DOWN:
			castDirection = Vector2.down;
			rectSize.x = m_playerCollider.bounds.size.x;
			castDistance = m_playerCollider.bounds.extents.y + Mathf.Abs(m_playerCollider.offset.y);
			break;
		case InputManager.DIRECTION.LEFT:
			castDirection = Vector2.left;
			rectSize.y = m_playerCollider.bounds.size.y;
			castDistance = m_playerCollider.bounds.extents.x + Mathf.Abs(m_playerCollider.offset.x);
			break;
		case InputManager.DIRECTION.UP:
			castDirection = Vector2.up;
			rectSize.x = m_playerCollider.bounds.size.x;
			castDistance = m_playerCollider.bounds.extents.y + Mathf.Abs(m_playerCollider.offset.y);
			break;
		case InputManager.DIRECTION.RIGHT:
			castDirection = Vector2.right;
			rectSize.y = m_playerCollider.bounds.size.y;
			castDistance = m_playerCollider.bounds.extents.x + Mathf.Abs(m_playerCollider.offset.x);
			break;
		default:
			break;
		}


		RaycastHit2D hit = Physics2D.Raycast(m_newPosition, Vector2.down, castDistance, m_layerMask);

		if (hit != null && hit.collider != null)
		if (hit.collider.tag == "Stage")
		{
			m_thisTransform.position = new Vector2(m_thisTransform.position.x, hit.collider.gameObject.transform.position.y + castDistance + hit.collider.bounds.extents.y);
			return false;
		}
		return true;
	}

	private enum DIRECTION
	{
		NEUTRAL,
		LEFT,
		RIGHT,
		UP,
		DOWN,

		JUMP,

		MAX,
	}

	private InputManager.DIRECTION GetKeyDirection ()
	{
		return InputManager.Instance.GetDirection (m_playerID);
	}

	private InputManager.DIRECTION GetKeyDownDirection ()
	{
		return InputManager.Instance.GetKey (m_playerID);
	}

	#endregion // Movement

	#region Animation Control

	private Animator m_playerAnim = null;

	private void _ManageAnimation ()
	{
		if (m_playerAnim == null)
			return;

		InputManager.DIRECTION charDirection = InputManager.Instance.GetDirection(m_playerID);
		switch (charDirection)
		{
		case InputManager.DIRECTION.DOWN:
			m_playerAnim.SetInteger("Direction", 0);
			break;
		case InputManager.DIRECTION.LEFT:
			m_playerAnim.SetInteger("Direction", 1);
			break;
		case InputManager.DIRECTION.UP:
			m_playerAnim.SetInteger("Direction", 2);
			break;
		case InputManager.DIRECTION.RIGHT:
			m_playerAnim.SetInteger("Direction", 3);
			break;
		default:
			break;
		}
	}

	#endregion // Animation Control
}


