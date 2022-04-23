using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public void ExitGame()
	{
		Application.Quit();
	}

	public void LoadLevel1()
	{
		SceneManager.LoadScene("Level 1");
	}

	public void LoadLevel2()
	{
		SceneManager.LoadScene("Level 2");
	}

	public void LoadLevel3()
	{
		SceneManager.LoadScene("Level 3");
	}

	public void LoadLevel4()
	{
		SceneManager.LoadScene("Level 4");
	}

	public void LoadLevel5()
	{
		SceneManager.LoadScene("Level 5");
	}

	public void LoadLevel6()
	{
		SceneManager.LoadScene("Level 6");
	}

	public void LoadLevel7()
	{
		SceneManager.LoadScene("Level 7");
	}
}
