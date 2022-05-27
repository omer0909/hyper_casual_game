using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragEvent : MonoBehaviour, IDragHandler
{
	Transform player;
	[SerializeField]
	GameObject startMenu;
	public float speed = 1;
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		speed /= Screen.width;
	}

	public void OnDrag(PointerEventData data)
	{
		if (!LevelManager.lockGame)
			player.Translate(data.delta.x * speed * 0.1f, 0, data.delta.y * speed * 0.1f);
		if (startMenu.activeSelf)
			startMenu.SetActive(false);
	}
}
