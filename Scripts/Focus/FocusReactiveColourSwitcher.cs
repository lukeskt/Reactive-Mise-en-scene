using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class FocusReactiveColourSwitcher : FocusReactiveBehaviour
    {
        public List<Color> switchColors = new List<Color>() { new Color(0,1,0,1), new Color(1,0.75f,0,1), new Color(1,0,0,1) };
        Color defaultColor;
        Material material;

        // Start is called before the first frame update
        void Start()
        {
            defaultColor = gameObject.GetComponent<Renderer>().material.color;
            material = gameObject.GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override void Offscreen() { material.SetColor("_BaseColor", defaultColor); }
        public override void Onscreen() { material.SetColor("_BaseColor", switchColors[0]); }
        public override void Attended() { material.SetColor("_BaseColor", switchColors[1]); }
        public override void Focused() { material.SetColor("_BaseColor", switchColors[2]); }

        public override void StayOffscreen() { }
        public override void StayOnscreen() { }
        public override void StayAttended() { }
        public override void StayFocused() { }
    }
}
