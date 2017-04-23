﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Publication : MonoBehaviour {

	public Text author;
	public Text comment;
	public Text likesText;
	public Text commentsNumber;

	private float commentN;

	private float likes;

	public RectTransform commentsContent;
	public GameObject commentPrefab;

	public Text textTemplate;
	private RectTransform rectTextTemplate;

	// Use this for initialization
	void Start () {
		likes = 0;
		commentN = 0;
		rectTextTemplate = textTemplate.GetComponent<RectTransform>(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddLikes(float likes = 1)
	{
		this.likes += likes;
		likesText.text = ""+this.likes;
	}

	public void SetAuthorAndComment(string author, string comment)
	{
		this.author.text = author;
		this.comment.text = comment;
	}

	public void AddComment(string from, string text)
	{
		commentN++;
		commentsNumber.text = "" + commentN;
		float height = 0;
		float last = 0;
		foreach (SocialComment child in commentsContent.GetComponentsInChildren<SocialComment>(false))
		{
			RectTransform rctT = child.GetComponent<RectTransform>();
			height += rctT.sizeDelta.y;
			last = rctT.sizeDelta.y;
		}

		GameObject comment = Instantiate(commentPrefab, commentsContent, false);
		SocialComment sc = comment.GetComponent<SocialComment>();
		sc.textPrefab = textTemplate;
		sc.SetAuthorAndComment(from, text);
		comment.transform.localScale = new Vector3(1, 1, 1);
		commentsContent.sizeDelta = new Vector2(commentsContent.sizeDelta.x, commentsContent.sizeDelta.y + last);

		comment.transform.localPosition = new Vector2(comment.transform.localPosition.x, comment.transform.localPosition.y - height);
	}
}
