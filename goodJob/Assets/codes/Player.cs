using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private LevelManager lm;
	List<Rigidbody> pieces = new List<Rigidbody>();
	[SerializeField]
	private Transform holeCollision;
	float holeRadious = 2.0f;
	float eventHrizonRadious = 5.0f;
	float hideYPos = -5;
	int takedGather = 0;
	int willGather = 0;
	public void levelSetup()
	{
		for (int i = 0; i < pieces.Count; i++)
			pieces[i].gameObject.SetActive(false);
		pieces.Clear();
		GameObject[] tmp = GameObject.FindGameObjectsWithTag("Piece");
		takedGather = 0;
		willGather = 0;
		for (int i = 0; i < tmp.Length; i++)
		{
			if (tmp[i].transform.position.z > transform.position.z + 40)
				continue;
			pieces.Add(tmp[i].GetComponent<Rigidbody>());
			if (tmp[i].GetComponent<IPiece>().isShouldTake())
				willGather++;
		}
	}

	void Awake()
	{
		for (int i = 0; i < pieces.Count; i++)
			pieces[i].isKinematic = true;
		lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
		levelSetup();
	}

	void pieceFell(int i)
	{
		pieces[i].gameObject.GetComponent<IPiece>().taked();
		Destroy(pieces[i].gameObject);
		if (pieces[i].GetComponent<IPiece>().isShouldTake())
			takedGather++;
		lm.sliderSet((float)takedGather / willGather);
		if (takedGather == willGather)
			lm.nextLevel();
		pieces.RemoveAt(i);
	}

	void piecesControl()
	{
		holeCollision.position = transform.position;
		for (int i = 0; i < pieces.Count; i++)
		{
			float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z)
			, new Vector2(pieces[i].transform.position.x, pieces[i].transform.position.z));
			if (distance < eventHrizonRadious && distance >= holeRadious)
			{
				pieces[i].isKinematic = false;
				pieces[i].AddExplosionForce(-10, transform.position, 5);
			}
			if (distance < holeRadious)
				pieces[i].AddForce(Vector3.down * 200);

			if (pieces[i].position.y < hideYPos)
				pieceFell(i);
		}
	}
	void FixedUpdate()
	{
		if (!LevelManager.lockGame)
			piecesControl();
	}
}