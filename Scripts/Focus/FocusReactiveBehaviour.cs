using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public abstract class FocusReactiveBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public virtual void FocusLevelChangeHandler(GameObject gameObject, FocusLevel currentFocusLevel, FocusLevel previousFocusLevel)
        {
            switch (currentFocusLevel)
            {
                case FocusLevel.offscreen:
                    Offscreen();
                    break;
                case FocusLevel.onscreen:
                    Onscreen();
                    break;
                case FocusLevel.attended:
                    Attended();
                    break;
                case FocusLevel.focused:
                    Focused();
                    break;
                default:
                    break;
            }
        }

        public abstract void Offscreen();
        public abstract void Onscreen();
        public abstract void Attended();
        public abstract void Focused();


        public virtual void FocusLevelStayHandler (GameObject gameObject, FocusLevel focusLevel)
        {
            switch (focusLevel)
            {
                case FocusLevel.offscreen:
                    StayOffscreen();
                    break;
                case FocusLevel.onscreen:
                    StayOnscreen();
                    break;
                case FocusLevel.attended:
                    StayAttended();
                    break;
                case FocusLevel.focused:
                    StayFocused();
                    break;
                default:
                    break;
            }
        }

        public abstract void StayOffscreen();
        public abstract void StayOnscreen();
        public abstract void StayAttended();
        public abstract void StayFocused();
    }
}
