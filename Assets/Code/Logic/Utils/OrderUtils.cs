using Player;
using Static;
using System;
using System.Collections.Generic;
using System.Linq;

public static class OrderUtils
{
  private static PlayerData _player => Global.Instance.PlayerData;

  public enum EOrderCompleteFailreason
  {
    IsAlreadyExpire,
    ProductNotComplete
  }

  public class OrderSlot
  {
    public int Id;
    public bool IsBussy;
  }

  public static List<OrderSlot> OrderSlots = new List<OrderSlot>()
  {
    new OrderSlot() { Id = 0, IsBussy = false},
    new OrderSlot() { Id = 1, IsBussy = false},
    new OrderSlot() { Id = 2, IsBussy = false}
  };

  // ===================================================================================================
  /// <summary>
  /// Создание заказов
  /// </summary>
  public static void CreateOrders()
  {
    var orderData = Global.Instance.StaticData.OrderDatas.FirstOrDefault(o => (_player.Level >= o.MinPlayerLevel) && (_player.Level < o.MaxPlayerLevel));

    if ((_player == null) || (orderData == null))
    {
      DebugX.LogForUtils($"OrderUtils : CreateOrders : (_player == null) || (orderData == null) : FALSE", error: true);
      return;
    }

    PlayerUtils.Orders = new List<Order>();

    for (var i = 0; i < orderData.OrdersCount; i++)
    {
      var order = Create(orderData, i + 1);

      if (order == null)
        continue;

      PlayerUtils.Orders.Add(order);
    }

    TryAddNewActiveOrder();
  }

  // ===================================================================================================
  /// <summary>
  /// Создание заказа
  /// </summary>
  /// <param name="orderData"></param>
  /// <returns></returns>
  private static Order Create(OrderData orderData, int index)
  {
    var weight = UnityEngine.Random.Range(orderData.MinOrderWeight, orderData.MaxOrderWeight + 1);

    var productDatas = Global.Instance.StaticData.ProductDatas.Where(p => orderData.Products.Contains(p.Id)).ToList();

    foreach (var p in productDatas)
    {
      DebugX.LogForUtils($"OrderUtils : Create : {p.Id}");
    }

    List<OrderProduct> consums = new List<OrderProduct>();
    List<ProductData> prods = new List<ProductData>();

    Order order = new Order();
    DebugX.LogForUtils($"OrderUtils : Create : ====================");

    int productIndex = 0;
    while (weight > 0)
    {
      if (consums.Count() >= 3)
        break;

      var product = productDatas.GetRandomProduct(weight);

      DebugX.LogForUtils($"OrderUtils : Create : {product.Id}");

      if (product == null)
        break;

      consums.Add(new OrderProduct(productIndex, new Consume(product.Id, 1), false, order));
      prods.Add(product);
      weight -= product.Weight;
      productIndex++;
    }

    if (!consums.Any())
    {
      DebugX.LogForUtils($"OrderUtils : Create : Order Products IS EMPTY : FALSE", error: true);
      return null;
    }

    order.Products = consums;

    var golds = prods.Sum(p => ProjectUtils.GetItemById(p.Id).Prices.First(c => c.Type.Equals("gold")).Amount);

    order.Rewards.Add(new Consume("gold", golds));
    order.Rewards.Add(new Consume("exp", 1));
    order.Expire = new Timer(golds * 100);
    var owners = new List<string> { "solo", "duo", "three" };
    order.Owner = owners[UnityEngine.Random.Range(0, owners.Count())];
    order.Index = index;
    order.SetStateInWaiting();

    return order;
  }

  // ===================================================================================================
  /// <summary>
  /// Проверка, ест ли расходник в заказе
  /// </summary>
  /// <param name="order">заказ</param>
  /// <param name="consume">расходник</param>
  /// <returns>bool</returns>
  public static bool ExistConsumeInOrder(Order order, Consume consume)
  {
    if ((consume == null) || (order == null) || (order.Products == null))
      return false;

    return order.Products.Any(p => p.Consume.Type.Equals(consume.Type));
  }

