using UnityEngine;

namespace TbsFramework.Gui
{
    public class CameraController : MonoBehaviour
    {
        public float ScrollSpeed = 15;
        public float ScrollEdge = 0.01f;
        public float SnapSpeed = 10f;

        [SerializeField] private Vector2 horizontalBounds;
        [SerializeField] private Vector2 verticalBounds;

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

            transform.position = CorrectVectorWithinBounds(transform.position, horizontalBounds, verticalBounds);
        }

        public void SnapToPosition(Vector3 position)
        {
            targetPosition = new Vector3(position.x, position.y, transform.position.z);
            isSnapping = true;

            targetPosition = CorrectVectorWithinBounds(targetPosition, horizontalBounds, verticalBounds);
        }

        private Vector3 CorrectVectorWithinBounds(Vector3 vector, Vector2 horizontalBounds, Vector2 verticalBounds)
        {
            Vector3 result = vector;

            if (result.x < horizontalBounds.x)
            {
                Vector3 newPos = result;
                newPos.x = horizontalBounds.x;
                result = newPos;
            }
            else if (result.x > horizontalBounds.y)
            {
                Vector3 newPos = result;
                newPos.x = horizontalBounds.y;
                result = newPos;
            }

            if (result.y < verticalBounds.x)
            {
                Vector3 newPos = result;
                newPos.y = verticalBounds.x;
                result = newPos;
            }
            else if (result.y > verticalBounds.y)
            {
                Vector3 newPos = result;
                newPos.y = verticalBounds.y;
                result = newPos;
            }

            return result;
        }
    }
}