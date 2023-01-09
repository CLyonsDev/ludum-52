using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RandomizeAppearance : MonoBehaviour {
    [SerializeField] private GameObject GraphicsPrefab;
    [SerializeField] private Material[] CharacterSkins;
    
    private void Start() {
        Material mat = CharacterSkins[Random.Range(0, CharacterSkins.Length)];
        foreach (Renderer rend in GraphicsPrefab.GetComponentsInChildren<Renderer>())
        {
            rend.material = mat;
        }
    }
}