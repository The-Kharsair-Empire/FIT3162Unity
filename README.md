# FIT3162Unity
FIT3162 Project Unity

How to run our project

End User Guide:

Basic button uses:
	Touchpad on the top of the controller: used to activate the pointer. This is done by simply resting your thumb upon the touchpad. Important note: the pointer is used to select from a distance, and should not be used when the controller is inside of an object. In the case that the controller is inside an object you can simply grab it without using the pointer

	Trigger: the trigger is used to grab objects, to either be moved or rescaled. Grabbing with a single controller allows translation and rotation, while grabbing with two controllers allows scaling. The trigger is also used to select objects with the pointer, such as states on the country map and nodes on the state maps.

	Grip button: the grip button is used to create a miniature copy of the country map that can be moved and rotated, and contains all the details on the presented path. This button only activates once an origin and destination node have been specified

Basic workflow:
Once the program has been started, you should be presented with a map of the USA with states delineated. At this stage the only action that can be taken is to select the origin state from which the package flow you want to explore starts. A copy of the selected state will then appear to the left of the starting area, with all the nodes you can select displayed. You can still select another state at this stage by simply reselecting on the country map.
Once you select a node on the state map you can still change your selected node on the state map just in case you accidentally clicked the wrong one, but the state map will be locked. You can go back to selecting a different origin state at anytime during the running of the program by simply grabbing the origin state with the trigger and throwing it away. Everytime you select a node on the state map, you will see all the OD links that come from the selected node displayed on the country map, along with a detailed text pop-up that describes the selected node, giving information such as facility type, name and SLIC code.
Once you are satisfied with the selected origin node, you can then select the destination state from the country map, again using the pointer and trigger. A pop-up copy of the selected state will appear to the right of the starting area, with the facilities that can be reached from the selected origin facility displayed as nodes on this state map. In the case no nodes are displayed, the selected origin facility can not send packages to your selected destination state. If you wish to reselect the destination state, you can simply select a different state on the country map given you have not yet specified a destination node. If you have, you can still simply grab and throw away the destination state and select a different one.
After selecting a destination node, the displayed OD links on the country map will be bound the the single one going from the selected origin to destination facility, and the details on the destination facility will be displayed on the destination state. At this stage you can squeeze the grip button to create a miniature copy of the country map just above your controller. This mini country map will display the OD link, as well as the underlying paths and text detailing the path and selected facilities will be displayed below the map for later comparison. This minimap will stay where you leave it regardless of how you interact with the rest of the program, but if you wish to delete it simply follow the same method used on the states of grabbing the object and throwing it away.



Technical User/ set-up Guide:
Programs required:
Unity (we use version 2018.3.2f1)   only support higher version 
VRTK (version 3.3.0) this version only support unity version 5.6.5 or above
steamVR (version 2.4.5 (sdk 1.7.15)) only support unity version 5.4.6 or higher

Scripts and prefabs required:
Arcs.cs
Bezier.cs
checkLabel.cs
DNodeInteraction.cs
Instruction.cs
Monitor.cs
Node.cs
ONodeInteraction.cs
PointerListener.cs
Preprocessor.cs
StateInteraction.cs
volumeInfo.cs
Sphere.prefab
copyAmerica.prefab
Floating text.prefab
Text Canvas.prefab
[VRTK_SDKManager]
[VRTK_Scripts]
[ExampleSceneScripts]

Data preprocessing:
Prepare your dataset (vol2.csv, pth.csv, slc2.csv, fac2.csv, lat2.csv), which has to have the same name and columns as provided by the UPS, if not, correct your csv files.
Install R studio version 1.2.5001, Install R Language version 3.5.1.
Run preprocessing.R in R studio, it will produce the arc_info.csv and node_info.csv under the same directory.

Installing our program/ Set-up procedure:
Our program comes in a single folder with a build already instantiated in Unity’s hierarchy, so the program can immediately be run. In the case that the build is cleared for some reason, the following instructions can be followed to get it up and running from the basic scripts and prefabs in the folder (specified above):

Drag the copyAmerica.prefab and Text Canvas.prefab into the scene, placing the canvas above the map. 
Create two empty objects called spawnPos_o and spawnPos_d, and place them where you wish the origin state pop up and destination state pop up to spawn respectively.
Create an empty game object and name it Global, and add the custom Preprocessor script and the Monitor script using add component. In the Preprocessor script:
Drag node_info from the data folder into the section labelled Node_data
Drag arc_info from the data folder into the section labelled Arc_data
Drag the Sphere prefab from the prefab folder into the section labeled Node prefab
Drag the previously created spawnPos_o and spawnPos_d from the hierarchy into the sections labeled SpawnPos_o and SpawnPos_d respectively
Drag the Text Canvas.prefab from the prefab folder into the section labelled Instruction canvas
Drag the floating text.prefab from the prefab folder into the section labelled Floating Text Prefab
In the Monitor script drag the floating text.prefab and copyAmerica prefab from the prefab folder into the sections labelled Floating Text Prefab and U Sprefab respectively.
Drag in [VRTK_SDKManager], [VRTK_Scripts] and [ExampleSceneScripts] into the hierarchy from the VRTK folder in the provided assets.
Inside of the [ExampleSceneScripts], find the objects called LeftController_PointerListener and RightController_PointerListener, remove the default script from it and add the custom Pointer Listener script (using add component).
In the Pointer Listener script for both LeftController_PointerListener and RightController_PointerListener, drag the previously created object labelled Global from the hierarchy into the section labelled Global.
In the Pointer Listener script for the LeftController_PointerListener, drag the LeftController object from [VRTK_Scripts] in the hierarchy into the section labelled Pointer, and the Controller (left) object from [VRTK_SDKManager] > SDKSetups > SteamVR > [CameraRig] into the section labelled Controller
In the Pointer Listener script for the RightController_PointerListener, drag the RightController object from [VRTK_Scripts] in the hierarchy into the section labelled Pointer, and the Controller (right) object from [VRTK_SDKManager] > SDKSetups > SteamVR > [CameraRig] into the section labelled Controller
Make an empty object called Colour_legend

Once these steps have been completed the program is ready to be run.
