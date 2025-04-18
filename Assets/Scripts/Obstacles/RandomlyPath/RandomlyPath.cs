using System.Collections.Generic;
using UnityEngine;

public class RandomlyPath : MonoBehaviour
{
    private enum Direction
    {
        Up = 1,
        Right = 2,
        Down = 3,
        left = 4
    }

    [SerializeField] private int wdith;
    [SerializeField] private int height;
    [SerializeField] private float spacing;
    [SerializeField] private GameObject destructiblePrefab;
    [SerializeField] private GameObject plataformPrefab;

    private int[,] path;

    [ContextMenu("Instantiate Path")]
    public void InstantiatePath()
    {
        GeneratePath();

        for (int i = 0; i < path.GetLength(1); i++)
        {
            for (int j = 0; j < path.GetLength(0); j++)
            {
                GameObject obj;
                if (path[j, i] == 1)
                    obj = Instantiate(plataformPrefab, new Vector3(transform.position.x + (j * spacing), transform.position.y, transform.position.z + (i * spacing)), Quaternion.identity);
                else
                    obj = Instantiate(destructiblePrefab, new Vector3(transform.position.x + (j * spacing), transform.position.y, transform.position.z + (i * spacing)), Quaternion.identity);                

                obj.transform.parent = this.transform;
            }
        }
    }

    [ContextMenu("Print Path")]
    public void PrintPath()
    {
        GeneratePath();

        string print = "";

        for (int i = 0; i < path.GetLength(1); i++)
        {
            print = "";
            print += "[";

            for (int j = 0; j < path.GetLength(0); j++)
            {
                print += path[j, i];
                if (j < path.GetLength(0) - 1)
                    print += ", ";
            }

            print += "]";
            Debug.Log(print);
        }          
    }

    private void GeneratePath()
    {       
        bool curved = false;
        bool freePath = false;
        while (!freePath)
        {
            path = new int[wdith, height];
            freePath = RecursiveGeneratePath(Random.Range(1, path.GetLength(0) - 1), 0, path, ref curved);
            Debug.Log(freePath);
        }        

        for (int i = 0; i < path.GetLength(0); i++)
        {
            for (int j = 0; j < path.GetLength(1); j++)
            {
                if (path[i, j] != 1)
                    path[i, j] = 0;
            }
        }        
    }

    private bool RecursiveGeneratePath(int x, int y, int[,] path, ref bool curved)
    {
        path[x, y] = 1;
        if (y == 0)
        {
            y++;
        }
        else if (y == path.GetLength(1) - 1)
        {
            return true;
        }
        else
        {
            bool freePath;
            Direction randDir = RandomizerDirection(x, y, path, curved, out freePath);

            if (!freePath)
                return false;

            switch (randDir)
            {
                case Direction.Up:
                    y++;
                    break;
                case Direction.Right:
                    x++;
                    break;
                case Direction.Down:
                    y--;
                    if (!curved)
                        curved = true;
                    break;
                case Direction.left:
                    x--;
                    break;
            }
        }

        return RecursiveGeneratePath(x, y, path, ref curved);
    }

    private Direction RandomizerDirection(int x, int y, int[,] path, bool curved, out bool freePath)
    {
        List<int> op = new List<int>();
        int multiplier = 1;
        

        if (y < path.GetLength(1) - 1)
            if (path[x, y + 1] != 1 && ((x < path.GetLength(0) - 1) ? path[x + 1, y + 1] != 1 : true) && ((x > 1) ? path[x - 1, y + 1] != 1 : true))
            {
                op.Add(1);
            }               

        if (x + 1 < path.GetLength(0))
            if (path[x + 1, y] != 1 && ((y < path.GetLength(1) - 1) ? path[x + 1, y + 1] != 1 : true) && ((y > 1) ? path[x + 1, y - 1] != 1 : true))
            {
                if (!curved)
                    multiplier = 2;
                else
                    multiplier = 2;

                for (int i = 0; i < multiplier; i++)
                {
                    op.Add(2);
                }               
            }                

        if (y > 1 && (1 < x && x < path.GetLength(0) - 2))
            if (path[x, y - 1] != 1 && path[x + 1, y - 1] != 1 && path[x - 1, y - 1] != 1)
            {
                if (!curved)
                    multiplier = 3;
                else
                    multiplier = 1;

                for (int i = 0; i < multiplier; i++)
                {
                    op.Add(3);
                }                
            }               

        if (x > 0)
            if (path[x - 1, y] != 1 && ((y < path.GetLength(1) - 1) ? path[x - 1, y + 1] != 1 : true) && ((y > 1) ? path[x - 1, y - 1] != 1 : true))
            {
                if (!curved)
                    multiplier = 2;
                else
                    multiplier = 2;

                for (int i = 0; i < multiplier; i++)
                {
                    op.Add(4);
                }              
            }                

        if (op.Count == 0)
        {
            Debug.Log("Travou!");
            freePath = false;
            return Direction.Up;
        }
        else
        {
            freePath = true;
        }

        return (Direction)op[Random.Range(0, op.Count)];
    }
}
