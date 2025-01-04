using System;
using ICanPay.Core;

namespace ZSN.Utils.Core.Pay
{
    /// <summary>
    /// 支付类接口
    /// </summary>
    public interface IPay
    {
        void BillDownload(IAuxiliary auxiliary);

        string CreateOrder(IOrder order, GatewayTradeType tradeType,
            Action<object, PaymentSucceedEventArgs> succeed = null,
            Action<object, PaymentFailedEventArgs> failed = null);

        INotify Cancel(IAuxiliary auxiliary);

        INotify Close(IAuxiliary auxiliary);

        INotify Query(IAuxiliary auxiliary);

        INotify Refund(IAuxiliary auxiliary);

        INotify RefundQuery(IAuxiliary auxiliary);
    }
}
