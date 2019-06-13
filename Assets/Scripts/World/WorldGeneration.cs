using MyRPGGame.Events;
using MyRPGGame.PathFinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyRPGGame.World
{
    public class WorldGeneration : MonoBehaviour
    {
        private enum Direction { Right, Top, Left, Down };

        [SerializeField] public Tilemap groundMap;
        [SerializeField] private Tilemap waterMap;
        [SerializeField] private Tilemap objectsMap;
        private const string BuildingsPrefabsCatalog = "MapObjects/Buildings";
        private const string TressPrefabsCatalog = "MapObjects/Tress";
        private const string ExtrasPrefabCatalog = "MapObjects/Extras";

        [SerializeField] private Tile waterTile;
        [SerializeField] private RuleTile groundTile;

        [SerializeField] private List<GameObject> enemyTypes;

        private int siezeOfTheMap;
        private int numberOfEnemiesToSpawn;
        private Vector3Int position = new Vector3Int(0, 0, 0);

        static WorldGeneration _instance;

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
            siezeOfTheMap = _size;
            numberOfEnemiesToSpawn = _numberOfEnemies;
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

            waterMap.origin = new Vector3Int(-2 * siezeOfTheMap, -2 * siezeOfTheMap, 0);
            waterMap.size = new Vector3Int(4 * siezeOfTheMap, 3 * siezeOfTheMap, 0);
            waterMap.FloodFill(waterMap.origin, waterTile);

            PlaceObjects(siezeOfTheMap / 2, 3, BuildingsPrefabsCatalog);
            PlaceObjects(siezeOfTheMap, 2, TressPrefabsCatalog);
            PlaceObjects(8 * siezeOfTheMap, 1, ExtrasPrefabCatalog);
            SpawnEnemies();
            SpawnPlayer();

            //Wait for Physics
            Invoke(nameof(CreateNodeGrid), Time.fixedDeltaTime);
        }
        private void MakeEdges()
        {
            position = Vector3Int.zero;

            do
            {
                Move(RandomWithoutOneDirection(Direction.Right));
                groundMap.SetTile(position, waterTile);
            } while (position.x > -siezeOfTheMap);
            do
            {
                Move(RandomWithoutOneDirection(Direction.Top));
                groundMap.SetTile(position, waterTile);
            } while (position.y > -siezeOfTheMap);

            do
            {
                Move(RandomWithoutOneDirection(Direction.Left));
                groundMap.SetTile(position, waterTile);
            } while (position.x < siezeOfTheMap);

            do
            {
                Move(RandomWithoutOneDirection(Direction.Down));

                groundMap.SetTile(position, waterTile);
            } while (position.y < 0);

            do
            {
                Move(RandomWithoutOneDirection(Direction.Right));
                groundMap.SetTile(position, waterTile);
            } while (position.x > siezeOfTheMap / 6);
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
                    position.x = Random.Range(-2 * siezeOfTheMap, 2 * siezeOfTheMap);
                    position.y = Random.Range(-2 * siezeOfTheMap, siezeOfTheMap);
                    count++;
                }
                while (!(CheckIfPositionPossible(distance, (x) => (objectsMap.GetTile(x) != null) || (groundMap.GetTile(x) != groundTile))) && count < 2000);
                if (count < 2000)
                {
                    objectsMap.SetTile(position, (RuleTile)loadedObjects[Random.Range(0, loadedObjects.Length)]);
                }
                else
                {
                    break;//propably not enough space
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
            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                do
                {
                    position.x = Random.Range(-2 * siezeOfTheMap, 2 * siezeOfTheMap);
                    position.y = Random.Range(-2 * siezeOfTheMap, siezeOfTheMap);
                } while (!CheckIfPositionPossible(5, (x) => groundMap.GetTile(x) != groundTile));

                GameObject newEnemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], position, Quaternion.identity);
            }
        }
        private void SpawnPlayer()
        {
            do
            {
                position.x = Random.Range(-2 * siezeOfTheMap, 2 * siezeOfTheMap);
                position.y = Random.Range(-2 * siezeOfTheMap, siezeOfTheMap);
            } while (!CheckIfPositionPossible(5, (x) => groundMap.GetTile(x) != groundTile));
            EventManager.Instance.TriggerEvent(new OnPlayerTeleportation(position));
        }

        private void CreateNodeGrid()
        {
            GetComponent<NodeGrid>().CreateNodeGrid();
        }
    }
}