  // ===================================================================================================
  /// <summary>
  /// Завершение продукта в заказе
  /// </summary>
  /// <param name="orderProduct">продукт</param>
  /// <param name="callbackEnd"></param>
  /// <param name="callbackFail"></param>
  public static void Complete(this OrderProduct orderProduct, Action callbackEnd = null, Action callbackFail = null)
  {
    if ((orderProduct == null))
    {
      DebugX.LogForUtils($"OrderUtils : OrderProduct : Complete : (orderProduct == null): FALSE", error: true);
      callbackFail?.Invoke();
      return;
    }

    if (orderProduct.Complete)
    {
      DebugX.LogForUtils($"OrderUtils : OrderProduct : Complete : orderProduct.Complete : FALSE", error: true);
      callbackFail?.Invoke();
      return;
    }

    ProjectUtils.RemoveConsumeFromPlayerSlot(orderProduct.Consume,
    () =>
    {
      orderProduct.Complete = true;
      EventManager.Instance?.TriggerEvent(new E_OrderProductComplete(orderProduct));
      
      DebugX.LogForUtils($"OrderUtils : OrderProduct : Complete");
      callbackEnd?.Invoke();
    },
    callbackFail);
  }

  // ===================================================================================================
  /// <summary>
  /// Завершение заказа
  /// </summary>
  /// <param name="order"></param>
  /// <param name="_callbackEnd"></param>
  /// <param name="_callbackFail"></param>
  public static void Complete(this Order order, Action callbackEnd = null, Action<EOrderCompleteFailreason> callbackFail = null)
  {
    if (order.Expire.IsExpired())
    {
      DebugX.LogForUtils($"OrderUtils : Complete : order.Expire.Expire() : FALSE", error: true);
      SkipExpireOrders();
      callbackFail?.Invoke(EOrderCompleteFailreason.IsAlreadyExpire);
      return;
    }

    bool isAnyProductNotComplete = order.Products.Any(p => !p.Complete);

    if (isAnyProductNotComplete)
    {
      DebugX.LogForUtils($"OrderUtils : Complete : !complete : FALSE", error: true);
      callbackFail?.Invoke(EOrderCompleteFailreason.ProductNotComplete);
      return;
    }

    order.SetStateComplete();
    DebugX.LogForUtils($"OrderUtils : Order : Complete");
    callbackEnd?.Invoke();
  }

