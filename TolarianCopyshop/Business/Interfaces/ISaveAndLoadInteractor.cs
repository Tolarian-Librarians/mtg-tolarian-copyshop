using System.Collections.Generic;

using Tolarian.Copyshop.Business.Models.SaveAndLoad;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.Interfaces
{
    public interface ISaveAndLoadInteractor
    {
        List<SfCard> LoadDeck(string fileName);
        void SaveDeck(List<SaveCard> linesToSave, string fileName);
    }
}