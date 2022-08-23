using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridGenerator))]
public class MazeManager : MonoBehaviour
{
    [SerializeField] private Vector2Int mazeSize;
    [SerializeField] private UnityEvent<Vector2Int> onGenerateMaze = new UnityEvent<Vector2Int>();
    [SerializeField] private UnityEvent<MazeGrid> onGenerateMazeFinish = new UnityEvent<MazeGrid>();

    private Dictionary<AlgorithmTypes, MazeAlgorithm> algorithms = new Dictionary<AlgorithmTypes, MazeAlgorithm>();

    private GridGenerator gridGenerator;
    private Coroutine currentMazeGenerator;
    private MazeAlgorithm currentMazeAlgorithm;
    
    private bool delay;

    public void SetWidth(int value)
    {
        mazeSize.x = value;
    }
    
    public void SetHeight(int value)
    {
        mazeSize.y = value;
    }

    public void SetDelay(bool value)
    {
        delay = value;
    }

    public void SetAlgorithm(string algorithmName)
    {
        AlgorithmTypes algorithmType = GetAlgorithmTypeByName(algorithmName);

        if (algorithmType.Equals(AlgorithmTypes.NULL))
        {
            Debug.LogError("Couldn't find algorithm of: " + algorithmName);
            return;
        }
        
        currentMazeAlgorithm = GetAlgorithm(algorithmType);
    }

    public MazeAlgorithm GetAlgorithm(AlgorithmTypes type)
    {
        return algorithms[type];
    }

    public AlgorithmTypes GetAlgorithmTypeByName(string algorithmName)
    {
        Array types = Enum.GetValues(typeof(AlgorithmTypes));

        AlgorithmTypes algorithm = AlgorithmTypes.NULL;
        
        for (int i = 0; i < types.Length; i++)
        {
            AlgorithmTypes type = (AlgorithmTypes) types.GetValue(i);
            if (algorithmName.Equals(type.ToString()))
            {
                algorithm = type;
                break;
            }
        }

        return algorithm;
    }

    public void Generate()
    {

        onGenerateMaze.Invoke(mazeSize);
        
        gridGenerator.GenerateGrid(mazeSize);
        
    }

    public void GenerateMaze(MazeGrid grid)
    {
        if (currentMazeAlgorithm == null)
        {
            Debug.LogError("There is no algorithm configured.");
            return;
        }

        currentMazeAlgorithm.MazeGrid = grid;
        currentMazeAlgorithm.VisualFeedback = HasDelay();
        currentMazeAlgorithm.Initialize();

        currentMazeGenerator = StartCoroutine(MazeHandler(currentMazeAlgorithm));

    }

    private IEnumerator MazeHandler(MazeAlgorithm algorithm)
    {
        while (!algorithm.IsDone())
        {
            if (HasDelay())
            {
                yield return null;
            }

            algorithm.GenerateMaze();
        }

        onGenerateMazeFinish.Invoke(algorithm.MazeGrid);
        yield return null;
    }

    public void DestroyMaze()
    {
        if (currentMazeGenerator != null)
        {
            StopCoroutine(currentMazeGenerator);

            MazeGrid grid = currentMazeAlgorithm.MazeGrid;
            
            grid.Destroy();

            currentMazeGenerator = null;
            currentMazeAlgorithm = null;

        }
    }
    
    private void Awake()
    {
        InitializeAlgorithmMap();
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        gridGenerator = GetComponent<GridGenerator>();
    }

    private void InitializeAlgorithmMap()
    {
        algorithms.Add(AlgorithmTypes.HuntAndKill, new HuntAndKill(null));
        algorithms.Add(AlgorithmTypes.Prims, new Prims(null));
    }

    private bool HasDelay()
    {
        return delay;
    }

}