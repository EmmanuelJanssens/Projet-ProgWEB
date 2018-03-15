using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportTextureIntoPanel : MonoBehaviour {

    public CObjectsToGenerate toGenerate;
    public Button cmdStartImportation;

    public List<CTexture> ReadyTextures;
    public bool TexturesAreReady { get; set; }

    public GameObject frmTextureProp;
    public Text txtTexturePropTitle;
    public Image sprTexturePropSprite;

    public Button[] cmdOpenTextureProp;

    public static int TextureIdentifier = 0;


    public ListManager textureList;

	// Use this for initialization
	void Start ()
    {
        cmdStartImportation.onClick.AddListener(PrepareTexture);
        ReadyTextures = new List<CTexture>();
	}

    public void PrepareTexture()
    {
        StartCoroutine(IPrepareTexture());
    }

    public void OpenTextureProp(int id)
    {
        TexturePropreties proprs = frmTextureProp.GetComponent<TexturePropreties>();
        proprs.toModify = ReadyTextures[id];

        txtTexturePropTitle.text = ReadyTextures[id].Name;
        sprTexturePropSprite.sprite = ReadyTextures[id].sprite;

        UIManager.Get.OpenFrame(frmTextureProp);
    }

    public IEnumerator IPrepareTexture()
    {
        yield return StartCoroutine(toGenerate.ImportObjectsToGenerate<CTexture>());

        cmdOpenTextureProp = toGenerate.Container.GetComponentsInChildren<Button>(true);

        for(int i = 0; i < toGenerate.AllObjectsToGenerate.Count; i++)
        {
            CTexture toPrepare = toGenerate.AllObjectsToGenerate[i].AddComponent<CTexture>();

            CTexture template = toGenerate.AllAddedCustomObject[i] as CTexture;

            toPrepare.Name  = template.Name;
            toPrepare.sprite = template.sprite;
            toPrepare.texture = template.texture;
            toPrepare.steepness = 0f;
            toPrepare.height = 0f;
            toPrepare.orientation = 0f;
            toPrepare.Mode = CTexture.ApplicationMode.Height;
            toPrepare.ID = TextureIdentifier;
            toPrepare.avgColor = AverageColorFromTexture(toPrepare.texture);
            ReadyTextures.Add(toPrepare);

            TextureIdentifier++;

            textureList.Init();
        }


        for (int j = 0; j < cmdOpenTextureProp.Length; j++)
        {
            CTexture linked = cmdOpenTextureProp[j].GetComponentInParent<CTexture>();
            if (cmdOpenTextureProp[j].name == "Propreties")
            {
                cmdOpenTextureProp[j].onClick.AddListener(delegate { OpenTextureProp(linked.ID); });
            }
        }

        UIManager.Get.CloseFrame();

        TexturesAreReady = true;
        yield return null;
    }

    Color AverageColorFromTexture(Texture2D tex)
    {

        Color[] texColors = tex.GetPixels();

        int total = texColors.Length;

        float r = 0;
        float g = 0;
        float b = 0;

        for (int i = 0; i < total; i++)
        {
            r += texColors[i].r;
            g += texColors[i].g;
            b += texColors[i].b;
        }

        float red = r / total;
        float green = g / total;
        float blue = b / total;
        return new Color((r / total), (g / total), (b / total), 0);

    }
}
