using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class FocusReactiveBehaviourExample : FocusReactiveBehaviour
    {
        public override void Offscreen() { print($"{gameObject.name} is offscreen!"); }
        public override void Onscreen() { print($"{gameObject.name} is onscreen!"); }
        public override void Attended() { print($"{gameObject.name} is attended!"); }
        public override void Focused() { print($"{gameObject.name} is focused!"); }

        public override void StayOffscreen() { }
        public override void StayOnscreen() { }
        public override void StayAttended() { }
        public override void StayFocused() { }
    }
}
