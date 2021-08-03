using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace ReactiveMedia
{
    public class Focus : MonoBehaviour
    {
        // Public
        public Camera cam;

        [Header("Thresholds")]
        // Set max distance object can be interacted with by camera.
        [Range(0.0f, 1000.0f)] [SerializeField] private float distanceThreshold = 50.0f;

        // Threshold Values (inc. defaults) - these are terrible vec2 dist related, can we normalize somehow?
        [Range(0.0f, 1000.0f)] [SerializeField] private float focusedThreshold = 100f;
        [Range(0.0f, 1000.0f)] [SerializeField] private float attendedThreshold = 400f;

        // Events
        [Header("Event Handling")]
        public UnityEvent<GameObject> EnterVisible;
        public UnityEvent<GameObject> StayVisible;
        public UnityEvent<GameObject> ExitVisible;

        public UnityEvent<GameObject> EnterAttention;
        public UnityEvent<GameObject> StayAttention;
        public UnityEvent<GameObject> ExitAttention;

        public UnityEvent<GameObject> EnterFocus;
        public UnityEvent<GameObject> StayFocus;
        public UnityEvent<GameObject> ExitFocus;

        // Private
        private enum Visibility
        {
            invisible,
            visible,
            attended,
            focused,
        }
        private Visibility currentVisbility;
        private Visibility lastVisibility = Visibility.invisible;

        // Other Vars - e.g. for debug/gizmos
        //private Rigidbody rigidBody;
        public Renderer meshRenderer;
        private Bounds meshBounds;

        // Start is called before the first frame update
        void Start()
        {
            //if (GetComponent<Rigidbody>())
            //{
            //    rigidBody = GetComponent<Rigidbody>();
            //}
            CamSetup();
            GetRenderer();
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: This getrenderer call is a fix/hack right now, the issue is we need bounds, but this method needs to keep updating e.g. when physics are involved, otherwise events don't fire etc.
            //if((rigidBody && !rigidBody.IsSleeping()) || (GetComponent<Animation>() && GetComponent<Animation>().isPlaying))
            GetRenderer();
            ThresholdCheck();
        }

        private void CamSetup()
        {
            if (!cam)
            {
                GameObject Viewer = GameObject.FindGameObjectWithTag("Player");
                if (Viewer)
                {
                    if (Viewer.GetComponent<Camera>())
                    {
                        cam = Viewer.GetComponent<Camera>();
                    }
                    else if (Viewer.GetComponentInChildren<Camera>())
                    {
                        cam = Viewer.GetComponentInChildren<Camera>();
                    }
                    else if (Viewer.GetComponentInParent<Camera>())
                    {
                        cam = Viewer.GetComponentInParent<Camera>();
                    }
                }
                else
                {
                    cam = Camera.main;
                }
            }
        }

        private void GetRenderer()
        {
            if (meshRenderer)
            {
                meshBounds = meshRenderer.bounds;
            }
            else if (gameObject.GetComponent<Renderer>())
            {
                meshRenderer = gameObject.GetComponent<Renderer>();
                meshBounds = meshRenderer.bounds;
            }
            else
            {
                meshRenderer = GetComponentInChildren<Renderer>();
                meshBounds = meshRenderer.bounds;
                // code below from: https://answers.unity.com/questions/17968/finding-the-bounds-of-a-grouped-model.html
                // need to change code later in here to use this combined bounds.
                var combinedBounds = meshBounds;
                var renderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer render in renderers)
                {
                    if (render != meshRenderer) combinedBounds.Encapsulate(render.bounds);
                }
                meshBounds = combinedBounds;
            }
        }

        private bool VisibleInCameraFrustrumCheck()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, meshBounds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SightlineCheck()
        {
            Debug.DrawLine(cam.transform.position, meshBounds.center, Color.red);
            if (Physics.Linecast(cam.transform.position,
                                 meshBounds.center,
                                 out RaycastHit hit, Physics.AllLayers, QueryTriggerInteraction.Ignore) &&
                hit.collider.name == gameObject.name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // TODO: compare doing this inversely, i.e. screentoworldpoint? - might have better results?
        private float ObjectToCamCentreDistance()
        {
            Vector2 gameObjectScreenPos = cam.WorldToScreenPoint(meshBounds.center); // alt:gameObject.transform.position);
            Vector2 camCentre = new Vector2(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2);
            float dist = Vector2.Distance(gameObjectScreenPos, camCentre);
            return dist;
        }

        private bool ObjectCloseEnough()
        {
            if (Vector3.Distance(meshBounds.center, cam.transform.position) < distanceThreshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ThresholdCheck()
        {
            // Set last state of visibility before updating to new.
            lastVisibility = currentVisbility;
            // If object is visible in camera frustrum check...
            if (VisibleInCameraFrustrumCheck() && ObjectCloseEnough() && SightlineCheck())
            {
                // Get dist of obj on screen from center
                float objDist = ObjectToCamCentreDistance();
                // Focus
                if (objDist < focusedThreshold)
                {
                    currentVisbility = Visibility.focused;

                    if (currentVisbility == lastVisibility) { StayFocus.Invoke(gameObject); }
                    else { EnterFocus.Invoke(gameObject); }
                }
                // Attention
                else if (objDist > focusedThreshold && objDist < attendedThreshold)
                {
                    currentVisbility = Visibility.attended;
                    if (lastVisibility == Visibility.focused) { ExitFocus.Invoke(gameObject); }

                    if (currentVisbility == lastVisibility) { StayAttention.Invoke(gameObject); }
                    else { EnterAttention.Invoke(gameObject); }
                }
                // Visible
                else if (objDist > attendedThreshold)
                {
                    currentVisbility = Visibility.visible;
                    if (lastVisibility == Visibility.attended) { ExitAttention.Invoke(gameObject); }

                    if (currentVisbility == lastVisibility) { StayVisible.Invoke(gameObject); }
                    else { EnterVisible.Invoke(gameObject); }
                }
                // Invisible
                else
                {
                    currentVisbility = Visibility.invisible;
                    if (lastVisibility == Visibility.visible) { ExitVisible.Invoke(gameObject); }
                }
            }
            // Invisible
            else
            {
                currentVisbility = Visibility.invisible;
                if (lastVisibility == Visibility.focused)
                {
                    ExitFocus.Invoke(gameObject);
                    ExitAttention.Invoke(gameObject);
                    ExitVisible.Invoke(gameObject);
                }
                if (lastVisibility == Visibility.attended)
                {
                    ExitAttention.Invoke(gameObject);
                    ExitVisible.Invoke(gameObject);
                }
                if (lastVisibility == Visibility.visible)
                {
                    ExitVisible.Invoke(gameObject);
                }
            }
        }

        private void EventCleanup(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.invisible:
                    break;
                case Visibility.visible:
                    ExitVisible.Invoke(gameObject);
                    break;
                case Visibility.attended:
                    ExitAttention.Invoke(gameObject);
                    ExitVisible.Invoke(gameObject);
                    break;
                case Visibility.focused:
                    ExitFocus.Invoke(gameObject);
                    ExitAttention.Invoke(gameObject);
                    ExitVisible.Invoke(gameObject);
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            EventCleanup(currentVisbility);
        }

        // old focus code here for ref in case needed...
        //private void VisibilityHandler()
        //{
        //    // TODO: Handle previous vis properly, enable Stay events
        //    lastVisibility = currentVisbility;
        //    if (PinpointCheck())
        //    {
        //        currentVisbility = Visibility.focused;
        //        if (currentVisbility == lastVisibility)
        //        {
        //            StayFocus.Invoke(gameObject); // this keeps calling, need to call once...
        //        }
        //        else
        //        {
        //            EnterFocus.Invoke(gameObject);
        //        }
        //    }
        //    else if (AttentionCheck() && LineOfSightCheck())
        //    {
        //        currentVisbility = Visibility.attended;
        //        ExitFocus.Invoke(gameObject);
        //        if (currentVisbility == lastVisibility)
        //        {
        //            StayAttention.Invoke(gameObject); // this keeps calling, need to call once...
        //        }
        //        else
        //        {
        //            EnterAttention.Invoke(gameObject);
        //        }
        //    }
        //    else if (LineOfSightCheck())
        //    {
        //        currentVisbility = Visibility.visible;
        //        ExitAttention.Invoke(gameObject);
        //        if (currentVisbility == lastVisibility)
        //        {
        //            StayVisible.Invoke(gameObject); // this keeps calling, need to call once...
        //        }
        //        else
        //        {
        //            EnterVisible.Invoke(gameObject);
        //        }
        //    }
        //    else
        //    {
        //        currentVisbility = Visibility.invisible;
        //        ExitVisible.Invoke(gameObject);
        //    }
        //}

        //private bool AttentionCheck()
        //{
        //    Vector3 gameObjectScreenPos = cam.WorldToScreenPoint(gameObject.transform.position);
        //    (int displayW, int displayH) = (Display.main.renderingWidth, Display.main.renderingHeight);
        //    Vector2 centrepoint = new Vector2(displayW / 2, displayH / 2);
        //    int widthDivider = 4;
        //    int heightDivider = 4;
        //    int attentionZDist = 100;
        //    if (centrepoint.x - (displayW / widthDivider) < gameObjectScreenPos.x && gameObjectScreenPos.x < centrepoint.x + (displayW / widthDivider) &&
        //        centrepoint.y - (displayH / heightDivider) < gameObjectScreenPos.y && gameObjectScreenPos.y < centrepoint.y + (displayH / heightDivider) &&
        //        0 < gameObjectScreenPos.z && gameObjectScreenPos.z < attentionZDist)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private bool PinpointCheck()
        //{
        //    Debug.DrawRay(cam.transform.position, cam.transform.forward);
        //    if (Physics.Raycast(cam.transform.position, 
        //                        cam.transform.forward, 
        //                        out RaycastHit hit, 
        //                        pinpointRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore) &&
        //        hit.collider.gameObject == gameObject)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        // maybe replace with dot product angle check? could be neater. see infallible code vid.
        // Also look to using field of view instead of display render?


        //private bool LineOfSightCheck()
        //{
        //    if (VisibleInCameraFrustrumCheck())
        //    {
        //        Debug.DrawLine(cam.transform.position, gameObject.GetComponent<Renderer>().bounds.center, Color.red);
        //        if (Physics.Linecast(cam.transform.position,
        //                             gameObject.GetComponent<Renderer>().bounds.center,
        //                             out RaycastHit hit, Physics.AllLayers, QueryTriggerInteraction.Ignore) &&
        //            hit.collider.name == gameObject.name)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

    }
}