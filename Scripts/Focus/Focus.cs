using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace ReactiveMiseEnScene
{
    public class Focus : MonoBehaviour
    {
        [Header("Thresholds")]
        // Set max distance object can be interacted with by camera.
        [Range(0.0f, 1000.0f)] [SerializeField] private double distanceThreshold = 50.0f;

        // Threshold Values (inc. defaults) - these are terrible vec2 dist related, can we normalize somehow? Also maybe should be calculated from display resolution?
        [Range(0.0f, 1000.0f)] [SerializeField] private double focusedThreshold = 100f;
        [Range(0.0f, 1000.0f)] [SerializeField] private double attendedThreshold = 400f;

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
        [Header("Debug Variables")]
        [Tooltip("If the interactions aren't triggering as expected, e.g. with a custom camera setup, with multiple colliders, or with nested models in an object hierarchy, you can specify the camera, collider and mesh renderer here.")]
        public Camera cam;
        public Collider specifiedCollider;
        public Renderer specifiedMeshRenderer;
        private Bounds meshBounds;

        // Start is called before the first frame update
        void Start()
        {
            //gameObject.layer = 10; // Set alternative to Physics.AllLayers to hit only Focus.
            CamSetup();
            GetCollider();
            GetRenderer();
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: This getrenderer call is a fix/hack right now, the issue is we need bounds, but this method needs to keep updating e.g. when physics are involved, otherwise events don't fire etc.
            //if((rigidBody && !rigidBody.IsSleeping()) || (GetComponent<Animation>() && GetComponent<Animation>().isPlaying))
            GetRenderer();
            GetCollider();
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
            if (specifiedMeshRenderer)
            {
                meshBounds = specifiedMeshRenderer.bounds;
            }
            else if (gameObject.GetComponent<Renderer>())
            {
                specifiedMeshRenderer = gameObject.GetComponent<Renderer>();
                meshBounds = specifiedMeshRenderer.bounds;
            }
            else
            {
                specifiedMeshRenderer = GetComponentInChildren<Renderer>();
                meshBounds = specifiedMeshRenderer.bounds;
                // code below from: https://answers.unity.com/questions/17968/finding-the-bounds-of-a-grouped-model.html
                // need to change code later in here to use this combined bounds.
                var combinedBounds = meshBounds;
                var renderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer render in renderers)
                {
                    if (render != specifiedMeshRenderer) combinedBounds.Encapsulate(render.bounds);
                }
                meshBounds = combinedBounds;
            }
        }

        private void GetCollider()
        {
            if (specifiedCollider) // if customCollider is specified in inspector
            {
                return;
            }
            else if (GetComponent<Collider>()) // look for collider on gameObject
            {
                specifiedCollider = GetComponent<Collider>();
            }
            else
            {
                specifiedCollider = GetComponentInChildren<Collider>(); // get the first collider found in children - not ideal.
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
            if (Physics.Linecast(cam.transform.position, meshBounds.center, out RaycastHit hit, 1 << 0, QueryTriggerInteraction.Ignore) // Physics.AllLayers replaced with 1 << 10 for focus? 1 << 0 = default
                && hit.collider == specifiedCollider)
            {
                Debug.DrawLine(cam.transform.position, meshBounds.center, Color.green);
                return true;
            }
            else
            {
                Debug.DrawLine(cam.transform.position, meshBounds.center, Color.red);
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
    }
}