  // ===================================================================================================
  /// <summary>
  /// Попытка добавить ноый активный заказ
  /// </summary>
  /// <returns>bool</returns>
  public static void TryAddNewActiveOrder(Action callBackEnd = null, Action callBackFail = null)
  {
    var activateOrderCount = 0;

    while (PlayerUtils.Orders.Count(o => o.IsInProgress) < 3)
    {
      var order = PlayerUtils.Orders.Where(o => o.IsInWaiting)?.OrderBy(o => o.Index)?.FirstOrDefault();

      if (order == null)
        break;

      var slot = OrderSlots.Where(os => !os.IsBussy)?.FirstOrDefault();

      if (slot == null)
        break;

      slot.IsBussy = true;
      order.OrderSlot = slot;
      order.Expire.Start();
      order.SetStateInProgress();
      activateOrderCount++;
    }

    if (activateOrderCount > 0)
    {
      callBackEnd?.Invoke();
    }
    else
    {
      callBackFail?.Invoke();
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Удаление просроченных закзов из списка активных заказов
  /// </summary>
  /// <param name="orders"></param>
  public static void SkipExpireOrders(Action callBackEnd = null, Action callBackFail = null)
  {
    var expireOrders = PlayerUtils.Orders.Where(o => o.Expire.IsExpired() && o.IsInProgress)?.ToList();

    expireOrders.ForEach(o =>
    {
      if (o.OrderSlot != null)
      {
        o.OrderSlot.IsBussy = false;
        o.OrderSlot = null;
      }
      o.SetStateSkip();
    });

    if (expireOrders.Count() > 0)
    {
      callBackEnd?.Invoke();
    }
    else
    {
      callBackFail?.Invoke();
    }
  }

  #region GetOrderListByState
  // ===================================================================================================
  /// <summary>
  /// Получить список активных заказов
  /// </summary>
  public static List<Order> GetOrdersInProgress()
  {
    return PlayerUtils.Orders.Where(o => o.IsInProgress)?.OrderBy(o => o.Index)?.ToList();
  }

  // ===================================================================================================
  /// <summary>
  /// Получить список ожидающих заказов
  /// </summary>
  public static List<Order> GetOrdersInWaiting()
  {
    return PlayerUtils.Orders.Where(o => o.IsInWaiting)?.OrderBy(o => o.Index)?.ToList();
  }

  // ===================================================================================================
  /// <summary>
  /// Получить список выполненных заказов
  /// </summary>
  public static List<Order> GetOrdersComplete()
  {
    return PlayerUtils.Orders.Where(o => o.IsComplete)?.OrderBy(o => o.Index)?.ToList();
  }

  // ===================================================================================================
  /// <summary>
  /// Получить список пропущенных заказов
  /// </summary>
  public static List<Order> GetOrdersSkip()
  {
    return PlayerUtils.Orders.Where(o => o.IsSkip)?.OrderBy(o => o.Index)?.ToList();
  }
  #endregion GetOrderListByState

  // ===================================================================================================
  /// <summary>
  /// Получить список активных заказов
  /// </summary>
  public static bool HasOrdersInProgress()
  {
    var orders = GetOrdersInProgress();
    return orders?.Any() ?? false;
  }

  // ===================================================================================================
  /// <summary>
  /// Получить список все награды завершенных заказов
  /// </summary>
  public static List<Consume> GetOrdersCompleteRewards()
  {
    var orders = GetOrdersComplete();

    if (orders == null)
      return new List<Consume>();

    List<Consume> result = new List<Consume>();

    foreach (var o in orders)
    {
      result.AddRange(o.Rewards);
    }

    return result;
  }

  // ===================================================================================================
  /// <summary>
  /// Получить количество выполненных заказов игорка
  /// </summary>
  public static int GetOrdersCompleteCount()
  {
    var orders = GetOrdersComplete();

    if (!orders?.Any() ?? true)
      return 0;

    return orders.Count();
  }

  // ===================================================================================================
  /// <summary>
  /// Получить количество пропущенных заказов игорка
  /// </summary>
  public static int GetOrdersSkipCount()
  {
    var orders = GetOrdersSkip();

    if (!orders?.Any() ?? true)
      return 0;

    return orders.Count();
  }

  // ===================================================================================================
  /// <summary>
  /// Получить количество заказов игрока
  /// </summary>
  public static int GetAllOrdersCount()
  {
    if (PlayerUtils.Orders == null)
      return 0;

    return PlayerUtils.Orders.Count();
  }

  // ===================================================================================================
  /// <summary>
  /// Получить суммарное значение всех таймеров заказов
  /// </summary>
  public static float GetAllOrdersDuration()
  {
    if (PlayerUtils.Orders == null)
      return 0;

    return PlayerUtils.Orders.Sum(o => o.Expire.Duration);
  }

  // ===================================================================================================
  /// <summary>
  /// Получить суммарное значение голды в награде всех заказов
  /// </summary>
  public static float GetAllOrdersRewardGold()
  {
    if (PlayerUtils.Orders == null)
      return 0;

    return PlayerUtils.Orders.Sum(o => o.Rewards.First(r => r.Type.Equals("gold")).Amount);
  }

  // ===================================================================================================
  /// <summary>
  /// Получить количество золота с выполненных заказов
  /// </summary>
  public static int GetOrdersCompleteRewardGold()
  {
    var orders = GetOrdersComplete();

    if (!orders?.Any() ?? true)
      return 0;

    var golds = orders.Sum(o => o.Rewards.First(r => r.Type.Equals("gold")).Amount);
    return golds;
  }

  // ===================================================================================================
  /// <summary>
  /// Получить количество золота с пропущенных заказов
  /// </summary>
  public static int GetOrdersSkipRewardGold()
  {
    var orders = GetOrdersSkip();

    if (!orders?.Any() ?? true)
      return 0;

    var golds = orders.Sum(o => o.Rewards.First(r => r.Type.Equals("gold")).Amount);
    return golds;
  }

}