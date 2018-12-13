using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoParallaxScrolling
{
    public class AutoParallaxScrolling : MonoBehaviour
    {
        private List<BalancedList<Transform>> balancedLists;
        public List<Transform> rootTransforms;
        public int numberOfBuddies = 10;
        private Vector3 previousCamPosition; //position of camera in previous frame
        private Transform camTransform; //reference to main camera transform
        public Camera parallaxCamera;


        private Vector3 speed = Vector3.zero;

        public bool parallaxHorizontal;
        public bool parallaxVertical;

        public float scaleX = 1f;
        public float scaleY = 1f;

        public float offsetX = 1f;

        public float smoothing = 1f; //how smooth the parallax is going to be - has to be > 0

        public bool tiled;

        private static ILogger logger = Debug.unityLogger;
        private bool isError;

        private bool checkSprites(List<Transform> transforms)
        {
            List<Transform> toRemove = new List<Transform>();

            foreach (var transform in transforms)
            {
                if (transform.GetComponent<SpriteRenderer>() == null)
                {
                    logger.LogWarning(
                        "Attached transform " + transform.name + " has no Sprite Renderer - Removing from Parallax",
                        "Auto Parallax", transform);
                    toRemove.Add(transform);
                }
                else
                {
                    balancedLists.Add(new BalancedList<Transform>(new BalancedNode<Transform>(transform)));
                }
            }
            return balancedLists.Count > 0;
        }


        private void Start()
        {
            if (isError) return;

            previousCamPosition = camTransform.position;

            if (checkSprites(rootTransforms))
            {
                spawnAllBuddies();
            }
            else
            {
                isError = true;
            }
        }

        private void Awake()
        {
                camTransform = Camera.main.transform;
                balancedLists = new List<BalancedList<Transform>>();
                
                rootTransforms = rootTransforms.Where(transform => transform != null).ToList();
        }

        private void Update()
        {
            if (isError) return;

            doParallax();
            if (numberOfBuddies > 0)
            {
                alignSprites();
                doBalancing();
            }
            
            previousCamPosition = camTransform.position;
        }

        private void spawnAllBuddies()
        {

            foreach (var balancedList in balancedLists)
            {
                for (var i = 0; i < numberOfBuddies; i++)
                {
                    balancedList.add(spawn_buddy(balancedList.getRootNode()), spawn_buddy(balancedList.getRootNode()));
                }
            }
        }

        private void alignAllLeft(BalancedNode<Transform> node)
        {
            alignToLeftBuddy(node);

            if (node.rightBuddy != null)
            {
                alignAllLeft(node.rightBuddy);
            }
        }

        private void alignAllRight(BalancedNode<Transform> node)
        {
            alignToRightBuddy(node);

            if (node.leftBuddy != null)
            {
                alignAllRight(node.leftBuddy);
            }
        }

        private void alignToLeftBuddy(BalancedNode<Transform> node)
        {
            SetTransformX(node, getNodePositionX(node.getLeftBuddy()) + getNodeWidth(node.getLeftBuddy()) - 0.001f);
            SetTransformY(node, getNodePositionY(node.getLeftBuddy()));
            SetTransformZ(node, getNodePositionZ(node.getLeftBuddy()));

            var leftBuddyScale = node.getLeftBuddy().payload.localScale;

            node.payload.localScale =
                tiled
                    ? new Vector3(leftBuddyScale.x * -1, leftBuddyScale.y, leftBuddyScale.z)
                    : node.getLeftBuddy().payload.localScale;
        }

        private void alignToRightBuddy(BalancedNode<Transform> node)
        {
            SetTransformX(node, getNodePositionX(node.getRightBuddy()) - getNodeWidth(node.getRightBuddy()) + 0.001f);
            SetTransformY(node, getNodePositionY(node.getRightBuddy()));
            SetTransformZ(node, getNodePositionZ(node.getRightBuddy()));

            var rightBuddyScale = node.getRightBuddy().payload.localScale;

            node.payload.localScale =
                tiled
                    ? new Vector3(rightBuddyScale.x * -1, rightBuddyScale.y, rightBuddyScale.z)
                    : node.getRightBuddy().payload.localScale;
        }


        private void alignSprites()
        {
            foreach (var balancedList in balancedLists)
            {
                alignAllLeft(balancedList.getRootNode().getRightBuddy());
                alignAllRight(balancedList.getRootNode().getLeftBuddy());
            }

        }


        private BalancedNode<Transform> spawn_buddy(BalancedNode<Transform> root)
        {
            Vector3 newPosition = new Vector3(getNodePositionX(root), getNodePositionY(root), getNodePositionZ(root));

            Transform sibling_transform = Instantiate(root.payload.transform, newPosition, root.payload.rotation);


            if (root.payload.parent != null)
            {
                sibling_transform.parent = root.payload.parent;
            }


            return new BalancedNode<Transform>(sibling_transform);
        }

        private static float getNodeWidth(BalancedNode<Transform> node)
        {
            return node.payload.GetComponent<SpriteRenderer>().sprite.bounds.size.x *
                   Mathf.Abs(node.payload.localScale.x);
        }

        private static float getNodePositionX(BalancedNode<Transform> node)
        {
            return node.payload.position.x;
        }

        private static float getNodePositionY(BalancedNode<Transform> node)
        {
            return node.payload.position.y;
        }

        private static float getNodePositionZ(BalancedNode<Transform> node)
        {
            return node.payload.position.z;
        }

        private static float getZDistanceFromCamera(Transform camTransform, BalancedNode<Transform> node)
        {
            return Vector3.Distance(camTransform.position,
                new Vector3(camTransform.position.x, camTransform.position.y, node.payload.transform.position.z));
        }

        private static void SetTransformX(BalancedNode<Transform> node, float n)
        {
            node.payload.position = new Vector3(n, node.payload.position.y, node.payload.position.z);
        }

        private static void SetTransformY(BalancedNode<Transform> node, float n)
        {
            node.payload.position = new Vector3(node.payload.position.x, n, node.payload.position.z);
        }

        private static void SetTransformZ(BalancedNode<Transform> node, float n)
        {
            node.payload.position = new Vector3(node.payload.position.x, node.payload.position.y, n);
        }

        private float getXParallax(BalancedNode<Transform> node)
        {
            return (previousCamPosition.x - camTransform.position.x) *
                   (10 / getZDistanceFromCamera(camTransform, node)) * scaleX;
        }

        private float getYParallax(BalancedNode<Transform> node)
        {
            return (previousCamPosition.y - camTransform.position.y) * getZDistanceFromCamera(camTransform, node) *
                   scaleY;
        }


        private void doParallax()
        {
            foreach (var balancedList in balancedLists)
            {

                BalancedNode<Transform> node = balancedList.getRootNode();
                try
                {
                    float parallaxX = getXParallax(node);
                    float parallaxY = getYParallax(node);

                    float targetPositionX = getNodePositionX(node);
                    float targetPositionY = getNodePositionY(node);

                    if (parallaxHorizontal)
                    {
                        targetPositionX += parallaxX;
                    }

                    if (parallaxVertical)
                    {
                        targetPositionY += parallaxY;
                    }

                    Vector3 targetPosition = new Vector3(targetPositionX, targetPositionY, getNodePositionZ(node));

                    node.payload.position = Vector3.SmoothDamp(node.payload.position, targetPosition, ref speed,
                        smoothing * Time.deltaTime);
                }
                catch
                {
                    logger.LogError("Problem doing Parallax - Make sure root transform is set", "AutoParallax");
                    isError = true;
                }
            }
        }


        private void doBalancing()
        {
            float cameraHorizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

            foreach (var balancedList in balancedLists)
            {


                BalancedNode<Transform> leftestNode = balancedList.getLeftestNode();
                BalancedNode<Transform> rightestNode = balancedList.getRightestNode();

                float edgeVisibleRightPosition =
                    getNodePositionX(rightestNode) + getNodeWidth(rightestNode) / 2 - cameraHorizontalExtent;
                float edgeVisibleLeftPosition =
                    getNodePositionX(leftestNode) - getNodeWidth(leftestNode) / 2 + cameraHorizontalExtent;


                if (camTransform.position.x > edgeVisibleRightPosition - offsetX)
                {
                    balancedList.popRight();
                }

                if (camTransform.position.x < edgeVisibleLeftPosition + offsetX)
                {
                    balancedList.popLeft();
                }
            }
        }
        
        
        
    }
}