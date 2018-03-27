using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class ImportElementIntoPanel : MonoBehaviour
{
    public CObjectsToGenerate toGenerate;

    public Button cmdStartImportation;

    public List<CObject> ReadyElements;
    public bool ElementsAreReady { get; set; }

    public GameObject elementFramePropreties;
    public Text txtElementTitle;
    public Image imgElement;

    public Button[] cmdOpenElementFrame;


    public ListManager ElementList;

    public virtual void Init()
    {
        ElementList.onEvent.onDelete += RemoveFromReady;
        ElementList.onEvent.onMoveDown += UpdateElementID;
        ElementList.onEvent.onMoveUp += UpdateElementID;
    }

    public void Start()
    {
        ReadyElements = new List<CObject>();
        Init();
    }

    public void Prepare<T>() where T : CObject
    {
        StartCoroutine(IPrepare<T>());
    }

    public void OpenPropreties<T>(int id) where T : CObject
    {
        ElementPropreties proprs = elementFramePropreties.GetComponent<ElementPropreties>();

        if (typeof(T) == typeof(CTree))
        {
            proprs.toModify = ReadyElements[id] as CTree;
        }
        if (typeof(T) == typeof(CTexture))
        {
            proprs.toModify = ReadyElements[id] as CTexture;

        }

        txtElementTitle.text = ReadyElements[id].ObjectName;
        imgElement.sprite = ReadyElements[id].ui_sprite;

        UIManager.Get.OpenFrame(elementFramePropreties);
    }

    public IEnumerator IPrepare<T>() where T : CObject
    {
        ElementsAreReady = false;
        yield return StartCoroutine(toGenerate.ImportObjectsToGenerate<T>());

        cmdOpenElementFrame = toGenerate.Container.GetComponentsInChildren<Button>(true);

        for ( int i = 0; i < toGenerate.UIObjectAdded.Count; i++)
        {

            if(typeof(T) == typeof(CTree))
            {
                CTree toPrepare = new CTree() ;
                CTree template = toGenerate.AllAddedCustomObject[i] as CTree;

                toPrepare.ObjectName = template.ObjectName;
                toPrepare.ui_sprite = template.ui_sprite;
                toPrepare.Mode = CObject.ApplicationMode.Height;
                toPrepare.steepness = 0f;
                toPrepare.height = 0f;
                toPrepare.orientation = 0f;
                toPrepare.avgColor = AverageColorFromTexture(template.ui_sprite.texture);
                toPrepare.avgColor.a = 1;
                toPrepare.obj = template.obj;
                toPrepare.Description = template.Description;

                ReadyElements.Add(toPrepare);
            }
            else if( typeof(T) == typeof(CTexture))
            {
                CTexture toPrepare = new CTexture();
                CTexture template = toGenerate.AllAddedCustomObject[i] as CTexture;

                toPrepare.ObjectName = template.ObjectName;
                toPrepare.ui_sprite = template.ui_sprite;
                toPrepare.Mode = CTexture.ApplicationMode.Height;
                toPrepare.steepness = 0f;
                toPrepare.height = 0f;
                toPrepare.orientation = 0f;
                toPrepare.avgColor = AverageColorFromTexture(template.ui_sprite.texture);
                toPrepare.avgColor.a = 1;


                toPrepare.texture = template.texture;
                ReadyElements.Add(toPrepare);
            }

            ElementList.Init();

            // ADD event to OPEN/REMOVE BUTTON
            for (int j = 0; j < cmdOpenElementFrame.Length; j++)
            {
                ListElement linked = cmdOpenElementFrame[j].GetComponentInParent<ListElement>();

                if (cmdOpenElementFrame[j].name == "Propreties")
                {
                    if (typeof(T) == typeof(CTree))
                    {
                        cmdOpenElementFrame[j].onClick.AddListener(delegate { OpenPropreties<CTree>(linked.ID); });

                    }
                    if (typeof(T) == typeof(CTexture))
                    {
                        cmdOpenElementFrame[j].onClick.AddListener(delegate { OpenPropreties<CTexture>(linked.ID); });

                    }
                }
            }
        }

        UIManager.Get.CloseFrame();
        ElementsAreReady = true;
        yield return null;
    }

    public void RemoveFromReady(int index)
    {
        ReadyElements.RemoveAt(index);
    }
    
    public void UpdateElementID(int id)
    {
        for(int i = 0; i < ElementList.Elements.Count; i++)
        {
            ReadyElements[i].ID = ElementList.Elements[i].ID;
        }
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
        return new Color((r / total), (g / total), (b / total), 0);
    }
}
