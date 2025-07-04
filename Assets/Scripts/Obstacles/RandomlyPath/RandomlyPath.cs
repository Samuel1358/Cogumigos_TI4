using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandolyPathFeedback))]
public class RandomlyPath : MonoBehaviour
{
    private enum Direction
    {
        Up = 1,
        Right = 2,
        Down = 3,
        left = 4
    }

    [SerializeField] private int _xLength;
    [SerializeField] private int _zLength;
    [SerializeField] private float _spacing;
    [SerializeField] private GameObject _pathPlataformPrefab;
    [SerializeField] private GameObject _fakePlatformPrefab;

    private int[,] _path;
    private bool _safetyLock = false;

    private RandolyPathFeedback _feedback;

    private void Awake()
    {
        if (_pathPlataformPrefab.TryGetComponent(out RandolyPathPlatform platform) == false)
        {
            _safetyLock = true;
            Debug.LogWarning("Path Platform is missing component 'RandolyPathPlatform'");
        }
        if (_fakePlatformPrefab.TryGetComponent(out RandolyPathPlatform fakePlatform) == false)
        {
            _safetyLock = true;
            Debug.LogWarning("Fake Path Platform is missing component 'RandolyPathPlatform'");
        }

        if (_safetyLock)
            return;

        _feedback = GetComponent<RandolyPathFeedback>();
    }

    [ContextMenu("Instantiate Path")]
    public void InstantiatePath()
    {
        if (_safetyLock)
        {
            Debug.LogError("Path Platform and/or Fake Path Platform are not suitable!");
            return;
        }

        GeneratePath();

        for (int i = 0; i < _path.GetLength(1); i++)
        {
            for (int j = 0; j < _path.GetLength(0); j++)
            {
                GameObject obj;
                Vector3 pos = CauculatePosition(j, i);
                if (_path[j, i] == 1)
                {
                    obj = Instantiate(_pathPlataformPrefab, new Vector3(pos.x, transform.position.y, pos.z), Quaternion.identity);
                    _feedback.AddPlatform(obj);
                }
                else
                {
                    obj = Instantiate(_fakePlatformPrefab, new Vector3(pos.x, transform.position.y, pos.z), Quaternion.identity);
                    _feedback.AddFakePlatform(obj);
                }                
            }
        }

        _feedback.StartFeedback();
    }

    [ContextMenu("Print Path")]
    public void PrintPath()
    {
        GeneratePath();

        string print = "";

        for (int i = 0; i < _path.GetLength(1); i++)
        {
            print = "";
            print += "[";

            for (int j = 0; j < _path.GetLength(0); j++)
            {
                print += _path[j, i];
                if (j < _path.GetLength(0) - 1)
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
            _path = new int[_xLength, _zLength];
            freePath = RecursiveGeneratePath(Random.Range(1, _path.GetLength(0) - 1), 0, _path, ref curved);
            //Debug.Log(freePath);
        }        

        for (int i = 0; i < _path.GetLength(0); i++)
        {
            for (int j = 0; j < _path.GetLength(1); j++)
            {
                if (_path[i, j] != 1)
                    _path[i, j] = 0;
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
            //Debug.Log("Travou!");
            freePath = false;
            return Direction.Up;
        }
        else
        {
            freePath = true;
        }

        return (Direction)op[Random.Range(0, op.Count)];
    }

    private Vector3 CauculatePosition(int x, int y)
    {
        /*float disX = x * _spacing;
        float disY = y * _spacing;

        float posx = (transform.right * disX).x;
        float posy = (transform.forward * disY).z;

        return new Vector2(transform.position.x + posx, transform.position.z + posy);*/

        Vector3 point = new Vector3(transform.position.x + (x * _spacing), 0, transform.position.z + (y * _spacing));
        float dis = Vector3.Distance(transform.position, point);

        Quaternion rot = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);
        Vector3 dir = rot * (point - transform.position);

        Vector3 offset = dir.normalized * dis;
        return new Vector3(transform.position.x + offset.x, transform.position.y, transform.position.z + offset.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (_xLength * _spacing * transform.right));
        Gizmos.DrawLine(transform.position, transform.position + (_zLength * _spacing * transform.forward));
    }
}
