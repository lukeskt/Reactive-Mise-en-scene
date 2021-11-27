using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class FocusReactiveBehaviourExample : FocusReactiveBehaviour
    {
        public override void Offscreen() { print("Offscreen!"); }
        public override void Onscreen() { print("Onscreen!"); }
        public override void Attended() { print("Attended!"); }
        public override void Focused() { print("Focused!"); }

        public override void StayOffscreen() { }
        public override void StayOnscreen() { }
        public override void StayAttended() { }
        public override void StayFocused() { }
    }
}
