using MyRPGGame.Events;
using MyRPGGame.PathFinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyRPGGame.World
{
    public class WorldGeneration : MonoBehaviour
    {
        enum Direction { Right, Top, Left, Down };

        static WorldGeneration _instance;
        public Tilemap groundMap;
        public Tilemap waterMap;
        public Tilemap objectsMap;

        public List<GameObject> enemies;
        public Tile waterTile;

        public RuleTile groundTile;

        private int size;
        private int numberOfEnemies;
        private Vector3Int position = new Vector3Int(0, 0, 0);

        public static WorldGeneration Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<WorldGeneration>();
                }
                return _instance;
            }
        }

        public void StartGeneratingWorld(int _size, int _numberOfEnemies)
        {
            Debug.Log("Starting world generation.");
            size = _size;
            numberOfEnemies = _numberOfEnemies;
            Invoke(nameof(GenerateWorld), 0.1f);
        }

        private void GenerateWorld()
        {
            groundMap.ClearAllTiles();
            objectsMap.ClearAllTiles();
            MakeEdges();
            position = Vector3Int.zero;
            while (groundMap.GetTile(position) == waterTile)
            {
                position.y--;
            }
            groundMap.FloodFill(position, groundTile);

            waterMap.origin = new Vector3Int(-2 * size, -2 * size, 0);
            waterMap.size = new Vector3Int(4 * size, 3 * size, 0);
            waterMap.FloodFill(waterMap.origin, waterTile);

            PlaceObjects(size / 2, 3, "MapObjects/Buildings");
            PlaceObjects(size, 2, "MapObjects/Tress");
            PlaceObjects(8 * size, 1, "MapObjects/Extras");
            SpawnEnemies();
            SpawnPlayer();

            Debug.Log("World generated.");
            //Aparently we need to wait one frame in order to colliders apear
            Invoke(nameof(CreateNodeGrid), Time.deltaTime);
        }
        private void MakeEdges()
        {
            position = Vector3Int.zero;

            do
            {
                Move(RandomWithoutOneDirection(Direction.Right));
                groundMap.SetTile(position, waterTile);
            } while (position.x > -size);
            do
            {
                Move(RandomWithoutOneDirection(Direction.Top));
                groundMap.SetTile(position, waterTile);
            } while (position.y > -size);

            do
            {
                Move(RandomWithoutOneDirection(Direction.Left));
                groundMap.SetTile(position, waterTile);
            } while (position.x < size);

            do
            {
                Move(RandomWithoutOneDirection(Direction.Down));

                groundMap.SetTile(position, waterTile);
            } while (position.y < 0);

            do
            {
                Move(RandomWithoutOneDirection(Direction.Right));
                groundMap.SetTile(position, waterTile);
            } while (position.x > size / 6);
            GoToStart();
        }
        private Direction RandomWithoutOneDirection(Direction withoutWhat)
        {
            Direction randomDirection;
            do
            {
                randomDirection = (Direction)Random.Range(0, 4);
            } while (randomDirection == withoutWhat);
            return randomDirection;
        }

        private void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    position.x++;
                    break;
                case Direction.Top:
                    position.y++;
                    break;
                case Direction.Left:
                    position.x--;
                    break;
                case Direction.Down:
                    position.y--;
                    break;
            }
        }

        private void GoToStart()
        {
            while (position.x != 0 || position.y != 0)
            {
                if (position.y > 0)
                    position.y--;
                if (position.y < 0)
                    position.y++;
                if (position.x > 0)
                    position.x--;
                groundMap.SetTile(position, waterTile);
            }
        }
        private void PlaceObjects(int numberOfTiles, int distance, string directoryName)
        {
            int count;//prevent invinite loop when not enought place for objects
            Object[] loadedObjects = Resources.LoadAll(directoryName);

            for (int i = 0; i < numberOfTiles; i++)
            {
                count = 0;
                do
                {
                    position.x = Random.Range(-2 * size, 2 * size);
                    position.y = Random.Range(-2 * size, size);
                    count++;
                }
                while (!(CheckIfPositionPossible(distance, (x) => (objectsMap.GetTile(x) != null) || (groundMap.GetTile(x) != groundTile))) && count < 2000);
                if (count < 2000)
                {
                    objectsMap.SetTile(position, (RuleTile)loadedObjects[Random.Range(0, loadedObjects.Length)]);
                }
                else
                {
                    Debug.Log("World Generator wasn't able to find a free spot for all " + directoryName + " propably due to the lack of space.");
                    break;
                }
            }
        }
        private bool CheckIfPositionPossible(int distance, System.Func<Vector3Int, bool> condition)
        {
            Vector3Int tempPosition = Vector3Int.zero;
            for (int i = -distance; i <= distance; i++)
            {
                for (int j = -distance; j <= distance; j++)
                {
                    tempPosition.x = position.x + i;
                    tempPosition.y = position.y + j;

                    if (condition(tempPosition))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void SpawnEnemies()
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                do
                {
                    position.x = Random.Range(-2 * size, 2 * size);
                    position.y = Random.Range(-2 * size, size);
                } while (!CheckIfPositionPossible(5, (x) => groundMap.GetTile(x) != groundTile));

                GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)], position, Quaternion.identity);
            }
        }
        private void SpawnPlayer()
        {
            do
            {
                position.x = Random.Range(-2 * size, 2 * size);
                position.y = Random.Range(-2 * size, size);
            } while (!CheckIfPositionPossible(5, (x) => groundMap.GetTile(x) != groundTile));
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(position));
        }

        private void CreateNodeGrid()
        {
            GetComponent<NodeGrid>().CreateNodeGrid();
        }
    }
}