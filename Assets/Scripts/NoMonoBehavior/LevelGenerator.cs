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
    private LabyrinthCell[,] m_LabyrinthCellsArray;

    public void GenerateLevel(GameObject cellPrefab, GameObject finishPrefab, NavMeshSurface navMeshSurface,GameManager gameManager)
    {
        //GameObject.Instantiate(wallPrefabs[0], new Vector3(-2.79f, 0.36f, 0), wallPrefabs[0].transform.rotation);

        GenerateLabyrinth(cellPrefab);





        var finish=GameObject.Instantiate(finishPrefab,new Vector3(3.09f, finishPrefab.transform.position.y,2.74f), finishPrefab.transform.rotation).GetComponent<Finish>();

        finish.m_gameWin.AddListener(gameManager.GameWin);

        navMeshSurface.BuildNavMesh();
    }

    private void GenerateLabyrinth(GameObject cellPrefab)
    {
        m_LabyrinthCellsArray = new LabyrinthCell[m_labyrinthX, m_labyrinthZ];
        for (var x= 0;x < m_labyrinthX; x++)
        {
            for(var z = 0; z< m_labyrinthZ; z++)
            {
                m_deltaX = (m_labyrinthX-1) * cellPrefab.transform.localScale.x / 2;
                m_deltaZ = (m_labyrinthZ-1) * cellPrefab.transform.localScale.z / 2;
                
                m_LabyrinthCellsArray[x,z] = GameObject.Instantiate(cellPrefab, new Vector3(x * cellPrefab.transform.localScale.x- m_deltaX, cellPrefab.transform.position.y, z * cellPrefab.transform.localScale.z- m_deltaZ), Quaternion.identity).GetComponent<LabyrinthCell>();
            }
        };

        BuildLabyrinthStepByStep(null, m_LabyrinthCellsArray[0, 0]);

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

        LabyrinthCell nextCell;

        do
        {
            nextCell = GetRandomNextCell(curCell);
            if (nextCell != null) BuildLabyrinthStepByStep(curCell, nextCell);
        } while (nextCell != null);
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
            if (!m_LabyrinthCellsArray[xIndex - 1, zIndex].IsProceeded)
            {
                cellsOptions.Add(m_LabyrinthCellsArray[xIndex - 1, zIndex]);
            }
        }
        if (xIndex + 1 < m_labyrinthX)
        {
            if (!m_LabyrinthCellsArray[xIndex + 1, zIndex].IsProceeded)
            {
                cellsOptions.Add(m_LabyrinthCellsArray[xIndex + 1, zIndex]);
            }
        }
        if (zIndex - 1 >= 0)
        {
            if (!m_LabyrinthCellsArray[xIndex, zIndex - 1].IsProceeded)
            {
                cellsOptions.Add(m_LabyrinthCellsArray[xIndex, zIndex - 1]);
            }
        }
        if (zIndex + 1 < m_labyrinthZ)
        {
            if (!m_LabyrinthCellsArray[xIndex, zIndex + 1].IsProceeded)
            {
                cellsOptions.Add(m_LabyrinthCellsArray[xIndex, zIndex + 1]);
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
}
