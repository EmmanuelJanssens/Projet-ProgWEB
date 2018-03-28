using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportVegetationIntoPanel : ImportElementIntoPanel
{
    public override void Init()
    {
        base.Init();
        cmdStartImportation.onClick.AddListener(Prepare<CVegetation>);
    }
}
