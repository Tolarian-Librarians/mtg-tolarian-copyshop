using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.DbRequestModels;

namespace Tolarian.Copyshop.Business.Utility
{
    public static class DeckImportHelper
    {
        public static List<GetCardCollectionRequest> ResolveCardRequestsFromImportString(List<string> importLines)
        {
            List<GetCardCollectionRequest> result = new List<GetCardCollectionRequest>();
            foreach (string entry in importLines.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                result.AddRange(ResolveCardName(entry));
            }

            return result;
        }

        private static List<GetCardCollectionRequest> ResolveCardName(string entry)
        {
            List<GetCardCollectionRequest> result = new List<GetCardCollectionRequest>();
            GetCardCollectionRequest actualCardRequest = new GetCardCollectionRequest();
            string[] splitted = entry.Split(' ');

            if (IsLastValueArenaCode(splitted))
                splitted = DeleteLastValue(splitted);

            if (IsLastValueMtgSetCode(splitted))
            {
                actualCardRequest.SetCode = splitted[GetLastIndexOfArray(splitted)].TrimStart('(').TrimEnd(')');
                splitted = DeleteLastValue(splitted);
            }

            int amount = GetAmountOfCardInDeck(ref splitted);
            actualCardRequest.Name = string.Join(" ", splitted);

            for (int i = 0; i < amount; i++)
                result.Add(actualCardRequest);

            return result;
        }

        private static int GetAmountOfCardInDeck(ref string[] splitted)
        {
            int amount;
            if (int.TryParse(splitted[0], out amount))
                splitted = splitted.Skip(1).ToArray();
            else
                amount = 1;
            return amount;
        }

        private static string[] DeleteLastValue(string[] splitted)
        {
            splitted = splitted.Take(GetLastIndexOfArray(splitted)).ToArray();
            return splitted;
        }

        private static bool IsLastValueArenaCode(string[] splitted)
        {
            return int.TryParse(splitted[GetLastIndexOfArray(splitted)], out _);
        }

        private static bool IsLastValueMtgSetCode(string[] splitted)
        {
            return splitted[GetLastIndexOfArray(splitted)].StartsWith("(") && splitted[GetLastIndexOfArray(splitted)].EndsWith(")");
        }

        private static int GetLastIndexOfArray(string[] array)
        {
            return array.Length - 1;
        }

    }
}
