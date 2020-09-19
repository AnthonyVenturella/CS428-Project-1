using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;

//Input from openweatermap.com is as follows:
/*{
	"coord": {
		"lon": -87.6,
		"lat": 41.88
	},
	"weather": [{
		"id": 803,
		"main": "Clouds",
		"description": "broken clouds",
		"icon": "04n"
	}],
	"base": "stations",
	"main": {
		"temp": 87.98,
		"feels_like": 88.45,
		"temp_min": 87.01,
		"temp_max": 89.01,
		"pressure": 1003,
		"humidity": 48
	},
	"visibility": 10000,
	"wind": {
		"speed": 9.17,
		"deg": 230
	},
	"clouds": {
		"all": 75
	},
	"dt": 1598663974,
	"sys": {
		"type": 1,
		"id": 4861,
		"country": "US",
		"sunrise": 1598613164,
		"sunset": 1598661039
	},
	"timezone": -18000,
	"id": 4887398,
	"name": "Chicago",
	"cod": 200
}*/

public class weatherAPIScript : MonoBehaviour{

	public string APIKey = "84d195e355ae1f1edaa2ae3273a6937e";

	public GameObject temperatureTextObject;
    public GameObject humidityTextObject;
    public GameObject conditionsTextObject;
    public GameObject windDirTextObject;
    public GameObject windSpeedTextObject;

	public GameObject windSock;
	public GameObject windSockParent;

	public GameObject[] temperatureDiscs;
	public GameObject[] humidityDisks;
	public GameObject[] conditionStates;

	private int _tempDiscCount;
	private int _humidityDiskCount;

	private float _temperature;
	private float _humidity;
	private float _windSpeed;
	private float _windDir;
	private float _mappedSpeed;

	private string _description;
	private string _cardinalDir;

	private int debugCond = 8;

	string url;

    

    void Start() {

        // wait a couple seconds to start and then refresh every 900 seconds
        InvokeRepeating("GetDataFromWeb", 2f, 900f);
		url = ("http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=" + APIKey + "&units=imperial");
	}

    void GetDataFromWeb() {

		//Spawning on a co routine so it don't bog down the runtime
        StartCoroutine(GetRequest(url));
    }

