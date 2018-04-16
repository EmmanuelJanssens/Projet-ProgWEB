using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Applies Events on the buttons that opense the 
/// Selection frame
///  in witch the chosen bundle is displayed
///  
/// </summary>
public class OpenAssetSelection : MonoBehaviour
{

    public BundleLoader MyBundles;

    #region Buttons
    public Button cmdOpenTextures;
    public Button cmdOpenPlants;
    public Button cmdOpenRocks;
    #endregion

    public void Start()
    {
        cmdOpenTextures.onClick.AddListener(
            delegate
            {
                UIManager.Get.OpenFrame(UIManager.Get.SelectFrame);
                StartCoroutine(MyBundles.LoadAssync<Texture2D>("terrain"));
            });
        cmdOpenPlants.onClick.AddListener(
            delegate
            {
                UIManager.Get.OpenFrame(UIManager.Get.SelectFrame);
                StartCoroutine(MyBundles.LoadAssync<GameObject>("vegetation"));
            });
        cmdOpenRocks.onClick.AddListener(
            delegate
            {
                UIManager.Get.OpenFrame(UIManager.Get.SelectFrame);
                StartCoroutine(MyBundles.LoadAssync<GameObject>("rock"));
            });
    }
}
