using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	[SerializeField] private bool boolGrounded = true;
	[SerializeField] private float playerJumpForce = 0f;
	public GameObject ground;
	const float DISTANCE_TO_GROUND = 1.3f;

	// Energy
	public static float playerEnergy = 1f;
	[SerializeField] public Slider energySlider;
	[SerializeField] private float energyConsumptionPerSprint = 0f;
	[SerializeField] private float energyConsumptionPerJump = 0f;
	[SerializeField] public float startRegeneratingEnergyTime = 0f;
	public float timeUntilStartRegeneratingEnergy = 0f;
	public TextMeshProUGUI notEnoughEnergyError;
	public Vector3 position_to_player;
	[SerializeField] private float CarrotEnergy;

	// Animations
	private SpriteRenderer playerSprite;
	private Animator playerAnimator;

	// Score
	public int playerScore = 0;

	// Win
	public bool is_at_the_exit = false;

	// Get components
	private void Awake()
	{
		body = gameObject.GetComponent<Rigidbody2D>();
		playerSprite = gameObject.GetComponent<SpriteRenderer>();
		playerAnimator = gameObject.GetComponent<Animator>();
		playerEnergy = 1f;
		energySlider.value = playerEnergy;
		default_speed = speed;
		notEnoughEnergyError.alpha = 0f;
		boolGrounded = true;
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
		
		if(boolRunning == true || boolGrounded == false)
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
		if ((Input.GetKey(KeyCode.Space) == true || Input.GetKey(KeyCode.W) == true) && boolGrounded == true)
		{
			if (playerEnergy <= energyConsumptionPerJump)
			{
				StartCoroutine(not_enough_energy("Jump"));
				boolGrounded = true;
			}
			else
			{
				body.velocity = new Vector2(body.velocity.x, playerJumpForce);
				boolGrounded = false;
				playerEnergy -= energyConsumptionPerJump;
			}
		}

		// Remove energy when running
		if(boolRunning == true)
		{
			playerEnergy -= Time.deltaTime * energyConsumptionPerSprint;
		}

		// Display no energy
		if(playerEnergy <= 0f && horizontalMovementInput != 0)
		{
			StartCoroutine(not_enough_energy("Run"));
		}

		// Change energy slider
		energySlider.value = playerEnergy;
	}

	// No enough energy error
	IEnumerator not_enough_energy(string type_of_action)
	{
		notEnoughEnergyError.alpha = 1;
		notEnoughEnergyError.text = "Not enough energy to " + type_of_action.ToLower() + "!";
		
		yield return new WaitForSeconds(2);
		notEnoughEnergyError.alpha = 0;
	}

	// Update color to reflect tiredness
	private void update_moving_color()
	{
		playerSprite.color = new Color(255f - energySlider.value, energySlider.value * 255f, energySlider.value * 255f);
	}

	// Movement & facing direction
	private void movement()
	{
		maintainDirection();
		moveToInput();
		update_moving_color();
	}

	// Execute animations
	private void animations()
	{
		playerAnimator.SetBool("Run", boolRunning);
		playerAnimator.SetBool("Grounded", boolGrounded);
	}

	// Check ground collision && exit collision
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			boolGrounded = true;
		} else if(collision.gameObject.tag == "ExitLevel")
		{
			is_at_the_exit = true;
		}
	}

	// Check if not touching exit
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "ExitLevel")
		{
			is_at_the_exit = false;
		}
	}

	// Triggers
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Carrot")
		{
			Destroy(collision.gameObject);
			++playerScore;
			playerEnergy = (playerEnergy + CarrotEnergy);
			if (playerEnergy > 1f)
				playerEnergy = 1f;
		} else if (collision.gameObject.tag == "ExitLevel")
		{
			is_at_the_exit = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "ExitLevel")
		{
			is_at_the_exit = false;
		}
	}

	// Move body on input
	private void Update()
	{
		movement();
		animations();
	}
}
