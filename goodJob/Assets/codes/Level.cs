using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Level : MonoBehaviour
{
	public float rateView = 16.7f;
	public Color placeColor, wallColor, groundColor;
	[Space]
	[SerializeField]
	private Material placeMaterial, wallMaterial;
	[SerializeField]
	private Material[] groundMaterials;

	[SerializeField]
	private Transform placesParent;
	public List<GameObject> places;
	public GameObject placeBegin;
	public GameObject place;

	public static float map_size = 70;
	public GameObject placeEnd;
	[Range(2, 10)]
	public int number;

	void getPlaces()
	{
		places.Clear();
		for (int i = 0; i < placesParent.childCount; i++)
			places.Add(placesParent.GetChild(i).gameObject);
	}

	private void Awake()
	{
		getPlaces();
		placeBegin.GetComponent<BoxCollider>().enabled = false;
		for (int i = 0; i < places.Count; i++)
			places[i].GetComponent<BoxCollider>().enabled = false;

		Camera.main.fieldOfView = rateView * (1 / Camera.main.aspect);
		setMaterials();
	}

	void setMaterials()
	{
		placeMaterial.color = placeColor;
		wallMaterial.color = wallColor;
		for (int i = 0; i < groundMaterials.Length; i++)
			groundMaterials[i].color = groundColor;
	}

#if UNITY_EDITOR
	public void updateLevel()
	{
		GameObject tmp;

		setMaterials();
		Camera.main.fieldOfView = rateView * (1 / Camera.main.aspect);
		getPlaces();
		for (int i = 0; i < places.Count; i++)
			DestroyImmediate(places[i]);
		places.Clear();
		float step = map_size;
		for (int i = 2; i < number; i++)
		{
			tmp = Instantiate(place, new Vector3(0, 0, step), Quaternion.identity, placesParent);
			places.Add(tmp);
			step += map_size;
		}

		tmp = Instantiate(placeEnd, new Vector3(0, 0, step), Quaternion.identity, placesParent);
		places.Add(tmp);
	}
#endif
}

#if UNITY_EDITOR

[CustomEditor(typeof(Level))]
public class levelEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (!Application.isPlaying)
		{
			Level myLevel = (Level)target;
			if (GUILayout.Button("update"))
			{
				myLevel.updateLevel();
			}
		}
	}
}
#endif
