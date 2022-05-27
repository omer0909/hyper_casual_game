using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
	[SerializeField]
	private Material floor;
	private Transform hole;
	public static Vector2 area_size = new Vector2(18.2f / 2, 35.0f);

	public Vector3 first_pos;
	float addUv = 0.2142857142f;
	private void Awake()
	{
		hole = GameObject.FindWithTag("Player").transform;
		first_pos = hole.position;
	}

	void my_collision()
	{
		Vector2 pos = new Vector2(hole.position.x, hole.position.z);
		if (pos.x > area_size.x + first_pos.x)
			pos.x = area_size.x + first_pos.x;
		if (pos.x < first_pos.x - area_size.x)
			pos.x = first_pos.x - area_size.x;
		if (pos.y > area_size.y + first_pos.z)
			pos.y = area_size.y + first_pos.z;
		if (pos.y < first_pos.z)
			pos.y = first_pos.z;
		hole.position = new Vector3(pos.x, 0, pos.y);
	}

	void Update()
	{
		if (!LevelManager.lockGame)
			my_collision();
		floor.mainTextureOffset = new Vector2((hole.position.x - first_pos.x) * (-0.2f / 14) - 0.1308f, ((hole.position.z - first_pos.z) * (-0.2f / 14) - addUv) % 14);
	}
}
