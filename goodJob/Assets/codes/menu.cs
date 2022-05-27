using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
	[SerializeField]
	float marketMenuSpeed = 0.5f;
	[SerializeField]
	RectTransform marketTransform;
	[SerializeField]
	AnimationCurve marketAnimation;
	float marketValue = 0;
	bool marketOpen = false;
	float yMarketPos;
	public void MarketButton(bool isOpen)
	{
		marketOpen = isOpen;
	}

	void Start()
	{
		yMarketPos = marketTransform.position.y;
	}

	void Update()
	{
		if ((!marketOpen && marketValue > 0) || (marketOpen && marketValue < 1))
		{
			marketValue += Time.deltaTime * marketMenuSpeed * (marketOpen ? 1 : -1);
			float curveOut = marketAnimation.Evaluate(marketValue);
			marketTransform.position = new Vector2(marketTransform.position.x, Mathf.Lerp(-marketTransform.sizeDelta.y * 0.5f, yMarketPos, curveOut));
			marketTransform.gameObject.SetActive(true);
		}
	}
}
