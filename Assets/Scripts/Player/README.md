# Player

Scripts associated with character controlled by the player.

## PlayerController

Input, moving, attacking, animations. 

## PlayerStatisticsController

Contains statBlock, performs the majority of operations related to statistics. 

## TODO
Clearer "differentiation" of responsibilities

Spreading PlayerController to: MovmentController, AttackController, AnimationController. It may be tricky due to "interlacing" of these components.
