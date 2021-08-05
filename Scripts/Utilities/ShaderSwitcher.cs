using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSwitcher : MonoBehaviour
{
    Material defaultMaterial;
    Material lastMaterial;
    public List<Material> switchMaterial = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }

    public void SetSwitchShader (int shaderRef)
    {
        lastMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = switchMaterial[shaderRef];
    }

    public void UnsetSwitchShader ()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
