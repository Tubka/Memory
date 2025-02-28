using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Memory.Messages
{
    public class DateChangedMessage : ValueChangedMessage<DateTime>
    {
        public DateChangedMessage(DateTime value) : base(value) { }
    }
}

