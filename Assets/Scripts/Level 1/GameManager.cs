using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Get objects
	public PlayerMovement player;
	public TextMeshProUGUI energyCD;
	public AudioClip get_audio;
	private AudioSource gameMusic;
	public GameObject pause_menu;
	public TextMeshProUGUI score;

	// Variables
	bool gameIsPaused = false;

	// Awake
	private void Awake()
	{
		energyCD.alpha = 0;
		score.text = 0.ToString();
	}

	// Start
	private void Start()
	{
		gameMusic = GetComponent<AudioSource>();
		gameMusic.PlayOneShot(get_audio);
		pause_menu.active = false;
	}

	// Update
	private void Update()
	{
		CD_energy_bar();
		check_pause();
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
