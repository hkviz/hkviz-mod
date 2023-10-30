# AnalyticsRecorder

A Hollow Knight mod that records analytics about player position and in game events

# Developer notes

## How hk maps player location
Each room which is shown on the map contains a game object called: "Map Scene Region". 
It contains a BoxCollider2D which contains the area which should be mapped to the map tile inside the game map with the same name as the scene name.

There is also a FSM named "Compass Icon-Position" it does not handle the mapping from player position to exact spot on the map, 
and is only used for the zoomed out map. Where the player position is the same no matter where within an area the player is currently.