using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class ISSSphere : MonoBehaviour {
	
	JSONNode sphereData;

	int secondsValue;
	int durationFlight;
	string d_time;
	string timestampFlight;
	string flightTitle;
	string flightDate;
	string flightTime;
	string flightDuration;

	Vector3 localSc;

	UnityEngine.UI.Text textCityName_new;
	UnityEngine.UI.Text textISSData_1_new;
	UnityEngine.UI.Text textISSData_2_new;
	UnityEngine.UI.Text textISSData_3_new;
	
	CanvasGroup myCanvasGroup_new;

	Canvas myCanvas_new;

	GUIText kreisText_new;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Animation der Kreise
		transform.localScale = Vector3.MoveTowards (transform.localScale, localSc, 0.1f);
	}

	//Setup-Funktion die die Daten aus dem JSON-Objekt holt
	public void Setup(JSONNode newData, int i, int buttonID, 
	                  UnityEngine.UI.Text textCityNameObject, 
	                  UnityEngine.UI.Text textISSDataObject_1,
	               	  UnityEngine.UI.Text textISSDataObject_2, 
	                  UnityEngine.UI.Text textISSDataObject_3,
	                  CanvasGroup myCanvasGroupObject,
	                  Canvas myCanvasObject,
	                  GUIText kreisTextObject) {  

		//CanvasGroup für das Einblenden des Canvas
		myCanvasGroup_new = myCanvasGroupObject;
		myCanvasGroup_new.alpha = 1;

		//Canvas in dem die Datensätze sitzen
		myCanvas_new = myCanvasObject;

		//Nummer des jew. Überfluges auf Kreis
		kreisText_new = kreisTextObject;

		//Name der Stadt und Datensätze als Text ausgeben
		textCityName_new = textCityNameObject;
		
		textISSData_1_new = textISSDataObject_1;
		textISSData_2_new = textISSDataObject_2;
		textISSData_3_new = textISSDataObject_3;

		//Hier werden die Daten des JSON-Objektes zwischengespeichert
		sphereData = newData;		

		//Dauer des Überfluges
		secondsValue = int.Parse(sphereData["duration"]);
		durationFlight = secondsValue;

		string minutes = Mathf.Floor(durationFlight / 60).ToString("0");
		string seconds = Mathf.Floor(durationFlight % 60).ToString("00");

		d_time = minutes + ":" + seconds;

		//Uhrzeit des Überfluges
		timestampFlight = sphereData["risetime"];
		double timestamp = Convert.ToDouble(timestampFlight);

		DateTime timeFull = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		DateTime timestampFull = timeFull.AddSeconds(timestamp);

		//Diese Überflugs-Daten benutzen beide Buttons
		flightTitle = (i + 1) + ". Überflug:";
		flightDuration = "Dauer: " + d_time + " Minuten";

		if (buttonID == 1) {

			//Konvertieren zu MESZ
			DateTime timestampFullFirst = timestampFull.AddHours(2);

			//Hole jeden Datums-Wert einzeln
			string rt_day = Convert.ToString(timestampFullFirst.Day);
			string rt_month = Convert.ToString(timestampFullFirst.Month);
			string rt_year = Convert.ToString(timestampFullFirst.Year);
			string rt_time = Convert.ToString(timestampFullFirst.TimeOfDay);

			flightDate = "Datum: " + rt_day + "." + rt_month + "." + rt_year;
			flightTime = "Uhrzeit: " + rt_time + " MESZ";

			if (i == 0) {
				//Überflug 1
				textISSData_1_new.text = flightTitle + "\n\n" + flightDate + "\n" + flightTime + "\n" + flightDuration;
			}
			
			if (i == 1) {
				//Überflug 2
				textISSData_2_new.text = flightTitle + "\n\n" + flightDate + "\n" + flightTime + "\n" + flightDuration;
			}
			
			if (i == 2) {
				//Überflug 3
				textISSData_3_new.text = flightTitle + "\n\n" + flightDate + "\n" + flightTime + "\n" + flightDuration;
			}

			//Name der Stadt laden
			textCityName_new.text = ISSPasses.geoMarks[1].cityName;

			//Verschiebung der Kreise, damit sie nicht kollidieren
			Vector3 localPos = transform.localPosition;
			localPos.x = localPos.x + 20;
			localPos.x = localPos.x + (i * 30);
			localPos.z = localPos.z + 25;
			
			transform.localPosition = new Vector3 (localPos.x, transform.localPosition.y, localPos.z);

			//Kreis einfärben
			GetComponentInChildren<Renderer>().material.color = new Color(1, 0.7f, 0, 0.5f);

			//Rahmenfarbe des Canvas ändern 
			Outline outlineCanvas;
			outlineCanvas = myCanvas_new.GetComponent<Outline>();
			outlineCanvas.effectColor = new Color(1, 0.7f, 0, 0.5f);

			//Text und Position der Zahl auf jedem Kreis	
			kreisText_new.text = Convert.ToString(i+1) + ".";

			Vector3 locPosKreisText = kreisText_new.transform.localPosition;
			locPosKreisText.x = locPosKreisText.x + 0.026f;
			locPosKreisText.x = locPosKreisText.x + (i * 0.049f);
			locPosKreisText.y = 0.81f;
			locPosKreisText.z = 2;

			kreisText_new.transform.localPosition = new Vector3 (locPosKreisText.x, locPosKreisText.y, locPosKreisText.z);

			//Blende Zahlen zeitverzögert ein
			StartCoroutine(ShowKreise(kreisText_new));

		} else if (buttonID == 2) {

			//Konvertieren zu  EDT
			float hoursOffset = -4f;			
			DateTime timestampFullHoursNY = timestampFull.AddHours(hoursOffset);

			//Hole jeden Datums-Wert einzeln
			string rt_day_new = Convert.ToString(timestampFullHoursNY.Day);
			string rt_month_new = Convert.ToString(timestampFullHoursNY.Month);
			string rt_year_new = Convert.ToString(timestampFullHoursNY.Year);
			string rt_time_new = Convert.ToString(timestampFullHoursNY.TimeOfDay);

			flightDate = "Datum: " + rt_day_new + "." + rt_month_new + "." + rt_year_new;
			flightTime = "Uhrzeit: " + rt_time_new + " EDT";

			if (i == 0) {
				//Überflug 1
				textISSData_1_new.text = flightTitle + "\n\n" + flightDate + "\n" + flightTime + "\n" + flightDuration;
			}
			
			if (i == 1) {
				//Überflug 2
				textISSData_2_new.text = flightTitle + "\n\n" + flightDate + "\n" + flightTime + "\n" + flightDuration;
			}
			
			if (i == 2) {
				//Überflug 3
				textISSData_3_new.text = flightTitle + "\n\n" + flightDate + "\n" + flightTime + "\n" + flightDuration;
			}

			//Name der Stadt laden
			textCityName_new.text = ISSPasses.geoMarks[2].cityName;

			//Verschiebung der Kreise, damit sie nicht kollidieren
			Vector3 localPos = transform.localPosition;
			localPos.x = localPos.x - 170;
			localPos.x = localPos.x + (i * 30);
			localPos.z = localPos.z + 18;
			
			transform.localPosition = new Vector3 (localPos.x, transform.localPosition.y, localPos.z);

			//Kreis einfärben
			GetComponentInChildren<Renderer>().material.color = new Color(0.8f, 0.3f, 0.5f, 0.5f);

			//Rahmenfarbe des Canvas ändern  
			Outline outlineCanvas;
			outlineCanvas = myCanvas_new.GetComponent<Outline>();
			outlineCanvas.effectColor = new Color(0.8f, 0.3f, 0.5f, 0.5f);

			//Text und Position der Zahl auf jedem Kreis	
			kreisText_new.text = Convert.ToString(i+1) + ".";			
			Vector3 locPosKreisText = kreisText_new.transform.localPosition;
			locPosKreisText.x = locPosKreisText.x - 0.282f;
			locPosKreisText.x = locPosKreisText.x + (i * 0.049f);
			locPosKreisText.y = 0.79f;
			locPosKreisText.z = 2;
			
			kreisText_new.transform.localPosition = new Vector3 (locPosKreisText.x, locPosKreisText.y, locPosKreisText.z);

			//Blende Zahlen zeitverzögert ein
			StartCoroutine(ShowKreise(kreisText_new));
		}

		//Skalierung des Kreises
		localSc = transform.localScale;
		localSc.x = localSc.x * secondsValue * 0.04f;
		localSc.z = localSc.z * secondsValue * 0.04f;
	}

	//Funktion um Zahlen auf Kreis zeitverzögert einzublenden
	IEnumerator ShowKreise(GUIText kt) {
		kt.color = new Color(0, 0, 0, 0);

		yield return new WaitForSeconds(6);

		kt.color = new Color(0, 0, 0, 255);

	}
}
