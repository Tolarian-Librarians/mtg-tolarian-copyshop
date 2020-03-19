using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.DbRequestModels;

namespace Tolarian.Copyshop.Business.Utility
{
    public static class CardRequestResolver
    {
        public static Dictionary<GetCardCollectionRequest, int> ResolveCardRequestsFromImportString(List<string> importLines)
        {
            var result = new Dictionary<GetCardCollectionRequest, int>();
            foreach (string line in importLines.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                var resolved = ResolveCardName(line);
                result.Add(resolved.Key, resolved.Value);
            }

            return result;
        }

        private static KeyValuePair<GetCardCollectionRequest, int> ResolveCardName(string line)
        {
            GetCardCollectionRequest actualCardRequest = new GetCardCollectionRequest();
            string[] splitted = line.Split(' ');

            if (IsLastValueArenaCode(splitted))
                DeleteLastValueOf(ref splitted);

            if (IsLastValueMtgSetCode(splitted))
            {
                actualCardRequest.SetCode = LastItemOf(splitted).TrimStart('(').TrimEnd(')');
                DeleteLastValueOf(ref splitted);
            }

            int amount = GetAmountOfCardInDeck(ref splitted);
            actualCardRequest.Name = string.Join(" ", splitted);

            return new KeyValuePair<GetCardCollectionRequest, int>(actualCardRequest, amount);
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

        private static void DeleteLastValueOf(ref string[] splitted)
        {
            splitted = splitted.Take(splitted.Length - 1).ToArray();
        }

        private static bool IsLastValueArenaCode(string[] splitted)
        {
            return int.TryParse(LastItemOf(splitted), out _);
        }

        private static bool IsLastValueMtgSetCode(string[] splitted)
        {
            return LastItemOf(splitted).StartsWith("(") && LastItemOf(splitted).EndsWith(")");
        }

        private static string LastItemOf(string[] array)
        {
            return array[array.Length - 1];
        }

    }
}
