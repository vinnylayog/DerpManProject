using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerOld : MonoBehaviour 
{
	private InputManager.DIRECTION m_currOrient 		= InputManager.DIRECTION.NEUTRAL;
	private InputManager.DIRECTION m_prevOrient			= InputManager.DIRECTION.NEUTRAL;
	private	bool							m_bIsJumpingRight		= true;

	private GameObject						m_spriteObj			= null;
	private GameObject						m_colliderObj		= null;

	private SpriteRenderer					m_spriteRenderer	= null;
	private Dictionary<string, Sprite>		m_spriteDict		= null;
	private	BoxCollider2D					m_playerCollider	= null;
	private Collider2D 						m_lastCollided 		= null;
	private Transform 						m_thisTransform 	= null;
	private Vector2 						m_newPosition 		= Vector2.zero;
	private Vector2							m_prvPosition		= Vector2.zero;

	private Sprite 							m_idleSouth			= null;
	private Sprite							m_idleWest			= null;
	private	Sprite							m_idleNorth			= null;
	private	Sprite							m_idleEast			= null;

	private AnimationClip					m_walkEast			= null;
	private Animation						m_walkSouth			= null;
	private Animation						m_walkNorth			= null;
	private Animation						m_walkWest			= null;

	private int 							m_playerID			= 0;
	private bool							m_bIsFalling		= true;

	private float							m_vertRaycastDist	= 0f;
	private	float							m_horiRaycastDist	= 0f;

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
		m_spriteRenderer = m_spriteObj.GetComponent<SpriteRenderer>();
		m_colliderObj = m_thisTransform.FindChild("collider").gameObject; 
		m_playerCollider = m_colliderObj.GetComponent<BoxCollider2D>();
		m_playerAnim = m_spriteObj.GetComponent<Animator>();

		// Initializing access to sprite atlas
		Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Attempt Sample/Angela/angela");
		m_spriteDict = new Dictionary<string, Sprite>();
		for (int i = 0; i < sprites.Length; i++)
		{
			m_spriteDict.Add(sprites[i].name, sprites[i]);
		}

		// set idle sprites
		m_idleEast = m_spriteDict["east_neutral"];
		m_idleWest = m_spriteDict["west_neutral"];
		m_idleNorth = m_spriteDict["north_neutral"];
		m_idleSouth = m_spriteDict["south_neutral"];

		m_walkEast = Resources.Load<AnimationClip>("Sprites/Attempt Sample/Angela/Angela_Walk_East");
		Debug.Log("WALK EAST = " + m_walkEast);

		// Stage Raycasts: Layer Mask. At time of implementation, stage layer is set to 8
		int layerToCheck = 8;

		m_layerMask = 1 << 8;

		m_vertRaycastDist = m_playerCollider.bounds.extents.y + Mathf.Abs(m_playerCollider.offset.y);
		m_horiRaycastDist = m_playerCollider.bounds.extents.x + Mathf.Abs(m_playerCollider.offset.x);
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
				// landed
				m_moveY = 0f;

				if (m_bIsJumpingRight)
					m_playerAnim.SetInteger("Direction", 13);
				else m_playerAnim.SetInteger("Direction", 11);
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

		_CalculateX(_GetInputAlways());

		_CheckGround();

		_CalculateY(_GetInputDown());


		m_newPosition.x = m_thisTransform.position.x + m_moveX;
		m_newPosition.y = m_thisTransform.position.y + m_moveY;

		m_thisTransform.Translate(m_moveX, m_moveY, 0f);


	}

	private void _CalculateX (InputManager.DIRECTION input)
	{
		float currFrame_moveX = ACCEL_X * Time.fixedDeltaTime;
		float currFrame_decelX = DECEL_X * Time.fixedDeltaTime;

		// Inline Input
		if (_GetInputAlways() != InputManager.DIRECTION.NEUTRAL)
		{
			// check to see if you walked off a cliff
			if (_IsNewLocationValid(InputManager.DIRECTION.DOWN))
			{
				m_bIsJumping = true;
				m_bIsFalling = true;
			}
		}
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

			if (m_currOrient == InputManager.DIRECTION.LEFT)
			{
				m_bIsJumpingRight = false;
			}
			else if (m_currOrient == InputManager.DIRECTION.RIGHT)
			{
				m_bIsJumpingRight = true;
			}

			if (m_bIsJumpingRight) m_playerAnim.SetInteger("Direction", 23); 
			else 
			m_playerAnim.SetInteger("Direction", 21);
		}
	

		// if Midair
		if (m_bIsJumping) 
		{
			m_moveY -= GRAV_STRENGTH * Time.deltaTime;
		}

		m_moveY = Mathf.Clamp(m_moveY, -FALL_LIMIT, FALL_LIMIT);
	}

	/// <summary>
	/// Predicts to see if the new location is valid for collision by scanning all directions through raycast.
	/// </summary>
	private bool _IsNewLocationValid (InputManager.DIRECTION direction, params InputManager.DIRECTION[] directionValues)//(InputManager.DIRECTION direction, params InputManager.DIRECTION[] directionValues)
	{
		/*
		for (int i = -1; i < directionValues; i++)
		{
			if (i == -1)
			{
				if (_IsNewLocationValid(direction);
			}
			else
			{

			}
		}
		return true;
		*/
		return true;
	}

	/// <summary>
	/// Predicts to see if the new location is valid for collision by scanning one directions through raycast.
	/// </summary>
	private bool _IsNewLocationValid (InputManager.DIRECTION direction)
	{
		RaycastHit2D hit = RaycastAtDirection(direction);

		if (hit != null && hit.collider != null)
		if (hit.collider.tag == "Stage")
		{
			if (direction == InputManager.DIRECTION.LEFT || direction == InputManager.DIRECTION.RIGHT)
			{
				m_thisTransform.position = new Vector2(m_thisTransform.position.x, hit.collider.gameObject.transform.position.y + m_horiRaycastDist + hit.collider.bounds.extents.y + hit.collider.offset.x);
			}
			else
			// DIRECTION.UP AND DOWN
			{
				float colliderYRadius = hit.collider.bounds.extents.y;
				float colliderCenter = hit.collider.gameObject.transform.position.y + (hit.collider.offset.y * hit.transform.localScale.y);
				float yPos = colliderCenter + colliderYRadius + m_vertRaycastDist;// + 
				m_thisTransform.position = new Vector2(m_thisTransform.position.x, yPos);
			}
			return false;
		}
		return true;
	}

	private RaycastHit2D RaycastAtDirection (InputManager.DIRECTION direction)
	{
		Vector2 rectSize 		= Vector2.zero; 
		Vector2 castDirection 	= Vector2.zero;
		float 	castDistance 	= 0.0f;

		switch (direction)
		{
		case InputManager.DIRECTION.DOWN:
			castDirection = Vector2.down;
			rectSize.x = m_playerCollider.bounds.size.x;
			castDistance = m_vertRaycastDist;
			break;
		case InputManager.DIRECTION.LEFT:
			castDirection = Vector2.left;
			rectSize.y = m_playerCollider.bounds.size.y;
			castDistance = m_horiRaycastDist;
			break;
		case InputManager.DIRECTION.UP:
			castDirection = Vector2.up;
			rectSize.x = m_playerCollider.bounds.size.x;
			castDistance = m_vertRaycastDist;
			break;
		case InputManager.DIRECTION.RIGHT:
			castDirection = Vector2.right;
			rectSize.y = m_playerCollider.bounds.size.y;
			castDistance = m_horiRaycastDist;
			break;
		default:
			break;
		}

		RaycastHit2D hit = Physics2D.Raycast(m_newPosition, castDirection, castDistance, m_layerMask);

		return hit;
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

	private InputManager.DIRECTION _GetInputAlways ()
	{
		m_currOrient = InputManager.Instance.GetInputAlways (m_playerID);
		return m_currOrient;
	}

	private InputManager.DIRECTION _GetInputDown ()
	{
		m_currOrient = InputManager.Instance.GetInputDown(m_playerID);
		return m_currOrient;
	}

	#endregion // Movement

	#region Animation Control

	private Animator m_playerAnim = null;

	//private InputManager.DIRECTION 
	private void _ManageAnimation ()
	{
		if (m_playerAnim == null)
			return;

		InputManager.DIRECTION charDirection = InputManager.Instance.GetInputAlways(m_playerID);

		// If a button is being pressed
		if (charDirection != InputManager.DIRECTION.NEUTRAL)
		{
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
			m_prevOrient = charDirection;
		}
			
		switch (InputManager.Instance.GetInputUp(m_playerID))
		{
		case InputManager.DIRECTION.DOWN:
			m_playerAnim.SetInteger("Direction", 10);
			break;
		case InputManager.DIRECTION.LEFT:
			m_playerAnim.SetInteger("Direction", 11);
			break;
		case InputManager.DIRECTION.RIGHT:	
			m_playerAnim.SetInteger("Direction", 13);
			break;
		case InputManager.DIRECTION.UP:
			m_playerAnim.SetInteger("Direction", 12);
			break;
		}
		/*
		else
		if (charDirection == InputManager.DIRECTION.NEUTRAL && m_prevOrient != charDirection)  
		{
			switch (m_currOrient)
			{
			case InputManager.DIRECTION.DOWN:
				m_playerAnim.SetInteger("Direction", 10);
				break;
			case InputManager.DIRECTION.LEFT:
				m_playerAnim.SetInteger("Direction", 11);
				break;
			case InputManager.DIRECTION.RIGHT:	
				m_playerAnim.SetInteger("Direction", 12);
				break;
			case InputManager.DIRECTION.UP:
				m_playerAnim.SetInteger("Direction", 13);
				break;
			}
			m_prevOrient = charDirection;
		}
		*/

		//Debug.Log(m_prevOrient);
	}

	#endregion // Animation Control
}


