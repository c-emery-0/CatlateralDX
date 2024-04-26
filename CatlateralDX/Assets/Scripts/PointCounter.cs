    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class PointCounter : MonoBehaviour
    {
        public int points = 0;

        void Start() {
        }
        public void UpdatePoints(int pts)
        {
            points += pts;
        }
    }