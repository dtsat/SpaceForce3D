using UnityEngine;
using System.Collections;

public class HeadBehaviour : MonoBehaviour {

	public GameObject shot;
	public GameObject[] damageEffects;
	public Transform[] gun;
	public int hitPoints;
	private Rigidbody rb;

	private float offset;
	public float direction, modifier;

	private bool alive, vulnerable;
	private MeshRenderer sprite;
	public Material hurtface, okface;

	private AudioSource[] sounds;
	public AudioClip explo;

	private UIScript guiFunctions;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		sounds = GetComponents<AudioSource> ();
		sprite = GetComponent<MeshRenderer> ();
		guiFunctions = GameObject.Find ("HUD").GetComponent<UIScript> ();
		alive = true;
		vulnerable = false;
		hitPoints = 50;
		StartCoroutine (fillBar ());
		StartCoroutine (attack ());
	}

	IEnumerator fillBar()
	{
		guiFunctions.showHealth ();
		for (int i = 0; i < 10; i++) 
		{
			guiFunctions.adjustBar (0.6f);
			yield return new WaitForSeconds (0.05f);
		}
	}
	
	void Update()
	{
		offset = direction * Mathf.Sin (modifier);
		modifier += 0.025f;
	}

	void FixedUpdate () 
	{
		transform.Translate (new Vector3 (0.0f, offset, 0.0f) * 5f * Time.deltaTime);
		if (!alive) {
			rb.AddForce(Vector3.down * 5f);
			transform.Rotate (0, 0, 400 * Time.deltaTime);
		}
	}

	void takeDamage()
	{
		if (hitPoints > 0) {
			sounds [0].Play ();
			hitPoints--;
			guiFunctions.adjustBar (-0.12f);
			Instantiate (damageEffects [0], transform.position, transform.rotation);
		} else {
			alive = false;
			GameObject sparks = Instantiate (damageEffects [0], transform.position, transform.rotation) as GameObject;
		}
	}

	public IEnumerator attack()
	{
		while (alive) 
		{
			yield return new WaitForSeconds (Random.Range (5f, 10f));
			if(!vulnerable)	
				sounds [1].Play ();
			yield return new WaitForSeconds (1f);
			for (int j = 0; j < 3; j++) 
			{
				if(!vulnerable)	
					sounds [2].Play ();
				for (int i = 0; i < 30; i++) {
					if (!vulnerable) 
					{	
						Instantiate (shot, gun [0].position, gun [0].rotation);
						Instantiate (shot, gun [1].position, gun [1].rotation);
						yield return new WaitForSeconds (0.01f);
					}
				}
				yield return new WaitForSeconds (1.0f);
			}
		}
	}

	IEnumerator deathExplo()
	{
		for (float i = 5f; i > 0f; i--) 
		{
			Instantiate (damageEffects [1], new Vector3 (transform.position.x, transform.position.y - i, transform.position.z), damageEffects [1].transform.rotation);
			yield return new WaitForSeconds (1f);
		}
	}

	public void setVulnerable()
	{
		vulnerable = true;
		sprite.material = hurtface;
	}

	public void resetHead()
	{
		vulnerable = false;
		sprite.material = okface;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Ground")) 
		{
			AudioSource.PlayClipAtPoint (explo, Camera.main.transform.position);
			StartCoroutine(deathExplo());
			GameObject.FindGameObjectWithTag("BossFight").GetComponent<BossFight>().setDead(0);
			guiFunctions.hideHealth ();
			Destroy (gameObject);
		}
		if (other.CompareTag ("PlayerShot") && vulnerable) {
			if (alive) {
				Destroy (other.gameObject);
				takeDamage ();
			}
		} else if (other.CompareTag ("PlayerShot") && !vulnerable) 
		{
			sounds [3].Play ();
			Destroy (other.gameObject);
		}
	}
}
