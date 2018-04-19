using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


/// <summary>
/// Saves and load all data used to generate the exact same map
/// </summary>
public class SaveData : MonoBehaviour
{

    /// <summary>
    /// save command 
    /// </summary>
    public Button cmdSave;

    /// <summary>
    /// load command
    /// </summary>
    public Button cmdLoad;

    /// <summary>
    /// Bundles that where used
    /// </summary>
    public BundleLoader MyBundles;

    /// <summary>
    /// Assets to import
    /// </summary>
    public ChooseAssets toImport;

    /// <summary>
    /// Name of the file to be loaded
    /// </summary>
    public string File;

    /// <summary>
    /// Initiate class and add listeners to the two commands
    /// </summary>
    public void Start()
    {
        cmdSave.onClick.AddListener(Save);
        cmdLoad.onClick.AddListener(delegate { StartCoroutine(Load()); });
    }

    /// <summary>
    /// Saves the data to a file
    /// </summary>
    public void Save()
    {
        //Directory where the files are saved
        string directory = (Path.Combine(Application.streamingAssetsPath, "saves"));

        //Full relative path of the save file
        string file = directory + "/" + File+".txt";

        //Create the directory if it does not exists
        if ( !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        StreamWriter fs = new StreamWriter(file);

        //Save the data from the noise map
        NoiseGenerator ng = AppManager.Get.NoiseMap;
        //Savew noiseMap Data
        fs.WriteLine("#NoiseMap");
        fs.WriteLine("-Width=" + ng.Width);
        fs.WriteLine("-Height="+ng.Height);
        fs.WriteLine("-Octaves="+ng.Octaves);
        fs.WriteLine("-Scale=" + ng.Scale);
        fs.WriteLine("-Lacunarity=" + ng.Lacunarity);
        fs.WriteLine("-Persistence=" + ng.Persistence);
        fs.WriteLine("-Seed=" + ng.Seed);
        fs.WriteLine("-WorldScale=" + ng.WorldScale);

        //Saves the data from the texture map
        TextureGenerator tg = AppManager.Get.SplatMap;
        fs.WriteLine("#SplatMap");
        if(tg.Textures != null)
        {
            fs.WriteLine("-Total=" + tg.Textures.Count);
            for (int i = 0; i < tg.Textures.Count; i++)
            {
                fs.WriteLine("<Texture>");
                fs.WriteLine("-Name="+tg.Textures[i].ui_sprite.name);
                switch (tg.Textures[i].Mode)
                {
                    case GDTexture.ApplicationMode.Height:
                        fs.WriteLine("-mode=Height");
                        fs.WriteLine("-Height=" + tg.Textures[i].height);
                        break;
                    case GDTexture.ApplicationMode.Slope:
                        fs.WriteLine("-mode=Slope");
                        fs.WriteLine("-steepness=" + tg.Textures[i].steepness);
                        break;
                    case GDTexture.ApplicationMode.Orientation:
                        fs.WriteLine("-mode=Orientation");
                        fs.WriteLine("-orientation=" + tg.Textures[i].orientation);
                        break;
                    case GDTexture.ApplicationMode.HeightRange:
                        fs.WriteLine("-mode=HeightRange");
                        fs.WriteLine("-minheight=" + tg.Textures[i].minheight);
                        fs.WriteLine("-maxheight=" + tg.Textures[i].maxheight);
                        break;
                    case GDTexture.ApplicationMode.SlopeRange:
                        fs.WriteLine("-mode=SlopeRange");
                        fs.WriteLine("-minslope=" + tg.Textures[i].minslope);
                        fs.WriteLine("-maxslope=" + tg.Textures[i].maxslope);
                        break;
                    default:
                        break;
                }
                fs.WriteLine("</Texture>");
            }
        }

        //Save the date from the lakes
        LakeGenerator lg = AppManager.Get.WaterMap;
        fs.WriteLine("#Lakes");
        fs.WriteLine("-OceanLevel=" + lg.OceanLevel);

        //Saves data from vegetation
        VegetationGenerator vg = AppManager.Get.PlantMap;
        fs.WriteLine("#Vegetation");
        if(vg.Plants != null)
        {
            fs.WriteLine("-Total=" + vg.Plants.Count);
            for (int i = 0; i < vg.Plants.Count; i++)
            {
                fs.WriteLine("<Plant>");
                fs.WriteLine("-Name=" + vg.Plants[i].ui_sprite.name);
                switch (vg.Plants[i].Mode)
                {
                    case GDTexture.ApplicationMode.Height:
                        fs.WriteLine("-mode=Height");
                        fs.WriteLine("-Height=" + vg.Plants[i].height);
                        break;
                    case GDTexture.ApplicationMode.Slope:
                        fs.WriteLine("-mode=Slope");
                        fs.WriteLine("-steepness=" + vg.Plants[i].steepness);
                        break;
                    case GDTexture.ApplicationMode.Orientation:
                        fs.WriteLine("-mode=Orientation");
                        fs.WriteLine("-orientation=" + vg.Plants[i].orientation);
                        break;
                    case GDTexture.ApplicationMode.HeightRange:
                        fs.WriteLine("-mode=HeightRange");
                        fs.WriteLine("-minheight=" + vg.Plants[i].minheight);
                        fs.WriteLine("-maxheight=" + vg.Plants[i].maxheight);
                        break;
                    case GDTexture.ApplicationMode.SlopeRange:
                        fs.WriteLine("-mode=SlopeRange");
                        fs.WriteLine("-minslope=" + vg.Plants[i].minslope);
                        fs.WriteLine("-maxslope=" + vg.Plants[i].maxslope);
                        break;
                    default:
                        break;
                }
                fs.WriteLine("-density=" + vg.Plants[i].density);
                fs.WriteLine("-clump=" + vg.Plants[i].clump);
                fs.WriteLine("-resolution=" + vg.Plants[i].details_resolution);

                fs.WriteLine("</Plant>");
            }
        }
     
        fs.Close();

    }

    public IEnumerator Load()
    {
        string directory = (Path.Combine(Application.streamingAssetsPath, "saves"));
        string file = directory + "/" + File + ".txt";
        string[] lines = System.IO.File.ReadAllLines(file);

        string dataName = "";
        string value = ""; 

        NoiseGenerator ng = AppManager.Get.NoiseMap;

        string generatorName = "";


        foreach (string line in lines)
        {
            if (line[0] == '#')
            {
                generatorName = line.Substring(line.LastIndexOf('#') + 1);
            }
            else
            {
                if (generatorName == "NoiseMap")
                {
                    dataName = line.Substring(1, line.LastIndexOf('=') - 1);
                    switch (dataName)
                    {
                        case "Height":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.Height = int.Parse(value);
                            break;
                        case "Width":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.Width = int.Parse(value);
                            break;
                        case "Octaves":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.Octaves = int.Parse(value);
                            break;
                        case "Scale":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.Scale = float.Parse(value);
                            break;
                        case "Lacunarity":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.Lacunarity = float.Parse(value);
                            break;
                        case "Persistence":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.Persistence = float.Parse(value);
                            break;
                        case "Seed":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.Seed = int.Parse(value);
                            break;
                        case "WorldScale":
                            value = line.Substring(line.LastIndexOf('=') + 1);
                            ng.WorldScale = float.Parse(value);
                            break;
                    }
                }
            }

        }
        AppManager.Get.NoiseGUI.UpdateUI();
        AppManager.Get.NoiseGUI.GenerateNoiseMap();

        yield return null;
    }

}
