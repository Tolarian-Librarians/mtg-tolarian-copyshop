using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Fontend.WPF.Model;

namespace Tolarian.Copyshop.Fontend.WPF.Helper
{
    internal static class CollectionExtension
    {
        private static ObservableCollection<FullCardModel> ToFullCardCollection(this ObservableCollection<IFullCard> IFullCardCollection)
            => IFullCardCollection.ToList().ToFullCardCollection();

        private static ObservableCollection<FullCardModel> ToFullCardCollection(this List<IFullCard> IFullCardCollection)
        {
            ObservableCollection<FullCardModel> FullCardCollection = new ObservableCollection<FullCardModel>();
            IFullCardCollection.ForEach(card => FullCardCollection.Add(FullCardModel.Create(card)));
            return FullCardCollection;
        }

        private static List<IFullCard> ToIFullCardList(this ObservableCollection<FullCardModel> FullCardCollection)
            => FullCardCollection.ToList().ToIFullCardList();

        private static List<IFullCard> ToIFullCardList(this List<FullCardModel> FullCardCollection)
        {
            List<IFullCard> IFullCardCollection = new List<IFullCard>();
            FullCardCollection.ForEach(card => IFullCardCollection.Add(FullCardModel.Create(card)));
            return IFullCardCollection;
        }
    }
}
