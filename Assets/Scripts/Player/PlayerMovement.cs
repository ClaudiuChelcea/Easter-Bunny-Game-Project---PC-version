using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
	/********* Variables *********/

	// Direction
	private enum direction {
		LEFT = -1,
		NEUTRAL = 0,
		RIGHT = 1
	};

	// Speed
	public bool boolRunning = false;
	private Rigidbody2D body;
	private float horizontalMovementInput = 0f;
	[SerializeField] public float speed = 0f;
	private float default_speed = 0f;

	// Jump
	[SerializeField] private bool boolJumping = false;
	[SerializeField] private float playerJumpForce = 0f;
	private float playerJumpReset = 0f;
	[SerializeField] private float jumpResetTimer = 0f;

	// Energy
	public bool boolDucking = false;
	public static float playerEnergy = 1f;
	[SerializeField] public Slider energySlider;
	[SerializeField] private float energyConsumptionPerSprint = 0f;
	[SerializeField] private float energyConsumptionPerJump = 0f;
	[SerializeField] public float startRegeneratingEnergyTime = 0f;
	public float timeUntilStartRegeneratingEnergy = 0f;

	// Animations
	private SpriteRenderer playerSprite;
	private Animator playerAnimator;

	// Get components
	private void Awake()
	{
		body = gameObject.GetComponent<Rigidbody2D>();
		playerSprite = gameObject.GetComponent<SpriteRenderer>();
		playerAnimator = gameObject.GetComponent<Animator>();
		playerEnergy = 1f;
		energySlider.value = playerEnergy;
		default_speed = speed;
	}
	
	// Face the walking direction
	private void maintainDirection()
	{
		if (horizontalMovementInput < (float) direction.NEUTRAL && speed != 0f)
		{ // face right
			playerSprite.flipX = false;
			boolRunning= true;
		} else if (horizontalMovementInput > (float) direction.NEUTRAL && speed != 0f) { // face left
			playerSprite.flipX = true;
			boolRunning = true;
		} else { // face ahead
			boolRunning = false;
		}
	}

	// Move based on input
	private void moveToInput()
	{
		// For energy regeneration
		if (boolRunning == false)
		{
			timeUntilStartRegeneratingEnergy += Time.deltaTime;
		}
		
		if(boolRunning == true || boolJumping == true)
		{
			timeUntilStartRegeneratingEnergy = 0f;
		}

		if (timeUntilStartRegeneratingEnergy > startRegeneratingEnergyTime && boolRunning == false)
		{
			playerEnergy += energyConsumptionPerSprint * Time.deltaTime;
		}

		// Check if we can still move
		if (playerEnergy <= 0f)
		{
			boolRunning = false;
			speed = 0f;
			return;
		} else
		{
			speed = default_speed;
		}

		// Left - right
		horizontalMovementInput = Input.GetAxis("Horizontal");
		body.velocity = new Vector2(horizontalMovementInput * speed, body.velocity.y);

		// Jump
		playerJumpReset += Time.deltaTime;
		if ((Input.GetKey(KeyCode.Space) == true || Input.GetKey(KeyCode.W) == true) && boolJumping == false)
		{
			body.velocity = new Vector2(body.velocity.x, playerJumpForce);
			playerAnimator.SetTrigger("Jumping");
			boolJumping = true;
			playerJumpReset = 0f;
			playerEnergy -= energyConsumptionPerJump;
		}

		// Remove energy when running
		if(boolRunning == true)
		{
			playerEnergy -= Time.deltaTime * energyConsumptionPerSprint;
		}

		// Change energy slider
		energySlider.value = playerEnergy;
	}

	// Movement & facing direction
	private void movement()
	{
		maintainDirection();
		moveToInput();
	}

	// Execute animations
	private void animations()
	{
		playerAnimator.SetBool("Run", boolRunning);
		playerAnimator.SetBool("Ducking", boolDucking);
		if(playerJumpReset > jumpResetTimer)
		{
			playerAnimator.SetFloat("JumpReset", playerJumpReset);
		} else
		{
			playerAnimator.SetFloat("JumpReset", 0f);
		}
	}

	// Check ground collision
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			boolJumping = false;
		}
	}

	// Move body on input
	private void Update()
	{
		movement();
		animations();
	}
}
