using System;
using System.Collections.Generic;
using System.Linq;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfSetInfo;

namespace Tolarian.Copyshop.Business.Entities
{
    public class SetCodeTranslator : ISetCodeTranslator
    {
        private Dictionary<string, string> arenaSetCodesToScryfallCodesMap = new();
        private readonly ISetDataGateway _setGateway;

        public SetCodeTranslator(ISetDataGateway setGateway)
        {
            _setGateway = setGateway;
        }

        public string TranslateArenaCodeToScryfallCode(string setCode)
        {
            if (setCode == null)
                return null;

            if (arenaSetCodesToScryfallCodesMap.ContainsKey(setCode.ToLower()))
            {
                return arenaSetCodesToScryfallCodesMap[setCode] ?? setCode;
            }
            else
            {
                SfSet correspondingSfSet = _setGateway.GetAllSets().Data.FirstOrDefault(set => string.Equals(set.MagicArenaSetCode, setCode, StringComparison.InvariantCultureIgnoreCase));

                string scryfallSetCode = correspondingSfSet != null ? correspondingSfSet.ScryfallSetCode : null;
                arenaSetCodesToScryfallCodesMap[setCode] = scryfallSetCode;

                return scryfallSetCode ?? setCode;
            }
        }
    }
}