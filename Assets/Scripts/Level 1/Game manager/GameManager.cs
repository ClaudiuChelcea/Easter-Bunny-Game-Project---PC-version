using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public enum EggsLife
	{
		egg1 = 15,
		egg2 = 20,
		egg3 = 25
	};
	
	// Get objects
	public PlayerMovement player;
	public TextMeshProUGUI energyCD;
	public AudioClip get_audio;
	private AudioSource gameMusic;
	public GameObject pause_menu;
	public TextMeshProUGUI score;
	public TextMeshProUGUI egg1Text;
	public TextMeshProUGUI egg2Text;
	public TextMeshProUGUI egg3Text;
	public GameObject EGG1, EGG2, EGG3;

	// Variables
	int egg1Seconds = (int) EggsLife.egg1;
	int egg2Seconds = (int) EggsLife.egg2;
	int egg3Seconds = (int) EggsLife.egg3;
	private int count_eggs_received = 3;

	// Variables
	bool gameIsPaused = false;

	// Timer
	public int time_spent_in_level = 0;
	IEnumerator time()
	{
		while (true)
		{
			timeCount();
			yield return new WaitForSeconds(1);
		}
	}
	void timeCount()
	{
		time_spent_in_level += 1;
	}

	// Awake
	private void Awake()
	{
		energyCD.alpha = 0;
		score.text = player.playerScore.ToString();
		egg1Text.text = ((int) EggsLife.egg1).ToString() + "s";
		egg2Text.text = ((int)EggsLife.egg2).ToString() + "s";
		egg3Text.text = ((int)EggsLife.egg3).ToString() + "s";
	}

	// Start
	private void Start()
	{
		gameMusic = GetComponent<AudioSource>();
		gameMusic.PlayOneShot(get_audio);
		pause_menu.active = false;
		StartCoroutine(time());
	}

	// Update
	private void Update()
	{
		CD_energy_bar();
		check_pause();
		update_score();
		modify_eggs();
	}

	// Modify text colors based on seconds left
	private void colors_per_time(int seconds_left, int low, int mid, TextMeshProUGUI text)
	{
		if(seconds_left > mid)
		{
			text.color = Color.green;
		} else if(seconds_left <= mid && seconds_left > low)
		{
			text.color = Color.yellow;
		} else
		{
			text.color = Color.red;
		}
	}

	// Eggs. If all eggs die, lose the game!
	private void modify_eggs()
	{
		if(egg1Seconds > 0)
		{
			egg1Seconds = (int) EggsLife.egg1 - time_spent_in_level;
			colors_per_time(egg1Seconds, 5, 10, egg1Text);
			egg1Text.text = egg1Seconds.ToString() + "s";
			count_eggs_received = 3;
		} else if (egg2Seconds > 0)
		{
			egg1Text.text = 0.ToString();
			egg2Seconds = (int) EggsLife.egg2 - time_spent_in_level + (int) EggsLife.egg1;
			egg2Text.text = egg2Seconds.ToString() + "s";
			colors_per_time(egg2Seconds, 7, 13, egg2Text);
			Destroy(EGG1);
			count_eggs_received = 2;
		} else if(egg3Seconds > 0)
		{
			Destroy(EGG2);
			egg1Text.text = 0.ToString() + "s";
			egg2Text.text = 0.ToString() + "s"; 
			egg3Seconds = (int) EggsLife.egg3 - time_spent_in_level + (int) EggsLife.egg2 + (int) EggsLife.egg1;
			egg3Text.text = egg3Seconds.ToString() + "s";
			colors_per_time(egg3Seconds, 10, 18, egg3Text);
			count_eggs_received = 1;
		} else
		{
			Destroy(EGG3);
			count_eggs_received = 0;
			loseLevel();
		}
	}

	// Lose game
	private void loseLevel()
	{

	}

	// CD for energy bar
	private void CD_energy_bar()
	{
		// Cooldown for energy regen, display only if regening
		if (player.energySlider.value != 1)
		{
			energyCD.alpha = 1;
			float cd = player.startRegeneratingEnergyTime - player.timeUntilStartRegeneratingEnergy;
			if (cd < 0f)
				cd = 0f;
			energyCD.text = "Regen in: " + (cd).ToString("F2");

			if (cd < 0.5f)
			{
				energyCD.color = Color.green;
			}
			else if (cd >= 0.5f && cd <= 1.5f)
			{
				energyCD.color = Color.yellow;
			}
			else
			{
				energyCD.color = Color.red;
			}
		}
		else
		{
			energyCD.alpha = 0;
		}
	}

	// Score
	private void update_score()
	{
		score.text = player.playerScore.ToString();
	}

	// Pause
	private void check_pause()
	{
		if (Input.GetKeyDown(KeyCode.Escape) == true && gameIsPaused == true)
		{
			resumeLevel();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) == true && gameIsPaused == false)
		{
			Time.timeScale = 0;
			gameIsPaused = true;
			gameMusic.Pause();
			pause_menu.active = true;
		}
	}

	// Resume button
	public void resumeLevel()
	{
		Time.timeScale = 1;
		gameIsPaused = false;
		gameMusic.UnPause();
		pause_menu.active = false;
	}

	// Restart button
	public void restartLevel()
	{
		SceneManager.LoadScene("Level 1");
		resumeLevel();
	}

	// Main menu
	public void goToMainMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
