using UnityEngine;


namespace ExampleGB
{
    [CreateAssetMenu(fileName = "New Car", menuName = "SObject/Car")]
    public class CarSObject : ScriptableObject
    {
        public GameObject CarPrefab;

        public float MotorTorque;
        public float FirstGear;
        public float SecondGear;
        public float ThirdGear;
        public float BackGear;
    }
}
