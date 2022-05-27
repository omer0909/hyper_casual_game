
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class HarmfulPiece : MonoBehaviour, IPiece
{
	LevelManager lm;
	private void Start()
	{
		lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	}
	public bool isShouldTake()
	{
		return false;
	}
	public void taked()
	{
		lm.killed();
	}
}