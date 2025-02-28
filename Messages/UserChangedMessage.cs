using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Memory.Messages
{
    public class UserChangedMessage : ValueChangedMessage<User>
    {
        public UserChangedMessage(User value) : base(value) { }
    }
}

