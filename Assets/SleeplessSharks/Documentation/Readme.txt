Sleepless Sharks - Version 1.1

Scripts will work perfectly on Mobile / PC / Consoles and while this scene was created with Unity 2017.1.0f3, it will also work for Unity 5+.

Tutorial Videos
---------------
Change Shark Model to use Sleepless Shark AI - https://youtu.be/uOlMg7GU420
How To Change Shark Targets - https://youtu.be/jtIwAIYIxVQ

Recommendations
---------------
The Shark Model is not game optimized, or animated. Both of which are recommended for a truly realistic Shark model using these scripts.
Also, For best effect, the water plane should be relatively calm to avoid the sharks dropping in and out of each wave. If you increase the wave height, ensure you drop the watersurfaceheight value in SharkFlocking to avoid the wake being drawn above the waves

Useful Information
------------------
1.	Water, Terrain and Obstacle Layers are important for filtering
2.	There are numerous Tags used to help the Shark AI determine what to do when they run into various situations. Eg. Player, Hunter, Water, Terrain etc
3.	The Shark is made up of the following Heirarchy :-
	Shark (Physics, Script and Collider)
	-----Shark# (Mesh Renderer)	
		-----Shark# (PlaceHolder for Eyes)
			-----LeftEye
			-----RightEye
		-----Fin (PlaceHolder for Wake Drawing)
			-----Wake (Particle Wake)
			-----Wake2 (Trail Renderer Wake)

The Character model can be found in the Unity AngryBots Demo.
Rust Texture on Barrels:- © License: Free for commercial use – Public Domain Photo | Uploaded by: Alexandr Blankov - http://atextures.com/old-brown-rusty-metal-texture/
Non optimized Shark Model modelled/created from online Tutorial - https://cgi.tutsplus.com/tutorials/modeling-texturing-rigging-a-realistic-shark-in-3d-studio-max-part-1--cg-27541

If you have any questions, comments or improvement suggestions, you can  email me: daceian@yahoo.com.au
Other contact details available on our website: www.sleeplessentertainment.com.au