using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Memory.Messages
{
    public class CompanyChangedMessage : ValueChangedMessage<Company>
    {
        public CompanyChangedMessage(Company value) : base(value) { }
    }
}

