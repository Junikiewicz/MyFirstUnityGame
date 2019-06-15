# Enemy

All scripts responsible for enemy behaviour and appearance.

## Enemy abstract class and Enemy Types

Enemy is an abstract class which defines almost all enemies interactions. All enemy types are subclasses.
At this moment, enemies basically differ only in the way of attacking.

## Missiles

Missiles shot by enemies. A fireball is following the player (because why not) while an arrow maintains initial flight direction.

## SpritesChanger

It's a very useful script for lazy people. I have 4 enemy classes and they have the same animations. Only sprites are different. So instead of making tons of new animations in the animator, I'm replacing sprites at runtime. It's like changing textures on models. It's quite a popular trick. There is still a place for optimization. You also have to be very careful since animations sprites need to match perfectly.

## PopupText

Just a simple popup message showing damage taken by the enemy. 

## TODO

Separation of animations and enemy logic into different scripts

Optimized connection with pathfinding Unit
