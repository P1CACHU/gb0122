using System;
using UnityEngine;


namespace ExampleGB
{
    public class CarView : MonoBehaviour, ICollision
    {
        public event Action<InfoCollision> CarDestruction;
        public void OnCollision(InfoCollision info)
        {
            CarDestruction?.Invoke(new InfoCollision(info.Damage * 2, info.ObjCollision, info.Dir));
        }
    }
}
