using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
	// Get objects
	public PlayerMovement player;
	public TextMeshProUGUI energyCD;

	// Variables


	// Awake
	private void Awake()
	{
		energyCD.alpha = 0;
	}

	// Update
	private void Update()
	{
		CD_energy_bar();
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

			if(cd < 0.5f)
			{
				energyCD.color = Color.green;
			} else if (cd >= 0.5f && cd <= 1.5f)
			{
				energyCD.color = Color.yellow;
			} else
			{
				energyCD.color = Color.red;
			}
		}
		else
		{
			energyCD.alpha = 0;
		}
	}
}