namespace AutoParallaxScrolling
{
    public class BalancedList<T>
    {
        private BalancedNode<T> rootNode;


        public BalancedList(BalancedNode<T> rootNode)
        {
            this.rootNode = rootNode;
        }

        public BalancedNode<T> getRootNode()
        {
            return rootNode;
        }

        private void setRootNode(BalancedNode<T> n)
        {
            this.rootNode = n;
        }

        public static BalancedList<T> createNewList(BalancedNode<T> node)
        {
            return new BalancedList<T>(node);
        }

        public void popLeft()
        {
            BalancedNode<T> rightestNode = getRightestNode();
            rightestNode.getLeftBuddy().setRightBuddy(null);

            detachNode(rightestNode);
            addLeftNode(rightestNode);

            setRootNode(rootNode.getLeftBuddy());
        }

        public void popRight()
        {
            BalancedNode<T> leftestNode = getLeftestNode();
            leftestNode.getRightBuddy().setLeftBuddy(null);

            detachNode(leftestNode);
            addRightNode(leftestNode);

            setRootNode(rootNode.getRightBuddy());
        }

        public int size()
        {
            var size = 0;
            BalancedNode<T> node = getLeftestNode();

            while (node != null)
            {
                size += 1;
                node = node.getRightBuddy();
            }

            return size;
        }

        public void add(BalancedNode<T> leftNode, BalancedNode<T> rightNode)
        {
            addLeftNode(leftNode);
            addRightNode(rightNode);
        }

        private static BalancedNode<T> traverseLeft(BalancedNode<T> n)
        {
            if (n.getLeftBuddy() != null)
            {
                return traverseLeft(n.getLeftBuddy());
            }

            return n;
        }

        private static BalancedNode<T> traverseRight(BalancedNode<T> n)
        {
            if (n.getRightBuddy() != null)
            {
                return traverseRight(n.getRightBuddy());
            }

            return n;
        }

        public BalancedNode<T> getLeftestNode()
        {
            return traverseLeft(rootNode);
        }

        public BalancedNode<T> getRightestNode()
        {
            return traverseRight(rootNode);
        }


        private void addLeftNode(BalancedNode<T> n)
        {
            BalancedNode<T> newNode = n;
            BalancedNode<T> currentLeft = getLeftestNode();
            currentLeft.setLeftBuddy(n);
            newNode.setRightBuddy(currentLeft);
        }

        private void addRightNode(BalancedNode<T> n)
        {
            BalancedNode<T> newNode = n;
            BalancedNode<T> currentRight = getRightestNode();

            currentRight.setRightBuddy(newNode);
            newNode.setLeftBuddy(currentRight);
        }

        private static void detachNode(BalancedNode<T> n)
        {
            n.leftBuddy = null;
            n.rightBuddy = null;
        }
    }
}