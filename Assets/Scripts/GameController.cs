using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {


	public GameObject waveA, waveB1, waveB2, waveB3, boss;
	public int totalShips, killedShips;
	public bool gameAlive, waveDead, bossDead;

	private SpawnPowerUp pu;

	private AudioSource[] sounds;

	private int gameScore, multiplier;
	private UIScript guiFunctions;
	private bool gameOverDisplayed;

	void Start () 
	{
		sounds = GetComponents<AudioSource> ();
		sounds [0].Play ();
		pu = GameObject.Find ("PowerUpSpawn").GetComponent<SpawnPowerUp> ();
		guiFunctions = GameObject.Find ("HUD").GetComponent<UIScript> ();
		totalShips = 1;
		killedShips = 1;
		gameAlive = true;
		waveDead = false;
		bossDead = false;
		gameOverDisplayed = false;
		gameScore = 0;
		multiplier = 1;
		StartCoroutine (gameWaves());
	}

	public void addScore(int n)
	{
		gameScore += n * multiplier;
		guiFunctions.UpdateScore ("SCORE - " + gameScore.ToString () + "\nMULTI - " + multiplier.ToString () + "X");
	}

	public void changeHealth(int i)
	{
		guiFunctions.UpdateHealth (i);
	}

	public void resetMulti()
	{
		multiplier = 1;
		addScore (0);
	}

	IEnumerator guiWave()
	{
		guiFunctions.ShowWave ();
		yield return new WaitForSeconds (2f);
		guiFunctions.HideWave ();
		yield return new WaitForSeconds (0.75f);
		guiFunctions.ShowPower ();
		StartCoroutine (pu.spawn ());
		yield return new WaitForSeconds (2f);
		guiFunctions.HidePower ();
	}

	IEnumerator guiWaveMulti()
	{
		guiFunctions.ShowWave ();
		yield return new WaitForSeconds (2f);
		guiFunctions.HideWave ();
		guiFunctions.ShowPower ();
		StartCoroutine (pu.spawn ());
		yield return new WaitForSeconds (2f);
		guiFunctions.HidePower ();
		guiFunctions.ShowMulti ();
		addScore (0);
		yield return new WaitForSeconds (2f);
		guiFunctions.HideMulti ();
	}

	public IEnumerator gameOver()
	{
		yield return new WaitForSeconds (3f);
		guiFunctions.ShowGO ("GAME OVER\n\n\nSCORE\n" + gameScore.ToString());
		yield return new WaitForSeconds (3.0f);
		SceneManager.LoadScene ("TitleScreen");
	}

	IEnumerator SpawnWaveA()
	{
		int spawns = Random.Range (2, 5);
		totalShips = spawns * 5;
		killedShips = totalShips;
		for (int i = 0; i < spawns; i++) 
		{
			int randomX = Random.Range (-9, 9);
			float randomY = Random.Range (-1f, 3f);
			Instantiate (waveA, new Vector3 (randomX, randomY, 200f), waveA.transform.rotation);
			yield return new WaitForSeconds (1.5f);
		}
	}

	IEnumerator SpawnWaveB()
	{
		int spawns = Random.Range (5, 10);
		totalShips = spawns + 1;
		killedShips = totalShips;
		for (int i = 0; i < spawns; i++) 
		{
			int randomZ = Random.Range (75, 80);
			float randomY = Random.Range (-0.5f, 8f);
			Instantiate (waveB1, new Vector3 (-50f, randomY, randomZ), waveB1.transform.rotation);
			yield return new WaitForSeconds (0.5f);
		}
		yield return new WaitForSeconds (1f);
		spawns = Random.Range (2, 6);
		totalShips += spawns;
		killedShips += spawns;
		for (int i = 0; i < spawns; i++) 
		{
			int randomZ = Random.Range (75, 80);
			float randomY = Random.Range (1f, 12f);
			Instantiate (waveB2, new Vector3 (50f, randomY, randomZ), waveB2.transform.rotation);
			yield return new WaitForSeconds (2f);
		}
		yield return new WaitForSeconds (5f);
		float height = Random.Range (-1f, 10f);
		Instantiate (waveB3, new Vector3 (waveB3.transform.position.x, height, waveB3.transform.position.z), waveB3.transform.rotation);
	}

	public void killShip(int n)
	{
		killedShips--;
		totalShips--;
		addScore (n);
	}

	public void setGameAlive()
	{
		sounds [0].Stop ();
		sounds [1].Stop ();
		sounds [2].Play ();
		gameAlive = false;
	}

	public bool getGameAlive()
	{
		return gameAlive;
	}

	public void loseShip()
	{
		totalShips--;
		resetMulti ();
	}

	IEnumerator gameWaves()
	{
		yield return new WaitForSeconds (5f);

		while (gameAlive) 
		{
			for (int i = 0; i < 3; i++) {
				if (gameAlive) {
					int number = Random.Range (1, 3);
					if (number == 1) {
						StartCoroutine (SpawnWaveA ());
					} else if (number == 2) {
						StartCoroutine (SpawnWaveB ());
					}
					waveDead = false;
					while (waveDead == false)
						yield return null;
					Debug.Log ("dead");
					yield return new WaitForSeconds (10.0f);
				} else
					break;
			}
			killedShips = 5;
			totalShips = 5;
			sounds [0].Stop ();
			yield return new WaitForSeconds (2.0f);
			sounds [3].Play ();
			yield return new WaitForSeconds (5.0f);
			Instantiate (boss, new Vector3 (0.0f, 5.0f, 30f), Quaternion.identity);
			yield return new WaitForSeconds (2.0f);
			sounds [1].Play ();
			bossDead = false;
			while (!bossDead)
				yield return null;
			sounds [1].Stop ();
			yield return new WaitForSeconds (2.0f);
			guiFunctions.winjingle ();
			guiFunctions.showBossWin ("BOSS DEFEATED!");
			yield return new WaitForSeconds (0.75f);
			guiFunctions.showBossWin ("BOSS DEFEATED!\n\nSCORE BONUS");
			yield return new WaitForSeconds (0.75f);
			guiFunctions.showBossWin ("BOSS DEFEATED!\n\nSCORE BONUS\n\n" + (5000*multiplier).ToString());
			addScore (5000);
			yield return new WaitForSeconds (3.0f);
			guiFunctions.showBossWin ("");
			yield return new WaitForSeconds (0.5f);
			sounds [0].Play ();

		}
	}

	public void setBossDead()
	{
		bossDead = true;
	}
		
	void Update () 
	{
		if (killedShips == 0) {
			killedShips = 5;
			totalShips = 5;
			multiplier++;
			if(!gameOverDisplayed)
				StartCoroutine (guiWaveMulti());
			waveDead = true;
			return;
		}
		if (totalShips == 0) {
			killedShips = 5;
			totalShips = 5;
			if(!gameOverDisplayed)
				StartCoroutine (guiWave ());
			waveDead = true;
			return;
		}

	
	}
}
