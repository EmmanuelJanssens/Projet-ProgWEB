using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class ImportTextureIntoPanel : ImportElementIntoPanel
{
    public override void Init ()
    {
        base.Init();
        cmdStartImportation.onClick.AddListener(Prepare<CTexture>);
    }
}
