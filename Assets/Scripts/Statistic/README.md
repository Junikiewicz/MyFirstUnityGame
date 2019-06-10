# Statistic

System used by the player and enemies. It stores and gives access to character attributes.

## Statistic abstract class and Stats

So every single stat like Health or Stamina is just a separate class delivered from Statistic class. Right now they don't differ, but it may be changed later. They store values related to the statistic (modifiers, etc.). 

## StatBlock

All stats are stored in the dictionary. Characters don't have access to the stats directly, but they can use Stat Block methods in order to get or set them.

## TODO

Validation of values (like negative Health)
