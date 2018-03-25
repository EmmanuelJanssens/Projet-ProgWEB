using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public ImportElementIntoPanel TreeElements;

    public TreePrototype[] DrawableTrees;
    // Use this for initialization
    public List<TreeInstance> instances;

    public int NumberofTrees = 1000;

    public void Generate()
    {
        DrawableTrees = new TreePrototype[TreeElements.ReadyElements.Count];
        instances = new List<TreeInstance>();
        AppManager.Get.NoiseMap.terrainData.treeInstances = instances.ToArray();


        for (int i = 0; i < DrawableTrees.Length; i++)
        {
            CTree tree = TreeElements.ReadyElements[i] as CTree;

            DrawableTrees[i] = new TreePrototype();
            DrawableTrees[i].prefab = tree.obj;

        }

        AppManager.Get.NoiseMap.terrainData.treePrototypes = DrawableTrees;
        AppManager.Get.NoiseMap.terrainData.RefreshPrototypes();

        System.Random prng = new System.Random(10);

        for (int i = 0; i < TreeElements.ReadyElements.Count; i++)
        {
            CTree tree = TreeElements.ReadyElements[i] as CTree;

            for ( int j = 0; j < tree.influence; j++)
            {               
                TreeInstance newtree = new TreeInstance();
                               
                newtree.prototypeIndex = i;
                newtree.color = Color.white;
                newtree.lightmapColor = Color.white;
                newtree.heightScale = Random.RandomRange(0.5f,0.8f);
                newtree.widthScale = newtree.heightScale;


                Vector3 pos = new Vector3((float)prng.NextDouble(), (float)prng.NextDouble(), (float)prng.NextDouble());
                newtree.position = new Vector3(pos.x, pos.y,pos.z);
                newtree.position.y = AppManager.Get.NoiseMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z)/AppManager.Get.NoiseMap.WorldScale;


                switch (tree.Mode)
                {
                    case CObject.ApplicationMode.Height:
                        float treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        while (treeheightpos < tree.height)
                        {
                            pos = new Vector3((float)prng.NextDouble(), (float)prng.NextDouble(), (float)prng.NextDouble());
                            newtree.position = new Vector3(pos.x, pos.y, pos.z);
                            newtree.position.y = AppManager.Get.NoiseMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z) / AppManager.Get.NoiseMap.WorldScale;
                            treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        }
                        break;
                    case CObject.ApplicationMode.Slope:
                        float steepness = AppManager.Get.NoiseMap.terrainData.GetSteepness(newtree.position.x , newtree.position.y);

                        while (steepness > tree.steepness)
                        {
                            pos = new Vector3((float)prng.NextDouble(), (float)prng.NextDouble(), (float)prng.NextDouble());
                            newtree.position = new Vector3(pos.x, pos.y, pos.z);
                            newtree.position.y = AppManager.Get.NoiseMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z) / AppManager.Get.NoiseMap.WorldScale;
                            steepness = AppManager.Get.NoiseMap.terrainData.GetSteepness(newtree.position.x, newtree.position.y);
                        }

                        break;
                    case CObject.ApplicationMode.Orientation:

                        break;
                    case CObject.ApplicationMode.HeightRange:
                        treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        while (treeheightpos > tree.maxheight || treeheightpos < tree.minheight)
                        {
                            pos = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                            newtree.position = new Vector3(pos.x, pos.y, pos.z);
                            newtree.position.y = AppManager.Get.NoiseMap.terrainData.GetInterpolatedHeight(newtree.position.x, newtree.position.z) / AppManager.Get.NoiseMap.WorldScale;
                            treeheightpos = newtree.position.y * AppManager.Get.NoiseMap.WorldScale;
                        }
                        break;
                    case CObject.ApplicationMode.SlopeRange:

                        break;
                    default:
                        break;
                }

                if(newtree.position.y > AppManager.Get.WaterMap.ScaledLevel)
                {
                    instances.Add(newtree);
                }
            }
           
        }

        Debug.Log(instances.Count);
        AppManager.Get.NoiseMap.terrainData.treeInstances = instances.ToArray();
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
