using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportTrees : MonoBehaviour
{
    public Transform Container;

    public ImportAssets Importer;

    public void Start()
    {
        Importer = gameObject.GetComponent<ImportAssets>();
    }
}
