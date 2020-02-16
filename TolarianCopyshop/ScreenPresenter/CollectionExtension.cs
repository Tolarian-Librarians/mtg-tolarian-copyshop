using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter
{
    static class CollectionExtension
    {
        private static ObservableCollection<FullCard> ToFullCardCollection(this ObservableCollection<IFullCard> IFullCardCollection)
            => IFullCardCollection.ToList().ToFullCardCollection();

        private static ObservableCollection<FullCard> ToFullCardCollection(this List<IFullCard> IFullCardCollection)
        {
            ObservableCollection<FullCard> FullCardCollection = new ObservableCollection<FullCard>();
            IFullCardCollection.ForEach(card => FullCardCollection.Add(new FullCard(card)));
            return FullCardCollection;
        }

        private static List<IFullCard> ToIFullCardList(this ObservableCollection<FullCard> FullCardCollection)
            => FullCardCollection.ToList().ToIFullCardList();

        private static List<IFullCard> ToIFullCardList(this List<FullCard> FullCardCollection)
        {
            List<IFullCard> IFullCardCollection = new List<IFullCard>();
            FullCardCollection.ForEach(card => IFullCardCollection.Add(new FullCard(card)));
            return IFullCardCollection;
        }
    }
}
