using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CreatorKitCodeInternal 
{
    public class InventoryCharacterRender : MonoBehaviour
    {
        public RawImage TargetImage;
        public GameObject RootToRender;
 
        Camera m_Camera;
        RenderTexture m_TargetTexture;
        private CharacterControl _playerCharacter;
    
        // Start is called before the first frame update
        void Start()
        {
            var characterControl = FindObjectOfType<CharacterControl>();
            if (characterControl.photonView.IsMine)
                _playerCharacter = characterControl;

            m_TargetTexture = new RenderTexture((int)TargetImage.rectTransform.rect.width * 2, (int)TargetImage.rectTransform.rect.height * 2, 16, RenderTextureFormat.ARGB32);
        
            TargetImage.texture = m_TargetTexture;
        
            GameObject cameObject = new GameObject();
            m_Camera = cameObject.AddComponent<Camera>();
            m_Camera.enabled = false;

            m_Camera.clearFlags = CameraClearFlags.SolidColor;
            m_Camera.backgroundColor = new Color(0,0,0,0);
            m_Camera.targetTexture = m_TargetTexture;
            m_Camera.cullingMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("PlayerEquipment"));  
        }

        // Update is called once per frame
        void Update()
        {
            var playerCharacterTransform = _playerCharacter.transform;
            var playerCharacterTransformPosition = playerCharacterTransform.position;
            m_Camera.transform.position = playerCharacterTransformPosition + playerCharacterTransform.forward * 1.6f + Vector3.up * 1.5f;
            m_Camera.transform.LookAt(playerCharacterTransformPosition + Vector3.up * 1.0f);

            m_Camera.Render();
        }
    }
}