﻿using UnityEngine;
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

	private	BoxCollider2D	m_playerCollider	= null;
	private Collider2D 		m_lastCollided 		= null;
	private Transform 		m_thisTransform 	= null;
	private Vector2 		m_moveDirection 	= Vector2.zero;

	private int 			m_playerID			= 0;

	#region MonoBehavior
	private void OnTriggerEnter2D (Collider2D col)
	{
		{
			m_lastCollided = col;
			m_moveDirection.y = 0.0f;

			m_bIsJumping = false;
			m_moveY = 0f;

			m_thisTransform.position = new Vector2(m_thisTransform.position.x, (col.transform.position.y + col.bounds.size.y));
		}

		//Debug.Log(col.name + "dave " + m_moveDirection);
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
		m_playerCollider = GetComponent<BoxCollider2D>();
	}

	public void ManualUpdate ()
	{
		//_ManageMovement();

		_Movement();
	}

	public void Jump ()
	{
		
	}

	private void _ManageMovement ()
	{
		//_ManageMovementX();
		//_ManageMovementY();

		// Apply Movement
		//m_thisTransform.Translate(m_moveDirection*Time.deltaTime);
	}

	private void _ManageMovementX ()
	{
		float mspd 			= PlayerManager.Instance.MoveSpeed;
		float limitCoeff 	= PlayerManager.Instance.MoveSpeedLimitCoefficient; // coefficient of current movespeed used to determine existing speed limit
		float inertiaCoeff	= PlayerManager.Instance.InertiaCoefficient;
		float totalMovement = mspd * limitCoeff * Time.deltaTime;

		// movedirection compared against tiny threshold instead of 0 to prevent "foxtrot" trick
		bool isMovingRight = (m_moveDirection.x > -mspd); 
		bool isMovingLeft = (m_moveDirection.x < mspd);

		bool isMovingRightInertial = (m_moveDirection.x > 0f);
		bool isMovingLeftInertial = (m_moveDirection.x < 0f);
		// Move. move code to inputmanager later?
		if (Input.GetKey(KeyCode.A))
		{
			if (isMovingRight)
			{
				if (m_moveDirection.x > (-totalMovement))
				{
					// Sharper turns. Higher intensity if changing direction
					m_moveDirection.x -= totalMovement;
				}
				else
				{
					m_moveDirection.x -= mspd * Time.deltaTime;
				}
			}
			else
			{
				//Debug.Log(Time.time + " " + m_moveDirection.x);
				m_moveDirection.x = -mspd;
			}
		}
		else
			if (Input.GetKey(KeyCode.D))
			{
				if (isMovingLeft)
				{
                    // 
					if (m_moveDirection.x < -totalMovement)
					{
						// Sharper turns. Higher intensity if changing direction
						m_moveDirection.x += totalMovement;
					}
					else
					{
						m_moveDirection.x += mspd * Time.deltaTime;
					}
				}
				else
				{
					//Debug.Log(Time.time + " " + m_moveDirection.x);
					m_moveDirection.x = mspd;
				}
			}
			else
			{
				// Inertia
				float inertiaSlowDownSpeed = mspd * inertiaCoeff * Time.deltaTime;
				if (isMovingRightInertial)
				{
					m_moveDirection.x -= inertiaSlowDownSpeed;
					if (m_moveDirection.x < 0f)
					{
						m_moveDirection.x = 0f;
					}
				}
				else
				if (isMovingLeftInertial)
				{
					m_moveDirection.x += inertiaSlowDownSpeed;
					if (m_moveDirection.x > 0f)
					{
						m_moveDirection.x = 0f;
					}
				}
			}
		
	}

	private void _ManageMovementY ()
	{
		// if not on ground
		if (m_lastCollided == null)
		{
			// apply gravity
			//m_moveDirection.y -= PlayerManager.Instance.Gravity*Time.deltaTime;
			m_moveY -= PlayerManager.Instance.Gravity*Time.deltaTime;
		}

		// Jump. move code to InputManager later
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// apply jump magnitude
			//m_moveDirection.y = PlayerManager.Instance.JumpSpeed;
			m_moveY = PlayerManager.Instance.JumpSpeed;
			m_lastCollided = null;
		}
	}

	// NEW MOVEMENT
	private static 	float ACCEL_X 		= 2f;
	private static 	float DECEL_X 		= 1.5f;
	private static	float LIMIT_X 		= 0.2f;

	private 		float m_moveX 		= 0f;

	private void _Movement () 
	{
		_MovementX();
		_MovementY();
		m_thisTransform.Translate(m_moveX, m_moveY, 0f);
		//Debug.Log (m_moveX + " " + m_moveY);
		//m_thisTransform.position = new Vector2(m_thisTransform.position.x + m_moveX, m_thisTransform.position.y + m_moveY);
	}

	private void _MovementX ()
	{
		float currFrame_moveX = ACCEL_X * Time.fixedDeltaTime;
		float currFrame_decelX = DECEL_X * Time.fixedDeltaTime;

		// Inline Input
		switch (GetKeyDirection()) 
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

	private bool 	m_bIsJumping 		= false;

	private static float JUMP_STRENGTH 	= 0.25f;
	private static float GRAV_STRENGTH 	= 1f;
	private static float FALL_LIMIT		= 0.8f;
	private	float 	m_moveY 		= 0f;


	private void _MovementY ()
	{
		//_ManageMovementY();
		if (InputManager.Instance.GetKey(m_playerID) == InputManager.DIRECTION.JUMP) 
		{
			m_moveY = JUMP_STRENGTH;
			m_bIsJumping = true;
		}

		if (m_bIsJumping) 
		{
			m_moveY -= GRAV_STRENGTH * Time.deltaTime;
		}

		m_moveY = Mathf.Clamp(m_moveY, -FALL_LIMIT, FALL_LIMIT);
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
		/*
		if (Input.GetKey (KeyCode.A)) { return DIRECTION.LEFT; }
		if (Input.GetKey (KeyCode.S)) { return DIRECTION.DOWN; }
		if (Input.GetKey (KeyCode.D)) { return DIRECTION.RIGHT; }
		//if (Input.GetKey (KeyCode.W) || ) { } // jumps should use getkeydown; maybe this can be used for a glide instead
		*/

		return InputManager.Instance.GetDirection (m_playerID);
	}

	private InputManager.DIRECTION GetKeyDownDirection ()
	{
		/*
		//if (Input.GetKey (KeyCode.S)) { return DIRECTION.DOWN; } // maybe for drop-downs?
		if (Input.GetKeyDown(KeyCode.Space)) { return DIRECTION.JUMP; }

		return DIRECTION.NEUTRAL;
		*/

		return InputManager.Instance.GetKey (m_playerID);
	}
}

