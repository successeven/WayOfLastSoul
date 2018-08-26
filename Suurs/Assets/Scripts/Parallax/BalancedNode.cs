namespace AutoParallaxScrolling
{
    public class BalancedNode<T>
    {
        
        public BalancedNode<T> leftBuddy;
        public BalancedNode<T> rightBuddy;
        public  T payload;
        private int id;


        public BalancedNode(T payload, BalancedNode<T> leftBuddy, BalancedNode<T> rightBuddy, int id) {
            this.payload = payload;
            this.leftBuddy = leftBuddy;
            this.rightBuddy = rightBuddy;
            this.id = id;
        }

        public BalancedNode(T payload, int id){
            this.payload = payload;
            leftBuddy = null;
            rightBuddy = null;
            this.id = id;
        }
        
        public BalancedNode(T payload){
            this.payload = payload;
            leftBuddy = null;
            rightBuddy = null;
            id = 0;
        }

        public void setLeftBuddy(BalancedNode<T> leftBuddy) {
            this.leftBuddy = leftBuddy;
        }

        public void setRightBuddy(BalancedNode<T> rightBuddy) {
            this.rightBuddy = rightBuddy;
        }

        public BalancedNode<T> getLeftBuddy() {
                return leftBuddy;
        }

        public BalancedNode<T> getRightBuddy() {
                return rightBuddy;
        }

        public T getPayload() {
            return payload;
        }

        public int getId() {
            return this.id;
        }
        
        
        
        
    }
}