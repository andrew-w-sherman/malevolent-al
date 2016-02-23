# Enemy Study

Study of enemy pathfinding

Enemy moves toward player when there is an obstacle-free path. If a wall impedes the path directly from the enemy to the player, the enemy will continue in the last direction it was moving for 3 seconds. If regains obstacle-free sight of the player in these 3 seconds, it will continue pursuing the obstacle as normal, otherwise it will stop moving after the 3 seconds are up.
