using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace ExampleGB
{
    public class RoomElement : MonoBehaviour, IUiElement
    {
        private TextMeshProUGUI _textObject;
        private Image _image;
        private Color _baseColor;

        public void Initialize()
        {
            _image = GetComponent<Image>();
            _textObject = GetComponentInChildren<TextMeshProUGUI>();
            _baseColor = _image.color;
        }

        public void Close()
        {
            ChangeAlpha(0);
            _textObject.text = "";
        }

        public void Show(string message)
        {
            _textObject.text = message;
            Show();
        }

        public void Show()
        {
            ChangeAlpha(1);
        }

        private void ChangeAlpha(int alpha)
        {
            var tempColor = _image.color;
            tempColor.a = alpha;
            _image.color = tempColor;
        }
    }
}