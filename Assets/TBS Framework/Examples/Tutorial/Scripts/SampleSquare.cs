using TbsFramework.Cells;
using UnityEngine;

namespace TbsFramework.Tutorial
{
    public class SampleSquare : Square
    {
        public GameObject highlightedPrefab;
        public GameObject pathPrefab;
        public GameObject reachablePrefab;
        public GameObject attackRangePrefab;

        private GameObject currentMarkingPrefab;

        public override Vector3 GetCellDimensions()
        {
            return GetComponent<Renderer>().bounds.size;
        }

        public override void MarkAsHighlighted()
        {
            ShowMarkingPrefab(highlightedPrefab);
        }

        public override void MarkAsPath()
        {
            ShowMarkingPrefab(pathPrefab);
        }

        public override void MarkAsReachable()
        {
            ShowMarkingPrefab(reachablePrefab);
        }

        public void MarkAsAttackRange()
        {
            ShowMarkingPrefab(attackRangePrefab);
        }

        public override void UnMark()
        {
            if (currentMarkingPrefab != null)
            {
                Destroy(currentMarkingPrefab);
                currentMarkingPrefab = null;
            }
        }

        private void ShowMarkingPrefab(GameObject prefab)
        {
            UnMark();
            currentMarkingPrefab = Instantiate(prefab, transform);
        }
    }
}