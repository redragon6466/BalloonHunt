
  DotMatrix - ReadMe.txt




Introduction
============


DotMatrix is low resolution display for example to clocks, elevators, roadsigns or to show scrolling texts. Each dot in display is object that changes
color (or rotation in case of 3D display) to show wanted content. Content can be added to display with simple commands or dot by dot.



How to use
==========


Choosing prefab
---------------


Prefabs folder have 3 subfolders. Choose the one that is suitable for your purposes:

- "Sprite-based_for_3D_or_2D_world" contains DotMatrix display that can be used in 3D or 2D worlds as part of the scene. Dots are made of sprites so
  display is totally flat and is typically embedded to another object that provides background and borders for the display. Typically, this is the one
  to use, unless you wish to add DotMatrix display to your UI.

- "Object-based_for_3D_world" contains DotMatrix where dots are actual 3D gameobjects that rotates when dots in display turns on or off. This is for
  you if you want everything in your scene to be real 3D.

- "Image-based_for_UI" contains DotMatrix that is meant to be used as part of UI. Dots in display are images that change colors when turning on or off.

Examples folder contains 5 scenes that show examples of all these three types.



Add DotMatrix display to scene
------------------------------


Simply drag prefab to your scene.

Note, if you are using UI-version of prefab: When dragging prefab to hierarchy or scene, prefab should automatically set UI canvas as its parent. If there is
no other UI and no UI Canvas in scene yet, one is created automatically. However, it's recommended to first create UI Canvas using normal Unity Editor tools
before adding UI-version of DotMatrix to the scene.

At this point display should be already visible on scene, though its size and colors may be totally wrong. Choose created DotMatrix gameobject and check
inspector window. It contains multiple settings where you can choose display size, dot prefabs, sizes and colors.

Choose desired parameters and position DotMatrix display to your scene/UI.

DotMatrix dots are created from prefabs. There are multiple ready made simple prefabs (different shape of dots). If you wish to have more control on how
display looks, you can make your own dot prefabs. After creating your own dot-object prefab, just drag that to "Dot Prefab" field in Unity inspector window.



Accessing DotMatrix
-------------------


Create new script that will be used to control DotMatrix display. Use any way you prefer to give your script reference to DotMatrix script in gameobject
created earlier. Make sure your own script is "using Leguar.DotMatrix;"

You can then get Controller of DotMatrix display using:

Controller controller = myDotMatrix.GetController();

Controller can be used to feed new commands to display. For example:

TextCommand textCommand = new TextCommand("Hello world!");
controller.AddCommand(textCommand);

This will cause text "Hello world!" to appear on display. By default it will be on center of the display.



Controller commands
-------------------


Commands can be added to controller before previous one is finished. All commands go to queue and they'll run in order they were added, one by one.

Basic commands are following:

- "TextCommand" adds text to display. Most common command to use.
- "PauseCommand" pauses Controller for certain amount of time. Whatever is currently on display will stay there.
- "ClearCommand" clears the display. There are different types of methods to clear the screen.

For example, following script would scroll text from left to center of display, wait for 2 seconds and then scroll text away:

TextCommand textCommand = new TextCommand("Hello and bye") {
    HorPosition = TextCommand.HorPositions.Center,
    Movement = TextCommand.Movements.MoveLeftAndStop
};
controller.AddCommand(textCommand);
controller.AddCommand(new PauseCommand(2f));
controller.AddCommand(new ClearCommand() {
    Method = ClearCommand.Methods.MoveLeft
});

Some more advanced commands:

- "ContentCommand" adds any free content to display, defined by two-dimensional int array.
- "CallbackCommand" will call Action given as parameter when this command is reached in Controller queue.



Direct access
-------------


If you wish to access display directly, you can use DisplayModel:

DisplayModel displayModel = myDotMatrix.GetDisplayModel();
displayModel.SetDot(0,0,1);

That will set upper left corner dot (0,0) to "on state" (1) during this or next Update loop (depending on script execution order).



Additional documents
====================


Inline c# documents are included to all public classes. They are also available online in html format:

http://www.leguar.com/unity/dotmatrix/apidoc/1.7/

There is also 5 example scenes containing 14 example displays and their scripts included in this package.
Take a look and feel free to modify and use any parts of the examples in your own projects.



Feedback
========


If you are happy with this package, please rate us or leave feedback in Unity Asset Store:

https://assetstore.unity.com/packages/slug/75420


If you have any problems, or maybe suggestions for future versions, feel free to contact:

http://www.leguar.com/contact/?about=dotmatrix
