using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.EntitiesModels;
using Tolarian.Copyshop.Business.Interfaces;

namespace Tolarian.Copyshop.Business.Entities
{
    public class ImportStringParser : IImportStringParser
    {
        public List<KeyValuePair<PreImportCard, int>> ResolvePreImportCardsFromImportString(List<string> importLines)
        {
            var result = new List<KeyValuePair<PreImportCard, int>>();
            foreach (string line in importLines.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                var resolved = ResolveCardName(line);
                result.Add(new KeyValuePair<PreImportCard, int>(resolved.Key, resolved.Value));
            }

            return result;
        }

        private KeyValuePair<PreImportCard, int> ResolveCardName(string line)
        {
            PreImportCard actualCardRequest = new PreImportCard();
            string[] splitted = line.Split(' ');

            if (IsLastValueArenaCode(splitted))
                DeleteLastValueOf(ref splitted);

            if (IsLastValueMtgSetCode(splitted))
            {
                actualCardRequest.SetCode = LastItemOf(splitted).TrimStart('(').TrimEnd(')');
                DeleteLastValueOf(ref splitted);
            }

            int amount = GetAmountOfCardInDeck(ref splitted);
            actualCardRequest.CardName = string.Join(" ", splitted);

            return new KeyValuePair<PreImportCard, int>(actualCardRequest, amount);
        }

        private int GetAmountOfCardInDeck(ref string[] splitted)
        {
            int amount;
            if (int.TryParse(splitted[0], out amount))
                splitted = splitted.Skip(1).ToArray();
            else
                amount = 1;
            return amount;
        }

        private void DeleteLastValueOf(ref string[] splitted)
        {
            splitted = splitted.Take(splitted.Length - 1).ToArray();
        }

        private bool IsLastValueArenaCode(string[] splitted)
        {
            return int.TryParse(LastItemOf(splitted), out _);
        }

        private bool IsLastValueMtgSetCode(string[] splitted)
        {
            return LastItemOf(splitted).StartsWith("(") && LastItemOf(splitted).EndsWith(")");
        }

        private string LastItemOf(string[] array)
        {
            return array[array.Length - 1];
        }

    }
}
