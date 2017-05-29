using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScript : MonoBehaviour {

	public SpriteRenderer splash;
	public Color spriteColor;
	public float fadeIn, delay, fadeOut;
	private AudioSource splashSound;

	void Start()
	{
		spriteColor = Color.white;
		fadeIn = 0.25f;
		delay = 1.0f;
		fadeOut = 1.0f;
		splashSound = GetComponent<AudioSource> ();
		StartCoroutine (FadeInOut ());
	}

	IEnumerator FadeInOut()
	{
		yield return new WaitForSeconds (delay);
		float fade = 0.0f;
		float startTime = Time.time;
		splashSound.Play ();
		while (fade < 1f)
		{
			fade = Mathf.Lerp (0.0f, 1.0f, (Time.time - startTime) / fadeIn);
			spriteColor.a = fade;
			splash.color = spriteColor;
			yield return null;
		}

		fade = 1.0f;
		spriteColor.a = fade;
		splash.color = spriteColor;
		yield return new WaitForSeconds (delay);
		startTime = Time.time;
		while (fade > 0.0f) 
		{
			fade = Mathf.Lerp (1.0f, 0.0f, (Time.time - startTime) / fadeOut);
			spriteColor.a = fade;
			splash.color = spriteColor;
			yield return null;
		}
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene ("TitleScreen");
	}

}
