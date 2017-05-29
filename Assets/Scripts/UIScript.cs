using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

	private Text[] text;
	private AudioSource[] sounds; 
	private Image img;

	void Start () {
		text = GetComponentsInChildren<Text> ();
		sounds = GetComponents<AudioSource> ();	
		img = GetComponentInChildren<Image> ();
	}
	
	public void UpdateScore(string s)
	{
		text [0].text = s;
	}

	public void UpdateHealth(int i)
	{
		text [1].text = "LIVES - " + i;
	}

	public void ShowWave()
	{
		text [2].text = "WAVE DEFEATED!";
		sounds [0].Play ();
	}

	public void HideWave()
	{
		text [2].text = "";
	}

	public void ShowPower()
	{
		text [4].text = "INCOMING POWERUP!";
	}

	public void HidePower()
	{
		text [4].text = "";
	}

	public void ShowMulti()
	{
		text [3].text = "MULTIPLIER INCREASED!";
		sounds [3].Play ();
	}

	public void HideMulti()
	{
		text [3].text = "";
	}

	public void ShowGO(string s)
	{
		text [5].text = s;
		sounds [1].Play ();
	}

	public void showBossWin(string s)
	{
		text [6].text = s;
	}

	public void winjingle()
	{
		sounds [2].Play ();
	}

	public void showHealth()
	{
		text [7].text = "BOSS HEALTH -";
	}

	public void hideHealth()
	{
		text [7].text = "";
	}

	public void adjustBar(float f)
	{
		float size = img.rectTransform.localScale.x;
		img.rectTransform.localScale = new Vector3 (size + f, 0.25f, 1f);
		sounds [4].Play ();
	}

}
