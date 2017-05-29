using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreen : MonoBehaviour {


	public void loadLevel()
	{
		GetComponent<AudioSource>().Play();

		SceneManager.LoadScene ("MainGame");
	}

	public void quit()
	{
		GetComponent<AudioSource>().Play();

		Application.Quit ();
	}
}
