using ClothingStore.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ClothingStore.Observer
{
    public class SmsNotificationService : IOrderObserver
    {
        private readonly string _accountSid = "YOUR_TWILIO_SID";
        private readonly string _authToken = "YOUR_TWILIO_AUTH_TOKEN";
        private readonly string _fromPhone = "YOUR_TWILIO_PHONE";

        public SmsNotificationService()
        {
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task NotifyAsync(Order order)
        {
            var message = await MessageResource.CreateAsync(
                body: $"Đơn hàng #{order.IdOrder} của bạn đã được xác nhận!",
                from: new Twilio.Types.PhoneNumber(_fromPhone),
                to: new Twilio.Types.PhoneNumber(order.Customer.Phone)
            );

            Console.WriteLine($"SMS sent: {message.Sid}");
        }
    }
}
