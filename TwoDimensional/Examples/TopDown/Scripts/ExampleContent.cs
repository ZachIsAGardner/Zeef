using System;
using UnityEngine;

namespace Zeef.TwoDimensional.Example {

    public class ExampleContent : SingleInstance<ExampleContent> {

        [Required]
        [SerializeField] private GameObject playerPrefab;
        public static GameObject PlayerPrefab { get { return GetInstance().playerPrefab; } }
    }
}