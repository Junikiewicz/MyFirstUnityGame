using UnityEngine;
using System.Collections.Generic;
using MyRPGGame.World;

namespace MyRPGGame.PathFinding
{
    public class NodeGrid : MonoBehaviour
    {

        public bool displayGridGizmos;
        public LayerMask unwalkableMask;
        Vector2 gridWorldSize;
        Vector3 worldCenter;
        public GameObject player;
        Vector3 worldBottomLeft;
        float nodeRadius;
        Node[,] grid;

        float nodeDiameter;
        int gridSizeX, gridSizeY;

        public void CreateNodeGrid()
        {
            worldBottomLeft = GetComponent<WorldGeneration>().groundMap.origin;
            gridWorldSize = new Vector2(GetComponent<WorldGeneration>().groundMap.size.x, GetComponent<WorldGeneration>().groundMap.size.y);
            worldCenter = new Vector3(worldBottomLeft.x + gridWorldSize.x / 2, worldBottomLeft.y + gridWorldSize.y / 2);
            nodeRadius = GetComponent<WorldGeneration>().groundMap.tileAnchor.x;
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }

        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];


            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius-0.05f, unwalkableMask);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }


        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            Vector3 gridWorldPosition = worldPosition - worldBottomLeft;
            int x = Mathf.RoundToInt(gridWorldPosition.x / nodeDiameter - nodeRadius);
            int y = Mathf.RoundToInt(gridWorldPosition.y / nodeDiameter - nodeRadius);
            return grid[x, y];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(worldCenter, new Vector3(gridWorldSize.x, gridWorldSize.y));
            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    //if (n == NodeFromWorldPoint(new Vector3(player.transform.position.x, player.transform.position.y - 1)))
                    //{
                    //    Gizmos.color = Color.blue;
                    //}
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }
}