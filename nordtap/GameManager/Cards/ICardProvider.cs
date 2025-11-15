using Nordtap.GameComponents;

namespace Nordtap.GameManager.Cards;

public interface ICardProvider
{
    Card GetCardFromName(string cardName);
    Card GetCardFromId(string id);
    Card GetCardFromUrl(string url);
}