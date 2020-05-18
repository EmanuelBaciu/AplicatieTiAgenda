using NivelAccesDate;
using System.Configuration;

namespace AplicatieTipAgenda
{
    public static class StocareFactory
    {
        private const string FORMAT_SALVARE = "FormatSalvare";
        private const string NUME_FISIER = "NumeFisier";
        public static IStocareData GetAdministratorStocare()
        {
            var formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE];
            var numeFisier = ConfigurationManager.AppSettings[NUME_FISIER];
            if (formatSalvare != null)
            {
                switch (formatSalvare)
                {
                    default:
                    case "bin":
                        return new AdministrarePersoaneContact_FisierBinar(numeFisier + "." + formatSalvare);
                    case "txt":
                        return new AdministrarePersoaneContact_FisierText(numeFisier + "." + formatSalvare);
                }
            }

            return null;
        }
    }
}
