using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSwitcher : MonoBehaviour
{
    Renderer objRenderer;
    Material defaultMaterial;
    Material lastMaterial;
    public List<Material> switchMaterial = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        defaultMaterial = GetComponent<Renderer>().material;
    }

    public void SetSwitchShader (int shaderRef)
    {
        lastMaterial = objRenderer.material;
        objRenderer.material = switchMaterial[shaderRef];
    }

    public void Focus2Reader(GameObject obj, ReactiveMiseEnScene.FocusLevel currentFocusLevel, ReactiveMiseEnScene.FocusLevel lastFocusLevel)
    {
        switch (currentFocusLevel)
        {
            case ReactiveMiseEnScene.FocusLevel.offscreen:
                SetSwitchShader(0);
                break;
            case ReactiveMiseEnScene.FocusLevel.onscreen:
                SetSwitchShader(1);
                break;
            case ReactiveMiseEnScene.FocusLevel.attended:
                SetSwitchShader(2);
                break;
            case ReactiveMiseEnScene.FocusLevel.focused:
                SetSwitchShader(3);
                break;
            default:
                break;
        }
    }

    public void UnsetSwitchShader ()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
