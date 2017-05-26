﻿using RAGE.Analytics;
using RAGE.Analytics.Storages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsApp : MonoBehaviour {

	public GameObject confirmExitPanel;

	// Use this for initialization
	void Start () {
		if (confirmExitPanel)
		{
			confirmExitPanel.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ExitButton()
	{
		if (confirmExitPanel)
		{
			confirmExitPanel.SetActive(true);
		} else
		{
			ExitGameConfirmed();
		}
	}

	public void ExitGameConfirmed()
	{
		//Application.Quit();
		if (PlayerPrefs.HasKey("LimesurveyToken") && PlayerPrefs.GetString("LimesurveyToken") != "ADMIN" && PlayerPrefs.HasKey("LimesurveyPost"))
		{
			string path = Application.persistentDataPath;

			if (!path.EndsWith("/"))
			{
				path += "/";
			}

			Dictionary<string, string> headers = new Dictionary<string, string>();

			Net net = new Net(this);

			WWWForm data = new WWWForm();

			data.AddField("token", PlayerPrefs.GetString("LimesurveyToken"));
			data.AddBinaryData("traces", System.Text.Encoding.UTF8.GetBytes(System.IO.File.ReadAllText(Tracker.T.RawFilePath)));

			//d//ata.headers.Remove ("Content-Type");// = "multipart/form-data";

			net.POST(PlayerPrefs.GetString("LimesurveyHost") + "classes/collector", data, new SavedTracesListener());

			System.IO.File.AppendAllText(path + PlayerPrefs.GetString("LimesurveyToken") + ".csv", System.IO.File.ReadAllText(Tracker.T.RawFilePath));
			PlayerPrefs.SetString("CurrentSurvey", "post");

			//POST-TEST
			Invoke("ChangeLevel", 1);
		}
		else
			Application.Quit();

		if (Application.isWebPlayer == false && Application.isEditor == false){
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
	}

	void ChangeLevel()
	{
		SceneManager.LoadScene(31);
	}

	public void ExitGameCanceled()
	{
		confirmExitPanel.SetActive(false);
	}

	class SavedTracesListener : Net.IRequestListener
	{
		public void Result(string data)
		{
			Debug.Log("------------------------");
			Debug.Log(data);
		}

		public void Error(string error)
		{
			Debug.Log("------------------------");
			Debug.Log(error);
		}
	}
}

