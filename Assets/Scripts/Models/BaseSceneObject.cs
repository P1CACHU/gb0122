using UnityEngine;
using UnityEngine.AI;


namespace ExampleGB
{
    public abstract class BaseSceneObject : MonoBehaviour
    {
        protected RaycastHit hit;

        [HideInInspector] public GameObject InstanceObject;
        [HideInInspector] public Rigidbody Rigidbody;
        [HideInInspector] public Transform Transform;
        [HideInInspector] public NavMeshAgent Agent;
        [HideInInspector] public Animator ObjectAnimator;
        [HideInInspector] public Camera Camera;

        protected virtual void Awake()
        {
            InstanceObject = gameObject;
            Camera = Camera.main;
            Transform = InstanceObject.transform;

            if (InstanceObject.GetComponent<Rigidbody>())
            {
                Rigidbody = InstanceObject.GetComponent<Rigidbody>();
            }

            if (InstanceObject.GetComponent<NavMeshAgent>())
            {
                Agent = InstanceObject.GetComponent<NavMeshAgent>();
            }

            if (InstanceObject.GetComponent<Animator>() != null)
            {
                ObjectAnimator = InstanceObject.GetComponent<Animator>();
            }
        }
    }
}
