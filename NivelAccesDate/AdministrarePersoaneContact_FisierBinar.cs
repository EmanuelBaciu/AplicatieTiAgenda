using LibrarieModele;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NivelAccesDate
{
    //clasa AdministrareStudenti_FisierBinar implementeaza interfata IStocareData
    public class AdministrarePersoaneContact_FisierBinar : IStocareData
    {
        private const int ID_PRIMUL_CONTACT = 1;
        private const int INCREMENT = 1;
        string NumeFisier { get; set; }
        public AdministrarePersoaneContact_FisierBinar(string numeFisiser)
        {
            this.NumeFisier = numeFisiser;
            Stream sBinFile = File.Open(NumeFisier, FileMode.OpenOrCreate);
            sBinFile.Close();

            //liniile de mai sus pot fi inlocuite cu linia de cod urmatoare deoarece
            //instructiunea 'using' va apela sBinFile.Close();
            // using (Stream sBinFile = File.Open(NumeFisier, FileMode.OpenOrCreate)) { }

        }

        public void AddPersoana(PersoaneContact s)
        {
            s.IdContact = GetID();

            try
            {
                BinaryFormatter b = new BinaryFormatter();

                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Append, FileAccess.Write))
                {
                    //serializare unui obiect
                    b.Serialize(sBinFile, s);
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {

                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }

            //throw new Exception("Optiunea AddStudent nu este implementata");
        }

        public List<PersoaneContact> GetPersoane()
        {
            List<PersoaneContact> contacte = new List<PersoaneContact>();
            try
            {
                BinaryFormatter b = new BinaryFormatter();

                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
                {

                    while (sBinFile.Position < sBinFile.Length)
                    {
                        //Observati conversia!!!
                        contacte.Add((PersoaneContact)b.Deserialize(sBinFile));
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
            return contacte;
            // throw new Exception("Optiunea GetStudenti nu este implementata");
        }
        public PersoaneContact GetContactByIndex(int index)
        {
            PersoaneContact s = null;
            List<PersoaneContact> contacte = new List<PersoaneContact>();
            try
            {
                BinaryFormatter b = new BinaryFormatter();
                int contor = 0;
                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
                {

                    while (sBinFile.Position < sBinFile.Length)
                    {
                        s = (PersoaneContact)b.Deserialize(sBinFile);
                        //Observati conversia!!!
                        contacte.Add((PersoaneContact)b.Deserialize(sBinFile));
                        if (contor == index)
                        {
                            break;
                        }
                        contor++;
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }

            return s;

            //throw new Exception("Optiunea GetStudentByIndex nu este implementata");
        }

        public PersoaneContact GetPersoane(string nume, string prenume)//, string numar, string mail)
        {
            //try
            //{
            //    // instructiunea 'using' va apela sr.Close()
            //    using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
            //    {
            //        string line;

            //        //citeste cate o linie si creaza un obiect de tip Student pe baza datelor din linia citita
            //        while (sBinFile.Position < sBinFile.Length)
            //        {
            //            PersoaneContact persoana = new PersoaneContact();
            //            if (persoana.Nume.Equals(nume) && persoana.Prenume.Equals(prenume))
            //                return persoana;
            //        }
            //    }
            //}
            //catch (IOException eIO)
            //{
            //    throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            //}
            //catch (Exception eGen)
            //{
            //    throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            //}
            //return null;
           throw new Exception("Optiunea GetStudent nu este implementata");
        }

        public bool UpdatePersoana(PersoaneContact s)
        {
            bool ok = false;
            List<PersoaneContact> contacte = GetPersoane();
            try
            {
                BinaryFormatter b = new BinaryFormatter();

                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Truncate, FileAccess.Write))
                {
                    foreach (var stud in contacte)
                    {
                        //serializare unui obiect
                        if (stud.IdContact == s.IdContact)
                        {
                            b.Serialize(sBinFile, s);
                        }
                        else
                            b.Serialize(sBinFile, stud);
                    }
                    ok = true;

                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
            return ok;

            throw new Exception("Optiunea UpdateStudent nu este implementata");
        }
        private int GetID()
        {
            int Idcon = ID_PRIMUL_CONTACT;
            try
            {
                //instructiunea 'using' va apela sBinFile.Close();
                using (Stream sBinFile = File.Open(NumeFisier, FileMode.Open))
                {
                    BinaryFormatter b = new BinaryFormatter();

                    //citeste cate o linie si creaza un obiect de tip Student pe baza datelor din linia citita
                    while (sBinFile.Position < sBinFile.Length)
                    {
                        //Observati conversia!!!
                        PersoaneContact s = (PersoaneContact)b.Deserialize(sBinFile);
                        Idcon = s.IdContact + INCREMENT;
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
            return Idcon;
        }
        public List<PersoaneContact> GetGrupFamilie()
        {
            List<PersoaneContact> contacte = GetPersoane();
            List<PersoaneContact> contacteFam = new List<PersoaneContact>();
            foreach (PersoaneContact s in contacte)
            {
                if (s.GRUP == Grup.Familie)
                    contacteFam.Add(s);
            }
            return contacteFam;
        }
    }
}
