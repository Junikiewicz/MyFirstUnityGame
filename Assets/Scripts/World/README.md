# World

Scripts associated with our world - some small scripts responsible for scene switching and also our world generator.

## WorldGenerator

The name sounds impressive, but script is very simple. First, we make a shape by moving our seeker randomly. Then we fill it with grass. Then we just place all objects, keeping in mind they shouldn't interfere with each other. Then we invoke a pathfinding method in order to create a node grid based on our new world.

## TODO

Merge PortalController and DungeonExit into one script.
