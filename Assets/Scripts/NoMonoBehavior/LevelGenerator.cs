using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class LevelGenerator
{
    private int m_labyrinthX = 9;
    private int m_labyrinthZ = 5;
    //make detection of screen size

    private float m_deltaX;
    private float m_deltaZ;
    private LabyrinthCell[,] m_labyrinthCellsArray;
    private List<LabyrinthCell> m_unusedlabyrinthCells = new List<LabyrinthCell>();
    private Finish m_finish;
    private bool m_isGenerationFinished = false;
    public bool IsGenerationFinished
    {
        get
        {
            return m_isGenerationFinished;
        }
    }

    public void GenerateLevel(GameObject cellPrefab, GameObject finishPrefab, NavMeshSurface navMeshSurface,int xSize, int zSize)
    {
        m_labyrinthX = xSize;
        m_labyrinthZ = zSize;
        GenerateLabyrinth(cellPrefab);

        var cellForFinish = GetFreeRandomCell();
        var finishPos = new Vector3(cellForFinish.transform.position.x, finishPrefab.transform.position.y, cellForFinish.transform.position.z);
        m_finish = GameObject.Instantiate(finishPrefab, finishPos, finishPrefab.transform.rotation).GetComponent<Finish>();

        

        navMeshSurface.BuildNavMesh();

        m_isGenerationFinished = true;
    }

    public Finish GetFinish()
    {
        return m_finish;
    }

    private void GenerateLabyrinth(GameObject cellPrefab)
    {
        m_labyrinthCellsArray = new LabyrinthCell[m_labyrinthX, m_labyrinthZ];
        for (var x= 0;x < m_labyrinthX; x++)
        {
            for(var z = 0; z< m_labyrinthZ; z++)
            {
                m_deltaX = (m_labyrinthX-1) * cellPrefab.transform.localScale.x / 2;
                m_deltaZ = (m_labyrinthZ-1) * cellPrefab.transform.localScale.z / 2;

                m_labyrinthCellsArray[x,z] = GameObject.Instantiate(cellPrefab, new Vector3(x * cellPrefab.transform.localScale.x- m_deltaX, cellPrefab.transform.position.y, z * cellPrefab.transform.localScale.z- m_deltaZ), Quaternion.identity).GetComponent<LabyrinthCell>();
                m_unusedlabyrinthCells.Add(m_labyrinthCellsArray[x, z]);
            }
        };

        BuildLabyrinthStepByStep(null, m_labyrinthCellsArray[0, 0]);
        
    }

    private void BuildLabyrinthStepByStep(LabyrinthCell prevCell, LabyrinthCell curCell)
    {
        curCell.Proceed();
        if (prevCell != null)
        {

            int prevXIndex = (int)ReturnCellIndexes(prevCell).x;
            int prevZIndex = (int)ReturnCellIndexes(prevCell).y;

            int curXIndex = (int)ReturnCellIndexes(curCell).x;
            int curZIndex = (int)ReturnCellIndexes(curCell).y;

            int indexToRemovePrev = 0;

            if (prevZIndex == curZIndex)
            {
                indexToRemovePrev = (curXIndex - prevXIndex + 4) % 4;

            }
            else
            {
                indexToRemovePrev = 1 - (curZIndex - prevZIndex) % 4;

            }
            int indexToRemoveCur = (indexToRemovePrev + 2) % 4;

            

            curCell.DeactivateWall(indexToRemoveCur);
            prevCell.DeactivateWall(indexToRemovePrev);
        }

        //LabyrinthCell nextCell;
        var nextCell = GetRandomNextCell(curCell);
        while (nextCell != null)
        {
            BuildLabyrinthStepByStep(curCell, nextCell);
            nextCell = GetRandomNextCell(curCell);
            
        };
    }

    private Vector2 ReturnCellIndexes(LabyrinthCell cell)
    {
        float xIndex = ((cell.transform.position.x + m_deltaX) / cell.transform.localScale.x);
        float zIndex = ((cell.transform.position.z + m_deltaZ) / cell.transform.localScale.z);
        return new Vector2(xIndex, zIndex);
    }
    private LabyrinthCell GetRandomNextCell(LabyrinthCell currentCell)
    {
        List<LabyrinthCell> cellsOptions = new List<LabyrinthCell>();
        int xIndex = (int)ReturnCellIndexes(currentCell).x;
        int zIndex = (int)ReturnCellIndexes(currentCell).y;
        
        if (xIndex - 1 >= 0)
        {
            if (!m_labyrinthCellsArray[xIndex - 1, zIndex].IsProceeded)
            {
                cellsOptions.Add(m_labyrinthCellsArray[xIndex - 1, zIndex]);
            }
        }
        if (xIndex + 1 < m_labyrinthX)
        {
            if (!m_labyrinthCellsArray[xIndex + 1, zIndex].IsProceeded)
            {
                cellsOptions.Add(m_labyrinthCellsArray[xIndex + 1, zIndex]);
            }
        }
        if (zIndex - 1 >= 0)
        {
            if (!m_labyrinthCellsArray[xIndex, zIndex - 1].IsProceeded)
            {
                cellsOptions.Add(m_labyrinthCellsArray[xIndex, zIndex - 1]);
            }
        }
        if (zIndex + 1 < m_labyrinthZ)
        {
            if (!m_labyrinthCellsArray[xIndex, zIndex + 1].IsProceeded)
            {
                cellsOptions.Add(m_labyrinthCellsArray[xIndex, zIndex + 1]);
            }
        }
        if (cellsOptions.Count > 0)
        { 
            var random = Random.Range(0, cellsOptions.Count);

            return cellsOptions[random];
        }
        else
        {
            return null;
        }
    }

   public LabyrinthCell GetFreeRandomCell()
    {
        if (m_unusedlabyrinthCells.Count ==0)
        {
            return null;
        }
        var randIdex = Random.Range(0, m_unusedlabyrinthCells.Count);
        var cellToReturn = m_unusedlabyrinthCells[randIdex];
        m_unusedlabyrinthCells.Remove(m_unusedlabyrinthCells[randIdex]);

        return cellToReturn;
    }

    public Vector3 GetWayPointInCell(LabyrinthCell cell,float deltaToWalls)
    {
        var range = cell.InnerSize / 2 - deltaToWalls;
        var xCenter = cell.transform.position.x;
        var zCenter = cell.transform.position.z;
       
        var waypointX = Random.Range(xCenter - range, xCenter+range);
        var waypointZ = Random.Range(zCenter - range, zCenter+range);
        
        return new Vector3(waypointX,0, waypointZ);
    }
}
