using Static;

namespace Player
{
  public class OrderProduct
  {
    public int Index;
    public Consume Consume;
    public bool Complete;
    public Order Order;
    // =====
    public OrderProduct(int index, Consume consume, bool complete, Order order)
    {
      Index = index;
      Consume = consume;
      Complete = complete;
      Order = order;
    }
  }
}