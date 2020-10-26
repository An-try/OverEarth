using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class CommandCenter : MonoBehaviour
    {
        [SerializeField] private Material _blueMaterial;
        private Material _defaultMaterial;

        private List<DamageableUI> _damageablesUI;

        private MeshRenderer _glassRenderer;

        private Material _glassMaterial
        {
            get => _glassRenderer.materials[1];
            set => _glassRenderer.materials[1] = value;
        }

        private void Awake()
        {
            _damageablesUI = GetComponentsInChildren<DamageableUI>().ToList();
            _glassRenderer = GetComponent<MeshRenderer>();

            _defaultMaterial = _glassMaterial;
        }

        private void Start()
        {
            AssignDamageableParts();
        }

        private void AssignDamageableParts()
        {
            for (int i = 0; i < _damageablesUI.Count; i++)
            {
                _damageablesUI[i].AssignDamageableParts(PlayerController.Instance.Ship);
            }
        }

        public void SetGlassMaterialToBlackWhenWarping()
        {
            _glassMaterial = _blueMaterial;
        }

        public void SetGlassMaterialToDefaultWhenEndWarping()
        {
            _glassMaterial = _defaultMaterial;
        }
    }
}
