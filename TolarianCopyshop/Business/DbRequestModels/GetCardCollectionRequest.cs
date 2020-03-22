namespace Tolarian.Copyshop.Business.DbRequestModels
{
    public class GetCardCollectionRequest
    {
        public string Name { get; set; }
        public string SetCode { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, SetCode: {SetCode}";
        }
    }
}
