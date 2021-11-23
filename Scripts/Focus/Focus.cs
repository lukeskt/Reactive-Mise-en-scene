using System;
using UnityEngine;
using UnityEngine.Events;

namespace ReactiveMiseEnScene
{
    public enum FocusLevel
    {
        offscreen,
        onscreen,
        attended,
        focused
    }

    public class Focus : MonoBehaviour
    {
        // Focus Levels
        [HideInInspector] public FocusLevel currentFocusLevel;
        [HideInInspector] public FocusLevel lastFocusLevel;

        // Focus Level Thresholds
        [Header("Thresholds")]
        // Set max distance object can be interacted with by camera.
        [Range(0.0f, 1000.0f)] [SerializeField] private double distanceThreshold = 50.0f;
        // Viewport thresholds from 0f to 1f - not ideal but better than screen res thresholds, more consistent.
        [Range(0.0f, 1.0f)] [SerializeField] private double focusedThreshold = 0.075f;
        [Range(0.0f, 1.0f)] [SerializeField] private double attendedThreshold = 0.25f;

        // Focus Level Events: This Object, it's current focus level, bool of true if same as last?
        [Header("Events")]
        public UnityEvent<GameObject, FocusLevel, FocusLevel> FocusLevelChange;
        public UnityEvent<GameObject, FocusLevel> FocusLevelStay;

        // Debug / Fix Variables
        [Header("Debug Variables")]
        [Tooltip("If the interactions aren't triggering as expected, e.g. with a custom camera setup, with multiple colliders, or with nested models in an object hierarchy, you can specify the camera, collider and mesh renderer here.")]
        public Camera cam;
        public Collider specifiedCollider;
        public Renderer specifiedMeshRenderer;
        private Bounds meshBounds;

        // Start is called before the first frame update
        void Start()
        {
            CamSetup();
            GetObjectCollider();
            GetObjectRenderer();
        }

        private void GetObjectRenderer()
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

        private void GetObjectCollider()
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

        private void CamSetup()
        {
            if (!cam)
            {
                GameObject Viewer = GameObject.FindGameObjectWithTag("Player");
                if (Viewer)
                {
                    if (Viewer.GetComponent<Camera>()) cam = Viewer.GetComponent<Camera>();
                    else if (Viewer.GetComponentInChildren<Camera>()) cam = Viewer.GetComponentInChildren<Camera>();
                    else if (Viewer.GetComponentInParent<Camera>()) cam = Viewer.GetComponentInParent<Camera>();
                }
                else cam = Camera.main;
            }
        }

        // Update is called once per frame
        void Update()
        {
            GetObjectCollider();
            GetObjectRenderer();
            GetFocusLevel();
        }

        private bool ObjectFrustrumCheck()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, meshBounds)) return true;
            else return false;
        }

        private bool ObjectLineOfSightCheck()
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

        private bool ObjectDistanceCheck()
        {
            if (Vector3.Distance(meshBounds.center, cam.transform.position) < distanceThreshold) return true;
            else return false;
        }

        private void GetFocusLevel()
        {
            //throw new NotImplementedException();
            lastFocusLevel = currentFocusLevel;
            // If object is being rendered and isn't behind another object and is close enough...
            if(ObjectFrustrumCheck() && ObjectLineOfSightCheck() && ObjectDistanceCheck())
            {
                float objectScreenCentreDistance = GetObjectScreenPosition();
                switch (objectScreenCentreDistance)
                {
                    case var _ when objectScreenCentreDistance < focusedThreshold:
                        currentFocusLevel = FocusLevel.focused;
                        break;
                    case var _ when objectScreenCentreDistance >= focusedThreshold && objectScreenCentreDistance < attendedThreshold:
                        currentFocusLevel = FocusLevel.attended;
                        break;
                    case var _ when objectScreenCentreDistance >= attendedThreshold:
                        currentFocusLevel = FocusLevel.onscreen;
                        break;
                    default:
                        currentFocusLevel = FocusLevel.offscreen;
                        break;
                }
            }
            else currentFocusLevel = FocusLevel.offscreen;

            if (currentFocusLevel == lastFocusLevel) FocusLevelStay.Invoke(gameObject, currentFocusLevel);
            else FocusLevelChange.Invoke(gameObject, currentFocusLevel, lastFocusLevel);
        }

        private float GetObjectScreenPosition()
        {
            // TODO: compare doing this inversely, i.e. viewporttoworldpoint? - might have better results?
            Vector2 gameObjectViewportPosition = cam.WorldToViewportPoint(meshBounds.center);
            Vector2 viewportCentre = new Vector2(0.5f, 0.5f);
            float dist = Vector2.Distance(gameObjectViewportPosition, viewportCentre);
            return dist;
        }

        private void OnDisable()
        {
            // Event Cleanup?
            currentFocusLevel = FocusLevel.offscreen;
            FocusLevelChange.Invoke(gameObject, currentFocusLevel, lastFocusLevel);
        }
    }
}
