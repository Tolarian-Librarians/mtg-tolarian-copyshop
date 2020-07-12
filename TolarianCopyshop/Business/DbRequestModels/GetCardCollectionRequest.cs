using System;

namespace Tolarian.Copyshop.Business.DbRequestModels
{
    public class GetCardCollectionRequest
    {
        public string Name { get; set; }
        public string SetCode { get; set; }
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, SetCode: {SetCode}";
        }
    }
}
