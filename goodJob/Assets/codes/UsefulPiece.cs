
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class UsefulPiece : MonoBehaviour, IPiece
{
	public void taked()
	{
		if (LevelManager.vibration)
			Vibration.Vibrate(35);
	}
	public bool isShouldTake()
	{
		return true;
	}
}