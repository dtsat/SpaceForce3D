using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public IEnumerator Shake(float mag, float dur)
	{
		float elapsed = 0.0f;
		float duration = dur;
		float magnitude = mag;

		Vector3 originalCamPos = new Vector3 (0.0f, 1.0f, -7f);

		while (elapsed < duration) {
			elapsed += Time.deltaTime;

			float percentComplete = elapsed / duration;
			float damper = 1.0f - Mathf.Clamp (4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			float x = Random.value * 2f - 1.0f;
			float y = Random.value * 2f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			transform.position = new Vector3 (x, y, originalCamPos.z);

			yield return null;
		}

		transform.position = originalCamPos;
	}
}
