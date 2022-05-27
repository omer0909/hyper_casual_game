using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPieces : MonoBehaviour
{
	Transform hole;
	void Awake()
	{
		hole = GameObject.FindWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (!LevelManager.lockGame)
			return;
		float distance = transform.position.z - hole.position.z;
		if (distance < 0.4f)
		{
			transform.Translate(Vector3.down * Time.deltaTime * 20);
			if (transform.position.y < -10)
			{
				if (LevelManager.vibration)
					Vibration.Vibrate(35);
				Destroy(gameObject);
			}
		}
	}
}
