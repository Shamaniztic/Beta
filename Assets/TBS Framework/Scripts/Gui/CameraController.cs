using UnityEngine;

namespace TbsFramework.Gui
{
    public class CameraController : MonoBehaviour
    {
        public float ScrollSpeed = 15;
        public float ScrollEdge = 0.01f;
        public float SnapSpeed = 10f;

        private bool isSnapping = false;
        private Vector3 targetPosition;

        private void Update()
        {
            if (isSnapping)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * SnapSpeed);
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isSnapping = false;
                }
            }
            else
            {
                if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
                {
                    transform.Translate(transform.right * Time.deltaTime * ScrollSpeed, Space.World);
                }
                else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
                {
                    transform.Translate(transform.right * Time.deltaTime * -ScrollSpeed, Space.World);
                }
                if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
                {
                    transform.Translate(transform.up * Time.deltaTime * ScrollSpeed, Space.World);
                }
                else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
                {
                    transform.Translate(transform.up * Time.deltaTime * -ScrollSpeed, Space.World);
                }
            }
        }

        public void SnapToPosition(Vector3 position)
        {
            targetPosition = new Vector3(position.x, position.y, transform.position.z);
            isSnapping = true;
        }
    }
}