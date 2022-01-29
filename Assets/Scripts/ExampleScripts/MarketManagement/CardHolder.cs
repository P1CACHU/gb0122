using UnityEngine;


namespace ExampleGB
{
    [CreateAssetMenu(fileName = nameof(CardHolder), menuName = "Scriptable/" + nameof(CardHolder))]
    public class CardHolder : ScriptableObject
    {
        public CardProperties[] Cards;
    }
}