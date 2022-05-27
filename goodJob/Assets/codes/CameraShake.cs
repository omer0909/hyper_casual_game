using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	float force;
	float time;
	Vector3 oldPos;
	Vector3 oldLook;
	Quaternion oldAngle;

	private void Awake()
	{
		oldPos = transform.position;
		oldAngle = transform.rotation;
		time = -2;
	}
	public void Shake(float force, float time)
	{
		oldLook = transform.forward * 10;
		oldPos = transform.position;
		oldAngle = transform.rotation;
		this.force = force;
		this.time = time;
	}
	void LateUpdate()
	{
		if (time > 0)
		{
			transform.position = Vector3.Lerp(transform.position, oldPos + Random.insideUnitSphere * force * time, Time.deltaTime);
			Quaternion randomRotation = Quaternion.LookRotation(oldLook + Random.insideUnitSphere * force * time, Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, randomRotation, Time.deltaTime);
			time -= Time.deltaTime;
		}
		else if (time > -1)
		{
			transform.position = Vector3.Lerp(transform.position, oldPos, Time.deltaTime * 2);
			transform.rotation = Quaternion.Lerp(transform.rotation, oldAngle, Time.deltaTime * 2);
			time -= Time.deltaTime;
		}
	}
}
