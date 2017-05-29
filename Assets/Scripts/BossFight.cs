using UnityEngine;
using System.Collections;

public class BossFight : MonoBehaviour {

	public GameObject left, right, head, head2;
	public Transform leftPos, rightPos, headPos;
	private bool leftAlive, rightAlive, headAlive;

	private AudioSource[] sounds;

	void Start () 
	{
		leftAlive = true;
		rightAlive = true;
		headAlive = true;
		sounds = GetComponents<AudioSource> ();
		StartCoroutine (bossSpawn ());
	}
	

	IEnumerator bossSpawn()
	{
		sounds [0].Play ();
		yield return new WaitForSeconds (2f);
		sounds [1].Play ();
		head2 = Instantiate (head, headPos.position, head.transform.rotation) as GameObject;
		yield return new WaitForSeconds (0.5f);
		sounds [1].Play ();
		Instantiate (left, leftPos.position, left.transform.rotation);
		yield return new WaitForSeconds (0.5f);
		sounds [1].Play ();
		Instantiate (right, rightPos.position, right.transform.rotation);
		yield return new WaitForSeconds (0.5f);
	}

	public void setDead(int i)
	{
		if (i == 1)
			leftAlive = false;
		else if (i == 2)
			rightAlive = false;
		else
			headAlive = false;
	}

	IEnumerator vulnerable()
	{
		HeadBehaviour headScript = head2.GetComponent<HeadBehaviour> ();
		sounds [2].Play ();
		headScript.setVulnerable ();
		yield return new WaitForSeconds (5f);
		if (headAlive)
			headScript.resetHead ();
		yield return new WaitForSeconds (2f);
		if (headAlive) {
			Instantiate (left, leftPos.position, left.transform.rotation);
			sounds [3].Play ();
		}
		yield return new WaitForSeconds (0.5f);
		if (headAlive) {
			Instantiate (right, rightPos.position, right.transform.rotation);
			sounds [3].Play ();
		}
		yield return new WaitForSeconds (0.5f);

	}

	void Update () 
	{
		if (!leftAlive && !rightAlive) 
		{
			leftAlive = true;
			rightAlive = true;
			StartCoroutine (vulnerable ());
		}
		if (!headAlive) 
		{
			GameObject.Find ("GameController").GetComponent<GameController> ().setBossDead();
			Destroy (gameObject);
		}
	}
}
