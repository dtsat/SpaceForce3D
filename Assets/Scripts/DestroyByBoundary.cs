using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Enemy"))
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().loseShip ();
		Destroy (other.gameObject, 6f);
	}

}