    private void Update() {
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			debugCond++;
			if (debugCond > 8) {
				debugCond = 0;
			}

			unAssignConditions();
			print("right arrow key was pressed, at scene " + debugCond);

			conditionStates[debugCond].gameObject.SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			debugCond--;
			if (debugCond < 0) {
				debugCond = 8;
            }

			unAssignConditions();
			print("left arrow key was pressed, at scene " + debugCond);

			conditionStates[debugCond].gameObject.SetActive(true);
		}
	}

    IEnumerator GetRequest(string uri) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError) {
                Debug.Log(": Error: " + webRequest.error);

			//if feeling lucky add in more catches

            } else {
                // print out the weather data to make sure it makes sense
                string data = webRequest.downloadHandler.text;

				//used in temperature and humidity widget for the inside "liquid"
				unAssignDisks();

				/*May future generations forgive me for this abomination ive created. Mono doesnt have support for JSON parsing natively
				  but has support for XML(??), I dont know anything about XML and I dont want to use a third party library like MiniJSON.cs
				  (https://gist.github.com/darktable/1411710) since its a school project. Soooooo we're going to parse this JSON input 
				  using C# string manipulation (i'm so sorry)*/

				//Splitting input into new array elements after every comma and bracket. Will make a lot of garbage but at least our data
				//	will be in splittable tupple and not a massive string
				string[] subStrings = data.Split(new char[] { ',', '{', '}' });

				//scan through the array of tuples (and garbage) and find what we're looking for.
				foreach (string str in subStrings) {
                    //Debug.Log(str);

                    //the true monstrosity begins here....
                    if (str.StartsWith("\"temp\":")) {
						_temperature = float.Parse(str.Substring(7));

					} else if (str.StartsWith("\"humidity\":")) {
						_humidity = float.Parse(str.Substring(11));

					} else if (str.StartsWith("\"description\":")) {
						_description = str.Substring(14);

					} else if (str.StartsWith("\"speed\":")) {
						_windSpeed = float.Parse(str.Substring(8));

					} else if (str.StartsWith("\"deg\":")) {
						_windDir = float.Parse(str.Substring(6));
					}
				}

				_cardinalDir = getWindCardinalDir(_windDir);

				assignDisks(getPercentRange(_temperature), temperatureDiscs);
				assignDisks(getPercentRange(_humidity), humidityDisks);

				_mappedSpeed = mapWindSpeedRange(_windSpeed);
				windSock.transform.localScale = new Vector3(1.0f, _mappedSpeed, 1.0f);
				windSockParent.transform.Rotate(1.0f, _windDir, 1.0f, Space.Self);

				setIconState(_description);

				temperatureTextObject.GetComponent<TextMeshPro>().text = _temperature + " \u00B0F";
				humidityTextObject.GetComponent<TextMeshPro>().text = _humidity + "% Humidity";
				conditionsTextObject.GetComponent<TextMeshPro>().text = _description;
				windSpeedTextObject.GetComponent<TextMeshPro>().text = _windSpeed + "mph";
				windDirTextObject.GetComponent<TextMeshPro>().text = _cardinalDir;
			}

		}
    }

	private int getPercentRange(float value) {
		if (value > 100) {
			return 10;
		} else if (value > 90 && value <= 100) {
			return 9;
		} else if (value > 80 && value <= 90) {
			return 8;
		} else if (value > 70 && value <= 80) {
			return 7;
		} else if (value > 60 && value <= 70) {
			return 6;
		} else if (value > 50 && value <= 60) {
			return 5;
		} else if (value > 40 && value <= 50) {
			return 4;
		} else if (value > 30 && value <= 40) {
			return 3;
		} else if (value > 20 && value <= 30) {
			return 2;
		} else if (value > 10 && value <= 20) {
			return 1;
		} else {
			return 0;
		}
    }

	private void assignDisks(int disks, GameObject[] diskArray) {
		for(int i = 0; i < disks; i++) {
			diskArray[i].gameObject.SetActive(true);
		}
	}

	private void unAssignDisks() {
		for (int i = 0; i < 10; i++) {
			temperatureDiscs[i].gameObject.SetActive(false);
			humidityDisks[i].gameObject.SetActive(false);
		}
	}

	private void unAssignConditions() {
		for (int i = 0; i < 9; i++) {
			conditionStates[i].gameObject.SetActive(false);
		}
	}


	/*	Assuming wndDir is based off of true north azimuths
			N   =     0 deg		S   =   180 deg
			NNE =  22.5 deg		SSW = 202.5 deg
			NE  =    45 deg		SW  =   225 deg
			ENE =  67.5 deg		WSW = 247.5 deg
			E   =    90 deg		W	=   270 deg
			ESE = 112.5 deg		WNW = 292.5 deg
			SE  =   135 deg		NW	=	315 deg
			SSE = 157.5 deg		NNW = 337.5 deg

			Zones are 11.25 deg to either side of prime IE N is 11.25 - 348.75
	 */
	private string getWindCardinalDir(float dir) {
		if ((dir >= 0 && dir < 11.25) || (dir >= 348.75 && dir < 360)) {
			return "N";

		} else if (dir >= 11.25 && dir < 33.75) {
			return "NNE";

		} else if (dir >= 33.75 && dir < 56.25) {
			return "NE";

		} else if (dir >= 56.25 && dir < 78.75) {
			return "ENE";

		} else if (dir >= 78.75 && dir < 101.25) {
			return "E";

		} else if (dir >= 101.25 && dir < 123.75) {
			return "ESE";

		} else if (dir >= 123.75 && dir < 146.25) {
			return "SE";

		} else if (dir >= 146.25 && dir < 168.75) {
			return "SSE";

		} else if (dir >= 168.75 && dir < 191.25) {
			return "S";

		} else if (dir >= 191.25 && dir < 213.75) {
			return "SSW";

		} else if (dir >= 213.75 && dir < 236.25) {
			return "SW";

		} else if (dir >= 236.25 && dir < 258.75) {
			return "WSW";

		} else if (dir >= 258.75 && dir < 281.25) {
			return "W";

		} else if (dir >= 281.25 && dir < 303.75) {
			return "WNW";

		} else if (dir >= 303.75 && dir < 326.25) {
			return "NW";

		} else if (dir >= 326.25 && dir < 348.75) {
			return "NNW";
		} else {
			return "NaN";
        }
	}

	private float mapWindSpeedRange(float _speed) {

		return _speed * 1.0f / 50;
	}

	//redo this with hashtable when I have time (dont judge me! Call it a hack n slash prototype or something)
	private void setIconState(string description) {

		unAssignConditions();

		//Group "clear sky"
		if (description == "\"clear sky\"") {
			conditionStates[0].gameObject.SetActive(true);
		}

		//group "few clouds"
		else if(description == "\"few clouds\"") {
			conditionStates[1].gameObject.SetActive(true);
		}

		//group "scattered clouds"
		else if (description == "\"scattered clouds\"") {
			conditionStates[2].gameObject.SetActive(true);
		}

		//group "broken clouds"
		else if (description == "\"broken clouds\"" ||
				 description == "\"overcast clouds\"") {
			conditionStates[3].gameObject.SetActive(true);
		}

		//group "shower rain"
		else if (description == "\"light intensity shower rain\"" ||
				 description == "\"shower rain\"" ||
				 description == "\"heavy intensity shower rain\"" ||
				 description == "\"ragged shower rain\"" ||
				 description == "\"light intensity drizzle\"" ||
				 description == "\"drizzle\"" ||
				 description == "\"heavy intensity drizzle\"" ||
				 description == "\"light intensity drizzle rain\"" ||
				 description == "\"drizzle rain\"" ||
				 description == "\"heavy intensity drizzle rain\"" ||
				 description == "\"shower rain and drizzle\"" ||
				 description == "\"heavy shower rain and drizzle\"" ||
				 description == "\"shower drizzle\"") {
			conditionStates[4].gameObject.SetActive(true);
		}

		//group "rain"
		else if (description == "\"light rain\"" ||
				 description == "\"moderate rain\"" ||
				 description == "\"heavy intensity rain\"" ||
				 description == "\"very heavy rain\"" ||
				 description == "\"extreme rain\"" ||
				 description == "\"rain\"") {
			conditionStates[5].gameObject.SetActive(true);
		}

		//group "thunderstorm"
		else if (description == "\"thunderstorm with light rain\"" ||
				 description == "\"thunderstorm with rain\"" ||
				 description == "\"thunderstorm with heavy rain\"" ||
				 description == "\"light thunderstorm\"" ||
				 description == "\"thunderstorm\"" ||
				 description == "\"heavy thunderstorm\"" ||
				 description == "\"ragged thunderstorm\"" ||
				 description == "\"thunderstorm with light drizzle\"" ||
				 description == "\"thunderstorm with drizzle\"" ||
				 description == "\"thunderstorm with heavy drizzle\"") {
			conditionStates[6].gameObject.SetActive(true);
		}

		//group "snow"
		else if (description == "\"light snow\"" ||
				 description == "\"snow\"" ||
				 description == "\"heavy snow\"" ||
				 description == "\"sleet\"" ||
				 description == "\"light shower sleet\"" ||
				 description == "\"shower sleet\"" ||
				 description == "\"light rain and snow\"" ||
				 description == "\"rain and snow\"" ||
				 description == "\"light shower snow\"" ||
				 description == "\"shower snow\"" ||
				 description == "\"heavy shower snow\"" ||
				 description == "\"freezing rain\"") {
			conditionStates[7].gameObject.SetActive(true);
		}

		//group "mist"
		else if (description == "\"mist\"" ||
				 description == "\"smoke\"" ||
				 description == "\"haze\"" ||
				 description == "\"sand/ dust whirls\"" ||
				 description == "\"fog\"" ||
				 description == "\"sand\"" ||
				 description == "\"dust\"" ||
				 description == "\"volcanic ash\"" ||
				 description == "\"squalls\"" ||
				 description == "\"tornado\"") {
			conditionStates[8].gameObject.SetActive(true);
		}

		else {
			conditionStates[0].gameObject.SetActive(true);
		}
	}

}