using System.Collections.Generic;
using System.Linq;

namespace Tolarian.Copyshop.Business.Utility
{
    public static class DeckImportHelper
    {
        public static List<string> ResolveCardNamesFromList(List<string> cardNames)
        {
            List<string> result = new List<string>();
            foreach (string entry in cardNames.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                result.AddRange(ResolveCardName(entry));
            }

            return result;
        }

        private static List<string> ResolveCardName(string entry)
        {
            List<string> result = new List<string>();
            string[] splitted = entry.Split(' ');

            if (IsLastValueArenaCode(splitted))
                splitted = DeleteLastValue(splitted);

            if (IsLastValueMtgSetCode(splitted))
                splitted = DeleteLastValue(splitted);

            int amount = GetAmountOfCardInDeck(ref splitted);
            string actualCardName = string.Join(" ", splitted);

            for (int i = 0; i < amount; i++)
                result.Add(actualCardName);

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
