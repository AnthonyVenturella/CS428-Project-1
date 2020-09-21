# CS428-Project-1

## Built using:
-	[Unity 2019.4.1f1](https://unity3d.com/unity/whats-new/2019.4.1)
-	[Vuforia 9.3.3](https://developer.vuforia.com/downloads/sdk)

Above are the supported versions for code within this project.

Each widget is controlled by a unique mars database fiducial marker, click links below to download them. They can be printed out or used displayed on a device (printed for best results).
 - Astronaut, Drone, Fissure, Oxygen makers: https://www.evl.uic.edu/aej/428/20files/4%20mars%20markers.png
 - Mars and Database Markers:
https://www.evl.uic.edu/aej/428/20files/2%20more%20mars%20markers.png


###### To Run on PC/Laptop:
>Requires Unity 2019.4.1f1 (or compatible) to be installed on device to view and edit project (linked above). Vuforia 9.3.3 is installed and setup already in the project so there is no need to install it. To run, open the project up in unity, and make sure you are in scene “SampleScene”. Click on the “Weather Manager” child under the “Managers” game object in the hierarchy. Insert your API key from openweathermap.com replacing my key in the “API KEY” field in the inspector. You will need a webcam attached to your pc and image targets printed out from Vuforia (Astronaut, Drone, Fissure, Oxygen, And Mars) click on the play button to run in editor. Bring the image targets up to the webcam and you should see the widgets appear with current information about local time/date and weather information for Chicago.

###### To Run on Android:
>Included with the git repo is an .apk file in the “Android Build” folder. Install this on your android device and run. You will need image targets printed out from Vuforia (Astronaut, Drone, Fissure, Oxygen, And Mars), bring your devices camera over these image targets and you should see the widgets appear with current information about local time/date and weather information for Chicago. To rebuild follow the above steps in “To Run on PC/Laptop” (including replacing my API key) and instead of running in editor go to File > Build settings select platform Android (android module must be installed and setup for your unity version) and select build. It will build you an .apk file to a location you chose, transfer that to your device and run.


## Resources:
###### sounds used:
- http://soundbible.com/340-Bird-Song.html
- http://soundbible.com/2006-Bird-In-Rain.html
- http://soundbible.com/547-Birds-In-Forest.html
- http://soundbible.com/2065-Rain-Inside-House.html
- http://soundbible.com/1818-Rainforest-Ambience.html
- http://soundbible.com/1661-Sunny-Day.html
- http://soundbible.com/1913-Thunder-HD.html
- http://soundbible.com/540-Tree-Frogs-And-Birds.html
- http://soundbible.com/1247-Wind.html
- http://soundbible.com/280-Woodpecker-Tapping-Tree.html

###### lightning effect based on:
- https://www.youtube.com/watch?v=ikkr5jVJfFY
