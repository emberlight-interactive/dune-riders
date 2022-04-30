Dev Ops
---
- Integrate tdd with nunit?
- Add github issue templates for 
  - Environment/Rooms
  - Gameplay element
  - AI system
  - UI system
  - Event Triggers
  - Player Controller
- Save system enhancement (all one file)
- Finish collecting concept art


- Emberlight interactive Splash logo
- Opening menu
  - Build opening menu scene
  - Source menu design for logo, panels, buttons, sliders, toggles, checkboxes, font
  - Panel - Logo with list of saved games and slots
    - Delete save / load save options upon selecting save (confirm box)
    - New game confirm when selecting emtpy slot
  - [Research necessary quest settings]
- Build pause system with "Continue", "Save and exit"

- Build opening scene
  - Collect space sky box
  - Model for tidally locked planet with a populous "Ring City"
  - Model for sun
  - Parvati dialouge explaining context and objective

- Build tutorial
  - 

Build Player's Rider
  - Interior model
    - Windsheild and side door windows
    - Rear view door mirrors?
    - Resource stats panel for 3 current resources and burn rate / number of vechiles???
    - Panel for villages fuel status?
    - Should have sufficent space between player and front windshield for hand gestures
    - Should have a visible "radio" and a connected audio source
    - Should have a functioning "map" of the surrounding procedural area
  - Should have accelerate system
  - Should have brake/decelerate system
  - Should have steering system (wheel)
  - Should have a triple weapons system
    - SHould have retractable functionality that affects speed
      - Deployed weapons causes a dot to appear on display to indicate firing reticle
    - Should have swapable functionlity to use all weapon types (machine gun, missile, cannon)
    - Should have a firing and cooldown system for all 3 weapon types
      - Should have 3 different particle effects for shots and explosions
    - Should have a camera follow system to shoot in different directions / angles??? (Need to consider motion sickness)
  - Steering wheel with honk functionality to initiate dialouge
    - Hand gesture system for yes no dialouge options
      - Spheres with hand gesture inside with descriptive text for option
    - Slider system (on resource panel???) for amount transfers (slider, cancel, confirm | Yes / No)
  - Area for status text (saving, pausing, etc)

- Build dialouge system
  - Honkable areas -> events
  - Panel to hold yes / no context

- Build Model for home village with gate and "overhang?" for npcs to recieve player and initiate fuel transfer
- Build gas sustainence system
- Build dialouge system for gas transfer at village
- Add save system to save upon gas transfers
- Build transfer village dialouge after transfers where gas exceeds the "migration" requirement (enough to migrate and enough to sustain (about half of total))
- Build "destroyed home village model"
- Build village died fail scene 

- Build dune terrain generator
  - Build system that outlines "faster" travel routes (sand stone) and slower routes (dunes)
  - Optimize performance for quest (culling and fog system)
  - Consider / program in changes in terrain to represent different "levels" with an area that represents the "ring"s border (wall?)
    - Higher levels means less collectable gas and more necessity to attack bandits and destroy outposts
  - Consider system that gradually expands the ring's city buildings in the skybox
  - Build algorithm to populate landscape with different monuments / bandit party spawn locations / outpost locations depending on distance to ring


- Collect low poly human models to represent Men, women and children (floating torso, hands, head with drawn expressions and hair) similar to Wii characters
    - Collect idle, wave, sad, cheer and "boping walk" animations

- Collect concept art -> Collect Models with strong LOD's to represent 3 - 4 different types of "ruins" to harbour the 3 types of resources and mercenary hiring locations. Make the ruins easily traversable by car lol
  - Bake in "spawn" locations and random placement system
- Collect models to represent the 3 different type of collectibles - fuel, scrap metal and precious metal
- Collect models to represent the 3 types of "destroyables" which will drop the collectables
- Build the destroy system to drop collectables

- Collect concept art -> Collect models with very strong lods for different Car chasis -> wheels should be seperable model
  - Armoured rider
  - Rider
  - Buggy

- Collect concept art -> collect models with very strong lods for weapon systems
  - Machine gun
  - Triple missile system
  - Cannon

Build mercenary hire system
  - Build positions
  - Honk interaction
  - Sad when reject
  - Hop in car when accept and join company

Build rider AI
  - Peaceful follow system
  - Battle system

Build bandit AI system
  - Build metrics for when being attacked / engaged

Build company system
  - Simple instance counter for riders / damage stats


Build outpost models
  - Medium model
  - Large model
  - Turret locations
  - Health bar for turrets and main structure

End scene
  - Build the ring scene with villages and girl run up event. Credits and score?
  - Dialouge option pointing to door (exit)
  - Save and exit here considers it finished too
  - Leads to "locking" save file with score