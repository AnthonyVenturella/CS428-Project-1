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

    public GameObject temperatureTextObject;
    public GameObject humidityTextObject;
    public GameObject conditionsTextObject;
    public GameObject windDirTextObject;
    public GameObject windSpeedTextObject;

	private float _temperature;
	private float _humidity;
	private float _windSpeed;
	private float _windDir;

	private string _description;
	private string _cardinalDir;

	string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=84d195e355ae1f1edaa2ae3273a6937e&units=imperial";

    

    void Start() {

        // wait a couple seconds to start and then refresh every 900 seconds
        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void GetDataFromWeb() {

		//Spawning on a co routine so it don't bog down the runtime
        StartCoroutine(GetRequest(url));
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
                //Debug.Log(":\nReceived: " + data);

				/*May future generations forgive me for this abomination ive created. Mono doesnt have support for JSON parsing natively
				  but has support for XML(??), I dont know anything about XML and I dont want to use a third party library like MiniJSON.cs
				  (https://gist.github.com/darktable/1411710) since its a school project. Soooooo we're going to parse this JSON input 
				  using C# string manipulation (i'm so sorry)*/

				//Splitting input into new array elements after ever comma and bracket. Will make a lot of garbage but at least our data
				//	will be in splittable tupple and not a massive string
				string[] subStrings = data.Split(new char[] { ',', '{', '}' });

				//scan through the array of tuples (and garbage) and find what we're looking for.
				foreach (string str in subStrings) {
                    //Debug.Log(str);

                    //the true monstrosity begins here....
                    if (str.StartsWith("\"temp\":")) {
						_temperature = float.Parse(str.Substring(7));
						//Debug.Log("Temperature: " + _temperature);

					} else if (str.StartsWith("\"humidity\":")) {
						_humidity = float.Parse(str.Substring(11));
						//Debug.Log("Humidity: " + _humidity);

					} else if (str.StartsWith("\"description\":")) {
						_description = str.Substring(14);
						//Debug.Log("Description: " + _description);

					} else if (str.StartsWith("\"speed\":")) {
						_windSpeed = float.Parse(str.Substring(8));
						//Debug.Log("Speed: " + _windSpeed);

					} else if (str.StartsWith("\"deg\":")) {
						_windDir = float.Parse(str.Substring(6));
						//Debug.Log("Direction: " + _windDir);
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

				//continuing the "wtf man" code
				if((_windDir >= 0 && _windDir < 11.25) || (_windDir >= 348.75 && _windDir < 360)) {
					_cardinalDir = "N";

                } else if (_windDir >= 11.25 && _windDir < 33.75) {
					_cardinalDir = "NNE";

				} else if (_windDir >= 33.75 && _windDir < 56.25) {
					_cardinalDir = "NE";

				} else if (_windDir >= 56.25 && _windDir < 78.75) {
					_cardinalDir = "ENE";

				} else if (_windDir >= 78.75 && _windDir < 101.25) {
					_cardinalDir = "E";

				} else if (_windDir >= 101.25 && _windDir < 123.75) {
					_cardinalDir = "ESE";

				} else if (_windDir >= 123.75 && _windDir < 146.25) {
					_cardinalDir = "SE";

				} else if (_windDir >= 146.25 && _windDir < 168.75) {
					_cardinalDir = "SSE";


				} else if (_windDir >= 168.75 && _windDir < 191.25) {
					_cardinalDir = "S";

				} else if (_windDir >= 191.25 && _windDir < 213.75) {
					_cardinalDir = "SSW";

				} else if (_windDir >= 213.75 && _windDir < 236.25) {
					_cardinalDir = "SW";

				} else if (_windDir >= 236.25 && _windDir < 258.75) {
					_cardinalDir = "WSW";

				} else if (_windDir >= 258.75 && _windDir < 281.25) {
					_cardinalDir = "W";

				} else if (_windDir >= 281.25 && _windDir < 303.75) {
					_cardinalDir = "WNW";

				} else if (_windDir >= 303.75 && _windDir < 326.25) {
					_cardinalDir = "NW";

				} else if (_windDir >= 326.25 && _windDir < 348.75) {
					_cardinalDir = "NNW";

				}

				temperatureTextObject.GetComponent<TextMeshPro>().text = _temperature + " \u00B0F";
				humidityTextObject.GetComponent<TextMeshPro>().text = _humidity + "% Humidity";
				conditionsTextObject.GetComponent<TextMeshPro>().text = _description;
				windSpeedTextObject.GetComponent<TextMeshPro>().text = _windSpeed + "mph";
				windDirTextObject.GetComponent<TextMeshPro>().text = _cardinalDir;
			}

		}
    }

}