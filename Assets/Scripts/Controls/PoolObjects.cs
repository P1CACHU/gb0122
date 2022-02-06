using UnityEngine;


namespace ExampleGB
{
    public class PoolObjects : MonoBehaviour
    {
        public T CreateCar<T>(GameObject obj)
        {
            GameObject tempObject = Instantiate(obj);
            return tempObject.GetComponentInChildren<T>();
        }
    }
}