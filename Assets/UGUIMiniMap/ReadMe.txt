Thanks for purchase UGUI MiniMap!.
Version 2.4.3

For documentation and tutorials go to (in Unity Editor Toolbar) Window -> MiniMap -> Documentation
or see the online documentation (DEPRECATED): http://lovattostudio.com/documentations/ugui-minimap/

Get Started:

-After import the package in your project you only need add an layer to get ready.
in the LayerMask List (Edit -> Project Settings -> Tags And Layers -> Layers -> *) add a new layer in the field number 10 called 'MiniMap'.
that's, check the full documentation for details.

Any problem or question, feel free to contact us:

Contact Form: http://www.lovattostudio.com/en/support/
Forum: http://lovattostudio.com/forum/index.php

If you have a problem or bug, please contact us before leave a bad review, we respond in no time.

Change Log:

2.4.3
-Fix: Minimap marks keep working even after disable the feature in bl_MiniMap inspector.
-Fix: Minimap drag navigation didn't work on touch screen devices.

2.4.2
-Fix: Circle Minimap mask appear black in OpenGL 3.0 and Metal Graphic API's

2.4.1
-Fix: MiniMap orthographic prefabs in the 2D scene.

2.4

-Improve: Add Custom Mask Shader, now minimap shapes edges (e.g in circular maps) are much smoother.
-Improve: Minimap rotation, now the small rotation jump that happens when the player is oriented to the north doesn't happen anymore.
-Improve: Add more icons and shapes in the package.
-Improve: Now the MiniMap Entity and MiniMap Icon scripts are inheritable to allow modifying and extending them easier.
-Improve: The default MiniMaps prefabs.
-Fix: Minimap static icons jump to the border of the minimap randomly.
-Add: No-Render layer option in order to allow set up a certain layer that the minimap won't render (for Realtime MiniMap only).
-Improve: Overall performance and less memory allocation.

2.3

-Change: Unity 2019.4 minimum version.
-Improve: Added support for Picture mode in Orthographic minimaps in vertical orientation (for 2D Games).
-Improve: Picture creator, now you can preview the screen borders in the Game View window before take the screenshot.
-Fix: The position offset was not exposed in the inspector of bl_MiniMapItem.cs
-Fix: Issue that cause Picture mode minimap doesn't shows if the 'ShowGrids' toggle was turn off.

2.2.7
-Fix: Minimap is not rendered at certain heights.
-Improve: Use custom shader for Picture mode map for performance and better looking.
-Improve: Add support for Universal Render Pipeline.

2.2.5
-Fix: Circular compass was showing the inversed coordinates.

2.2.4
-Fix: Orthographic mini map prefab.
-Fix: bl_MiniMap.Target is readOnly.
-Improve: Remove warnings about unassigned private fields in Unity 2018.3++
-Improve: In-editor documentation skin in Unity Personal.

2.2
-Improve: Text tooltip, now the background of the text will be automatically scale depending of the string length.
-Improve: Add interact options, now you can select if the icon is interactable, if it's how? OnHover or OnTouch.
-Improve: Improve performance by changes made in the way that icons are updated.
-Improve: added custom update rate in bl_MiniMap.cs inspector, useful for mobile platforms (1 = normal Update rate)

2.1
-Improve: Update the documentation and integrate with Unity Editor (Windows -> MiniMap -> Documentation)
-Improve: Screen shot tool system.
-Fix: Icons was not masked when 'Show Icon OffScreen' was off, causing icons show outside of the minimap UI.
-Improve: Editor inspector scripts.
-Improve: Now can add the player icon in bl_MiniMap -> Render Settings -> Player Icon, instead search the Image Component for add it.
-Fix: Minimap icons what not working if you change the anchor pivot of the MiniMap UI root rectTransform.
-Improve: You not longer need to assign the Item prefab for each bl_MiniMapItem script that you set up, this will take automatically.
-Fix: Error in the compass when there's not main camera in the scene.
-Fix: Error when use Realtime mode but there's not a minimap texture assigned (which should not interfere because is not needed).

2.0
-New: Now can get an world position from the minimap.
-Add: World Point Markers, player click one point on the mini map and a marker will appear on the world map, it will disappear when player yet close to it.
-Improve: Clean code and improve performance.
-Improve: Add custom inspector for bl_MiniMap, now is easy to find and understand the settings.