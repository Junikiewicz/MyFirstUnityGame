
using UnityEngine;
namespace MyRPGGame.PathFinding
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;
        public Node parent;
        int heapIndex;

        public int gridX, gridY;

        public int hCost, gCost;

        public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            gridX = _gridX;
            gridY = _gridY;
        }

        public static bool AreNodesEqual(Node node1, Node node2)
        {
            if (node1.gridX == node2.gridX && node1.gridY == node2.gridY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public int FCost
        {
            get
            {
                return hCost + gCost;
            }
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }


    }
}