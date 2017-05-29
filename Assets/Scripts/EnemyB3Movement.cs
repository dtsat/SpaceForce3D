using UnityEngine;
using System.Collections;

public class EnemyB3Movement : MonoBehaviour {

	public float shipSpeed;

	public float tilt;

	private Rigidbody rb;

	public Transform smoke;
	public Transform[] guns;

	public GameObject[] damageEffects;

	private bool alive;

	public GameObject shot;

	public int hitpoints;

	private AudioSource[] sounds;
	public AudioClip explo;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		sounds = GetComponents<AudioSource> ();
		alive = true;
		rb.AddForce (Vector3.right * shipSpeed*1.75f);
		hitpoints = 50;
		StartCoroutine (attack ());
	}

	void takeDamage()
	{
		if (hitpoints > 0) {
			sounds [0].Play ();
			hitpoints--;
			Instantiate (damageEffects [3], smoke.position, smoke.rotation);
		} else {
			alive = false;
			sounds [1].Play ();
			GameObject sparks = Instantiate (damageEffects [0], smoke.position, smoke.rotation) as GameObject;
			sparks.transform.SetParent (smoke);
			sparks.transform.localScale = new Vector3 (3f, 3f, 3f);
			GameObject smokes = Instantiate (damageEffects [1], smoke.position, smoke.rotation) as GameObject;
			smokes.transform.SetParent (smoke);
			smokes.transform.localScale = new Vector3 (3f, 3f, 3f);
			rb.AddExplosionForce (1f, new Vector3 (transform.position.x - Random.Range (-1f, 1f), transform.position.y, transform.position.z), 2f, 2f, ForceMode.Impulse);

		}
	}


	void FixedUpdate () 
	{ 
		if(!alive)
		{
			rb.AddForce (Vector3.down*10f);
			transform.Rotate (0, 0, 250 * Time.deltaTime);
		}
	}

	IEnumerator attack()
	{
		
		while (transform.position.x <= 0)
			yield return null;
		rb.AddForce (Vector3.left * shipSpeed*1.75f);		
		yield return new WaitForSeconds (Random.Range (1f, 3f));
		while (alive) 
		{
			for (int i = 0; i < 4; i++) 
			{
				if (alive) {
					sounds [2].Play ();
					yield return new WaitForSeconds (0.1f);
					sounds [2].Play ();
					Instantiate (shot, guns [0].position, guns [0].rotation);
					Instantiate (shot, guns [1].position, guns [1].rotation);
					yield return new WaitForSeconds (0.75f);
				}
			}
			yield return new WaitForSeconds (3f);
		}
	}



	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) 
		{
			AudioSource.PlayClipAtPoint (explo, Camera.main.transform.position);
			Instantiate (damageEffects[2], new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), damageEffects[2].transform.rotation);
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().killShip (1000);
			Destroy (gameObject);
		}
		if (other.CompareTag ("PlayerShot")) 
		{
			Destroy (other.gameObject);
			takeDamage ();
		}
		if (other.CompareTag ("Player")) 
		{
			takeDamage ();
			rb.AddExplosionForce (1f, new Vector3(transform.position.x - Random.Range(-1f, 1f), transform.position.y, transform.position.z), 1f, 1f, ForceMode.Impulse);
		}
	}
}
