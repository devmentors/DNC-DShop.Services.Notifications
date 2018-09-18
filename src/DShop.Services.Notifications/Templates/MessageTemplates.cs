namespace DShop.Services.Notifications.Templates
{
    public static class MessageTemplates
    {
        public static string OrderCreatedSubject => "Order {0} has been created.";
        public static string OrderCreatedBody => @"
Hi {0} {1},
Thank you for choosing DSop! Your order with id: {2} has been created. We will inform you about its status changes.

Best regards,
DShop Team";
        
    }
}
