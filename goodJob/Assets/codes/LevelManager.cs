using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	Text vibrationText;
	public static bool vibration = true;
	public static int money;
	Transform door;
	[SerializeField]
	ParticleSystem confetti;
	[SerializeField]
	GameObject killedMenu;
	[SerializeField]
	private Level level;
	public static bool lockGame = false;
	[SerializeField]
	private GameObject[] themes;
	private List<GameObject> activeThemes = new List<GameObject>();
	bool goCenter = false;
	bool sleeping = false;
	Transform hole;
	[SerializeField]
	private float speed = 5;
	Vector3 first_campos;
	Vector3 old_campos;
	Vector3 old_holepos;
	Vector3 first_holepos;
	Transform cam;
	float timer = 0;
	float timerLate = 0;
	[SerializeField]
	private Place place;
	[SerializeField]
	Text moneyViewer;
	[SerializeField]
	RectTransform sliderArea;
	[SerializeField]
	GameObject slider;

	List<GameObject> sliders = new List<GameObject>();
	int activeLevel = 0;

	public void sliderSet(float value)
	{
		sliders[activeLevel].transform.GetChild(0).localScale = new Vector3(value, 1, 1);
	}

	void setSliders()
	{
		float xSize = 1.0f / level.number;
		for (int i = 0; i < level.number; i++)
		{
			GameObject tmp = Instantiate(slider, Vector3.zero, Quaternion.identity, sliderArea);
			tmp.transform.localPosition = Vector3.zero + Vector3.right * (xSize * i * sliderArea.sizeDelta.x - sliderArea.sizeDelta.x * 0.5f);
			tmp.transform.localScale = new Vector3(xSize * 0.9f, 1, 1);
			sliders.Add(tmp);
		}
	}

	public void vibrationButton()
	{
		vibration = !vibration;
		PlayerPrefs.SetInt("vibration", vibration ? 1 : 0);
		vibrationText.text = vibration ? "yes" : "no";
	}

	void getDoor()
	{
		GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
		float minZ = 1000;
		for (int i = 0; i < doors.Length; i++)
		{
			if (doors[i].transform.position.z < minZ)
			{
				door = doors[i].transform;
				minZ = door.position.z;
			}
		}
	}

	void setTheme(int active)
	{
		Transform levelT;
		GameObject tmp;

		for (int i = 0; i < activeThemes.Count; i++)
			Destroy(activeThemes[i]);
		activeThemes.Clear();

		levelT = level.transform;
		tmp = GameObject.Instantiate(themes[active], levelT.position, Quaternion.identity, levelT);
		activeThemes.Add(tmp);
		tmp = GameObject.Instantiate(themes[active], levelT.position - Vector3.forward * Level.map_size, Quaternion.identity, levelT);
		activeThemes.Add(tmp);
		for (int i = 0; i < level.places.Count; i++)
		{
			levelT = level.places[i].transform;
			tmp = GameObject.Instantiate(themes[active], levelT.position, Quaternion.identity, levelT);
			activeThemes.Add(tmp);
		}
		Destroy(tmp.transform.GetChild(0).gameObject);
		tmp = GameObject.Instantiate(themes[active], levelT.position + Vector3.forward * Level.map_size, Quaternion.identity, levelT);
		activeThemes.Add(tmp);
	}
	void Awake()
	{
		activeLevel = 0;
		if (!PlayerPrefs.HasKey("level"))
			PlayerPrefs.SetInt("level", 0);
#if !UNITY_EDITOR
		if (PlayerPrefs.GetInt("level") != SceneManager.GetActiveScene().buildIndex)
			SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
#endif
		if (!PlayerPrefs.HasKey("theme"))
			PlayerPrefs.SetInt("theme", 0);

		if (!PlayerPrefs.HasKey("money"))
			PlayerPrefs.SetInt("money", 100);

		if (!PlayerPrefs.HasKey("vibration"))
			PlayerPrefs.SetInt("vibration", 1);

		setTheme(PlayerPrefs.GetInt("theme"));
		money = PlayerPrefs.GetInt("money");
		vibration = PlayerPrefs.GetInt("vibration") == 1;
		vibrationText.text = vibration ? "yes" : "no";

		moneyViewer.text = money.ToString();
		lockGame = false;
		hole = GameObject.FindWithTag("Player").transform;
		cam = Camera.main.transform;
		first_campos = cam.position;
		first_holepos = hole.position;
		setSliders();
	}

	public void setThemeIndex(int index)
	{
		PlayerPrefs.SetInt("theme", index);
		setTheme(index);
	}

	void cutScene()
	{
		if (goCenter)
		{
			hole.position = Vector3.MoveTowards(hole.position, new Vector3(0, hole.position.y, hole.position.z), Time.deltaTime * speed);
			door.Translate(Vector3.down * Time.deltaTime * 10);

			if (Mathf.Approximately(hole.position.x, 0))
			{
				Destroy(door.gameObject);
				goCenter = false;
				old_holepos = hole.position;
				timer = 0;
				timerLate = 0;
			}
		}
		else if (timer <= 1)
		{
			hole.position = Vector3.Lerp(old_holepos, first_holepos, timer);
			timer += Time.deltaTime * 0.2f;
		}
	}

	void Update()
	{
		if (!sleeping && lockGame)
			cutScene();
	}

	private void LateUpdate()
	{
		if (!sleeping && lockGame && !goCenter)
		{
			cam.position = Vector3.Lerp(old_campos, first_campos, timerLate);
			timerLate += Time.deltaTime * 0.2f;
			if (timerLate > 1)
			{
				hole.GetComponent<Player>().levelSetup();
				lockGame = false;
			}
		}
	}

	IEnumerator confettiPlay()
	{
		yield return new WaitForSeconds(5);
		restartLevel();
	}

	public void nextLevel()
	{
		sliders[activeLevel].transform.GetChild(0).localScale = Vector3.one;
		activeLevel++;
		if (activeLevel == level.number)
		{
			money += 100;
#if !UNITY_EDITOR
			PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
#endif
			PlayerPrefs.SetInt("money", money);
			moneyViewer.text = money.ToString();
			sleeping = true;
			lockGame = true;
			confetti.Play();
			StartCoroutine(confettiPlay());
			return;
		}
		first_campos.z += Level.map_size;
		first_holepos.z += Level.map_size;
		old_campos = cam.position;
		goCenter = true;
		lockGame = true;
		place.first_pos.z += Level.map_size;
		getDoor();
	}
	public void killed()
	{
		sleeping = true;
		lockGame = true;
		cam.GetComponent<CameraShake>().Shake(5, 0.5f);
		killedMenu.SetActive(true);
	}
	public void restartLevel()
	{
		SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
	}
}
