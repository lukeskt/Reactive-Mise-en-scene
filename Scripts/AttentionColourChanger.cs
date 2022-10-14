using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class AttentionColourChanger : MonoBehaviour
    {
        public AttentionTracker focusComponent;
        private float focusValue;

        public Renderer rend;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            focusValue = focusComponent.getFocusValue;
            //print(focusValue);
            ChangeColour();
        }

        private void ChangeColour ()
        {
            var mixer = Mathf.InverseLerp(1, 0, focusValue);
            Color customColour = new Color(focusValue * 0.5f, mixer, 0);
            rend.material.SetColor("_BaseColor", customColour);
        }

        private void ColourSteps ()
        {
            if (focusValue == 0)
            {
                rend.material.SetColor("_BaseColor", Color.black);
            }
            else if (focusValue < 0.7f)
            {
                rend.material.SetColor("_BaseColor", Color.green);
            }
            else if (focusValue < 0.9f)
            {
                rend.material.SetColor("_BaseColor", Color.yellow);
            }
            else if (focusValue > 0.9f)
            {
                rend.material.SetColor("_BaseColor", Color.red);
            }
        }

        private float MapValue(float inputValue, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = inputValue - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var mappedValue = toAbs + toMin;
            mappedValue = Mathf.Clamp(mappedValue, toMin, toMax);
            return mappedValue;
        }
    }
}
