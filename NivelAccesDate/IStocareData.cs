using LibrarieModele;
using System.Collections;
using System.Collections.Generic;

namespace NivelAccesDate
{
    //definitia interfetei
    public interface IStocareData
    {
        void AddPersoana(PersoaneContact s);
        List<PersoaneContact> GetPersoane();
        PersoaneContact GetContactByIndex(int index);

        PersoaneContact GetPersoane(string nume, string prenume);//, string numar, string mail);

        bool UpdatePersoana(PersoaneContact s);

        List<PersoaneContact> GetGrupFamilie();
    }
}
