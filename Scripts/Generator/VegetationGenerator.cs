using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator: MonoBehaviour
{
    public ChooseAssets Import;

    public TreePrototype[] DrawableTrees;
    // Use this for initialization
    public List<TreeInstance> instances;


    public void Generate()
    {
        DrawableTrees = new TreePrototype[Import.ChoosenPlants.Count];
        instances = new List<TreeInstance>();
        AppManager.Get.SplatMap.terrainData.treeInstances = instances.ToArray();


        for (int i = 0; i < DrawableTrees.Length; i++)
        {
            GDPlant tree = Import.ChoosenPlants[i] as GDPlant;

            DrawableTrees[i] = new TreePrototype();
            DrawableTrees[i].prefab = tree.obj;

        }

        AppManager.Get.SplatMap.terrainData.treePrototypes = DrawableTrees;
        AppManager.Get.SplatMap.terrainData.RefreshPrototypes();

        System.Random prng = new System.Random(10);

        for (int i = 0; i < Import.ChoosenPlants.Count; i++)
        {
            GDPlant tree = Import.ChoosenPlants[i] as GDPlant;

            for (int j = 0; j < tree.influence; j++)
            {
                TreeInstance newtree = new TreeInstance();

                newtree.prototypeIndex = i;
                newtree.color = Color.white;
                newtree.lightmapColor = Color.white;
                newtree.heightScale = Random.RandomRange(0.5f, 0.8f);
                newtree.widthScale = newtree.heightScale;


                Vector3 pos = new Vector3((float)prng.NextDouble(), (float)prng.NextDouble(), (float)prng.NextDouble());
                newtree.position = new Vector3(pos.x, pos.y, pos.z);
                newtree.position.y = AppManager.Get.SplatMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z) / AppManager.Get.NoiseMap.WorldScale;


                switch (tree.Mode)
                {
                    case GDObject.ApplicationMode.Height:
                        float treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        while (treeheightpos < tree.height)
                        {
                            pos = new Vector3((float)prng.NextDouble(), (float)prng.NextDouble(), (float)prng.NextDouble());
                            newtree.position = new Vector3(pos.x, pos.y, pos.z);
                            newtree.position.y = AppManager.Get.SplatMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z) / AppManager.Get.NoiseMap.WorldScale;
                            treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        }
                        break;
                    case GDObject.ApplicationMode.Slope:
                        float steepness = AppManager.Get.SplatMap.terrainData.GetSteepness(newtree.position.x, newtree.position.y);

                        while (steepness > tree.steepness)
                        {
                            pos = new Vector3((float)prng.NextDouble(), (float)prng.NextDouble(), (float)prng.NextDouble());
                            newtree.position = new Vector3(pos.x, pos.y, pos.z);
                            newtree.position.y = AppManager.Get.SplatMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z) / AppManager.Get.NoiseMap.WorldScale;
                            steepness = AppManager.Get.SplatMap.terrainData.GetSteepness(newtree.position.x, newtree.position.y);
                        }

                        break;
                    case GDObject.ApplicationMode.Orientation:

                        break;
                    case GDObject.ApplicationMode.HeightRange:
                        treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        while (treeheightpos > tree.maxheight || treeheightpos < tree.minheight)
                        {
                            pos = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                            newtree.position = new Vector3(pos.x, pos.y, pos.z);
                            newtree.position.y = AppManager.Get.SplatMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z) / AppManager.Get.NoiseMap.WorldScale;
                            treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        }
                        break;
                    case GDObject.ApplicationMode.SlopeRange:

                        break;
                    default:
                        break;
                }

                if (newtree.position.y > AppManager.Get.WaterMap.ScaledLevel)
                {
                    instances.Add(newtree);
                }
            }

        }

        AppManager.Get.SplatMap.terrainData.treeInstances = instances.ToArray();
    }


    /*for(int y = 0;  y<AppManager.Get.NoiseMap.terrainData.alphamapHeight; y++)
           {
               for(int x = 0; x<AppManager.Get.NoiseMap.terrainData.alphamapWidth; x++)
               {
                   float y_01 = (float)y / (float)AppManager.Get.NoiseMap.terrainData.alphamapHeight;
   float x_01 = (float)x / (float)AppManager.Get.NoiseMap.terrainData.alphamapWidth;

   int ix = x / 2;
   int iy = y / 2;

   float height = AppManager.Get.NoiseMap.terrainData.GetHeight(Mathf.RoundToInt(y_01 * AppManager.Get.NoiseMap.terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * AppManager.Get.NoiseMap.terrainData.heightmapWidth));                    //Get Steepness of the terrain
   float steepness = AppManager.Get.NoiseMap.terrainData.GetSteepness(x_01, y_01);
   Vector3 direction = AppManager.Get.NoiseMap.terrainData.GetInterpolatedNormal(x, y);

                   if (height > 80)
                   {
                       Debug.Log("yolo");
                       break;
                   }
               }
           }*/

}
