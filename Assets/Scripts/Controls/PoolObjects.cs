using UnityEngine;
using Photon.Pun;


namespace ExampleGB
{
    public class PoolObjects : MonoBehaviour
    {
        //public T CreateCar<T>(GameObject obj, Vector3 position, Quaternion rotation)
        public T CreateCar<T>(string obj, Vector3 position, Quaternion rotation)
        {
            GameObject tempObject = PhotonNetwork.Instantiate(obj, position, rotation);
            return tempObject.GetComponentInChildren<T>();
        }
    }
}