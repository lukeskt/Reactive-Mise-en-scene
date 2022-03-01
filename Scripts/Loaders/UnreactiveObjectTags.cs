using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class UnreactiveObjectTags : MonoBehaviour
    {
        [SerializeField] public ReactiveMesSettings RMSettings;
        [SerializeField] public string locale;
        [SerializeField] public int localeIndex = 0;
        [SerializeField] public string tendency;
        [SerializeField] public int tendencyIndex = 0;
    }
}
