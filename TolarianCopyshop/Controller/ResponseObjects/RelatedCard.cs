using System;

using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class RelatedCard
    {
        public Guid Id { get; set; }
        public RelatedCardType Type { get; set; }
    }
}