using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Places trees randomly on the map
/// 
/// ///TODO
/// Generate a noisemap that will be used to control forest density
/// </summary>
public class VegetationGenerator: MonoBehaviour
{

    /// <summary>
    /// Elements of the vegetation assets that where imported
    /// </summary>
    public ChooseAssets Import;

    /// <summary>
    /// Parent transform for the trees
    /// </summary>
    public Transform TTrees;

    /// <summary>
    /// Parent transform for the plants
    /// </summary>
    public Transform TPlants;

    /// <summary>
    /// Poisson disk algorythm
    /// </summary>
    public PoissonDisk disk;

    /// <summary>
    /// List of the coroutines that are active
    /// </summary>
    public List<Coroutine> Coroutines;

    /// <summary>
    /// List of the plants that where selected
    /// </summary>
    public List<GDPlant> Plants;

    public void Start()
    {
        disk = new PoissonDisk();
    }

    /// <summary>
    /// Generates a single type of plant
    /// One plant uses one coroutine
    /// with that when multiplie plants are generated they are generated all together 
    /// 
    /// Sequence 
    /// -- generates all positions 
    /// -- go through all the positions and check if the position is valid
    /// -- add them to a list of available position
    /// -- Instantiates object at that valid position
    /// </summary>
    /// <param name="plant"></param>
    /// <returns></returns>
    public IEnumerator GenerateSingle(GDPlant plant)
    {
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> availablePositions = new List<Vector3>();

        //Wait until all the positions are generated
        yield return StartCoroutine(disk.Generate(positions, AppManager.Get.NoiseMap.Width, AppManager.Get.NoiseMap.Height, plant.details_resolution, plant.clump, plant.density));

        TerrainData terrainData = AppManager.Get.SplatMap.terrainData; ;
        float OceanLevel = AppManager.Get.WaterMap.OceanLevel;

        //Process all the positions 
        //add them to a new list that contains all the possible positions
        for (int j = 0; j < positions.Count; j++)
        {

            Vector3 newpos = new Vector3(positions[j].x, positions[j].y, positions[j].z);
            newpos.y = terrainData.GetInterpolatedHeight(newpos.x, newpos.z);

            bool canAdd = false;
            switch (plant.Mode)
            {
                case GDObject.ApplicationMode.Height:
                    canAdd = newpos.y < plant.height;
                    break;
                case GDObject.ApplicationMode.Slope:
                    float steepness = terrainData.GetSteepness(newpos.x, newpos.y);
                    canAdd = steepness > plant.steepness;
                    break;
                case GDObject.ApplicationMode.Orientation:
                    break;
                case GDObject.ApplicationMode.HeightRange:
                    canAdd = newpos.y < plant.maxheight && newpos.y > plant.minheight;
                    break;
                case GDObject.ApplicationMode.SlopeRange:
                    break;
                default:
                    break;
            }
            if (newpos.y > OceanLevel && canAdd)
            {
                availablePositions.Add(newpos);             
            }    
        }

        //process the new position list
        for (int i = 0; i < availablePositions.Count; i++)
        {
            GameObject clone = Instantiate(plant.obj);
            COObject data = clone.GetComponent<COObject>();

            if (data != null)
            {
                data.ID = i;
                data.name = plant.ObjectName;
                switch (data.type)
                {
                    case COObject.TYPE.TREE:
                        clone.transform.SetParent(TTrees);
                        break;
                    case COObject.TYPE.FLOWER:
                        clone.transform.SetParent(TPlants);
                        break;
                    case COObject.TYPE.ROCK:
                        break;
                    default:
                        clone.transform.SetParent(TTrees);
                        break;
                }
            }
            else
            {
                ///Tree object default parent
                clone.transform.SetParent(TTrees);

            }

            clone.transform.localScale = new Vector3(1, Random.Range(0.5f, 1f), 1);
            clone.transform.localRotation = new Quaternion(0, Random.Range(0, 360), 0, 0);
            clone.transform.localPosition = new Vector3(availablePositions[i].x * terrainData.heightmapResolution, availablePositions[i].y, availablePositions[i].z * terrainData.heightmapResolution);

            yield return null;
        }

        positions.Clear();
        availablePositions.Clear();

        yield return null;
    }
    public IEnumerator Generate()
    {
        //Stop current coroutines
        if(Coroutines != null)
        {
            for(int i = 0; i < Coroutines.Count; i++)
            {
                StopCoroutine(Coroutines[i]);
            }
            Coroutines.Clear();
        }
        else
        {
            Coroutines = new List<Coroutine>();
        }

        //Clear the child list
        foreach (Transform T in TTrees)
        {
            Destroy(T.gameObject);
        }
        foreach (Transform T in TPlants)
        {
            Destroy(T.gameObject);
        }

        Plants = new List<GDPlant>();
        for (int i = 0; i < Import.ChoosenPlants.Count; i++)
        {
            GDPlant plant = Import.ChoosenPlants[i] as GDPlant;
            Coroutines.Add(StartCoroutine(GenerateSingle(plant)));
            Plants.Add(plant);
        }

        yield return null;
    }




}
