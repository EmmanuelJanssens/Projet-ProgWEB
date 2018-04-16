using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonDisk
{

    private float _minDistance;
    private float _maxDistance;

    public GameObject yolo;
    public GameObject aights;

    public List<Cell> Cells;

    public void Start()
    {
    }

    /// <summary>
    /// Generate 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="min_dist"></param>
    /// <param name="totalPoints"></param>
    /// <returns></returns>
    public IEnumerator Generate(List<Vector3> data, int width, int height,int resolution,float min_dist,int density)
    {
        /*
            divide map in grid with dimensions (width*height)
             
            Generate a random position into each grid add them to the processing list
                       
            generate a random point into that grid

            generate random points arround that generated point

            if the generated point goes outside the grid
         */

        //An array of grid
        List<Cell> grid = new List<Cell>();

        List<Vector3> toProcess = new List<Vector3>();

        //number of cells in widht and height
        int gridWidth, gridHeight;


        gridWidth = (int)Mathf.Ceil(width / resolution);
        gridHeight = (int)Mathf.Ceil(height / resolution);


        //Create the grid of rectangles
        for(int y = 0; y < gridHeight; y++)
        {
            for( int x  =0; x < gridWidth; x++)
            {

                grid.Add(new Cell(new Rect( x * resolution, y * resolution, resolution, resolution)));            
            }

        }

        for (int i = 0; i < grid.Count; i++)
        {
            float x = Random.Range(grid[i].rect.position.x, grid[i].rect.xMax);
            float z = Random.Range(grid[i].rect.position.y, grid[i].rect.yMax);

            toProcess.Add(new Vector3(x, 0, z));
        }

        Vector3 nextpoint;

        //Go trough the list of points to process in each CELL
        for( int i = 0; i < toProcess.Count; i++)
        {
            //Start with the point at index 0
            nextpoint = toProcess[i];
            for (int j  = 0; j < density; j ++)
            {

                //Generate a position arround the next point
                Vector3 newpoint = GeneratePointArround(nextpoint, min_dist);
                //if the generated point is not into the cell
                if(grid[i].rect.Contains(new Vector2(newpoint.x, newpoint.z)))
                {

                    //Add the new points in to the current cell at index I
                    Vector3 normalized = new Vector3(newpoint.x / width, 0, newpoint.z / height);
                    grid[i].points.Add(normalized);
                    data.Add(normalized);

                    //Set the next point to generate a point arround to the previously generated new poin
                    nextpoint = newpoint;
                }
            }
        }
        yield return null;
    }
    public Vector3 GeneratePointArround(Vector3 origin, float minDist)
    {

        float ang = Random.value * 360;
        Vector3 pos;

        float dist = Random.Range(minDist, minDist * 2);
        pos.x = origin.x + dist * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = origin.y;
        pos.z = origin.z + dist * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;

    }
}
