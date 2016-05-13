using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ISSPasses : MonoBehaviour {

	public GameObject kreisPrefab;
	
	public int buttonID = 0;

	//Text field for city name
	public UnityEngine.UI.Text textCityName;
	//One text field for each pass
	public UnityEngine.UI.Text textIssData1;
	public UnityEngine.UI.Text textIssData2;
	public UnityEngine.UI.Text textIssData3;

	//Eine Gruppe, um die Text-Box einzufaden
	public CanvasGroup myCanvasGroup;

	//Gruppe für Intro-Box
	public CanvasGroup canvasGroupIntro;

	//Variable für Überflugs-Nummer
	public GameObject numberPrefab;

	//Variable für Canvas-Box
	public Canvas myCanvas;

	//Klasse für Geodaten
	public class GeoData{
		public float latitude;
		public float longitude;
		public string cityName;
	}

	public static Dictionary<int, GeoData> geoMarks = new Dictionary<int, GeoData>(){
		{1, new GeoData{latitude=49.757924f, longitude=6.647057f, cityName="Trier"} },
		{2, new GeoData{latitude=40.711120f, longitude=-74.006632f, cityName="New York"} }
	};

	//Variablen für Klickzahl Buttons
	public int button1Pressed = 0;
	public int button2Pressed = 0;

	// Use this for initialization
	void Start () {
		textCityName.text = "";
		textIssData1.text = "";
		textIssData2.text = "";
		textIssData3.text = "";

		//Datenbox bei Start verstecken
		myCanvasGroup.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Calculate(int id){

		//Intro Box nach Klick ausblenden
		canvasGroupIntro.alpha = 0;

		//Felder leeren wg. Problemen mit Darstellung
		textIssData1.text = "";
		textIssData2.text = "";
		textIssData3.text = "";

		//Button ID festlegen
		buttonID = id;

		StartCoroutine (GetJSON ("http://api.open-notify.org/iss-pass.json?lat=" + geoMarks [id].latitude.ToString () + "&lon=" + geoMarks [id].longitude.ToString () + "&n=3"));

	}

	//JSON-Datei parsen
	void ParseJSON(string jsonData){

		var issData = JSONNode.Parse(jsonData);

		if (issData ["response"].Count > 0) {
			for(int i = 0; i<issData["response"].Count; i++){

				//Wir erstellen eine Instanz des Prefabs für den Kreis
				GameObject kreis;
				kreis = Instantiate(kreisPrefab) as GameObject;
				ISSSphere sphereX;
				sphereX = kreis.GetComponent<ISSSphere>();

				//Instanz Prefab für Text(Nummer) auf Kreis
				GameObject numberFlight;
				numberFlight = Instantiate(numberPrefab) as GameObject;
				GUIText kreisText;
				kreisText = numberFlight.GetComponent<GUIText>();

				//Wir übergeben Daten an Setup-Funktion
				sphereX.Setup(issData["response"][i], i, buttonID, textCityName, textIssData1, textIssData2, textIssData3, myCanvasGroup, myCanvas, kreisText);
			}
		} else {
			textIssData1.text = "Derzeit keine sichtbaren Überflüge.";
		}
	}

	IEnumerator GetJSON(string url){
		WWW www = new WWW (url);
		yield return www;

		ParseJSON (www.text);
	}
}
