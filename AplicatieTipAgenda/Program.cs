using System;
using LibrarieModele;
using NivelAccesDate;
using System.Configuration;
using System.IO;

namespace AplicatieTipAgenda
{
    class Program
    {
        static void Main(string[] args)
        {
            int nrPersoane;
            PersoaneContact[] persoane;

            IStocareData adminPersoane = StocareFactory.GetAdministratorStocare();
            persoane = adminPersoane.GetPersoane(out nrPersoane);



            do
            {
                Console.WriteLine("\n~~~~~Aplicatie_Agenda~~~~~~\n" +
                                  "F - Afisare persoana Grup cu parametri\n" +
                                  "C - Citeste persoana de la tastatura\n" +
                                  "A - Afisare persoane\n" +
                                  "B - Compara doua persoane\n" +
                                  "Z - Cauta persoana Contact\n" +
                                  "Y - Afiseaza Grupuri persoane\n" +
                                  "R - Modifica persoana Contact\n" +
                                  "I - Info Administrator lista\n" +
                                  "X - EXIT!");
                string opt = Console.ReadLine();
                switch (opt.ToUpper())
                {
                    case "F":
                        // Initializarea unui obiect de tip persoana cu parametri 
                        PersoaneContact p0 = new PersoaneContact("Baciu", "Emanuel", "0743307248", "emy_baciu@yahoo.com", 1);
                        persoane[nrPersoane++] = p0;
                        Console.WriteLine(p0.ConversieLaSir());
                        Console.WriteLine(p0.DataNasterii = new DateTime(1999, 01, 03));

                        Console.ReadKey();

                        //Intantierea unui obiect de tip Persoana cu parametrii intr-un sir de caractere
                        PersoaneContact p1 = new PersoaneContact("Baciu", "Ionut Emanuel", "0756818340", "ionut@yahoo.com", 2);
                        persoane[nrPersoane++] = p1;
                        Console.WriteLine(p1.ConversieLaSir());
                        Console.WriteLine(p1.DataNasterii = new DateTime(1999, 07, 31));
                        break;

                    case "C":
                        //Instantierea unui obiect de tip Persoana cu parametrii introdusi de la tastatura
                        Console.WriteLine("\nIntroduceti Numele: ");
                        string num = Console.ReadLine();
                        Console.WriteLine("\nIntroduceti Prenumele: ");
                        string prenum = Console.ReadLine();
                        Console.WriteLine("\nIntroduceti Numarul de Telefon: ");
                        double nrTEl = Convert.ToDouble(Console.ReadLine());
                        while (true)
                        {
                            if (nrTEl.ToString().Length == 10)
                            {
                                Console.WriteLine("Ati introdus un numar valid!");
                                break;

                            }
                            else
                            {
                                Console.WriteLine("Ati introdus un numar gresit!\n" +
                                                  "Aveti doar " + nrTEl.ToString().Length + " Cifre.\n" +
                                                  "Reintroduceti cu atentie 10 cifre:07__________: ");
                                nrTEl = Convert.ToDouble(Console.ReadLine());
                            }

                        }
                        string TE = string.Empty;
                        TE = string.Join(" ", nrTEl);
                        Console.WriteLine("Introduceti Adresa de e-mail: ");
                        string em = Console.ReadLine();
                        Console.WriteLine("Introduceti Grupul din care face parte:\n" +
                                          "1 - Familie\n" +
                                          "2 - Prieteni\n" +
                                          "3 - Serviciu\n" +
                                          "4 - Necunoscut");
                        string gr = Console.ReadLine();
                        while (Convert.ToInt32(gr) < 1 || Convert.ToInt32(gr) > 4)
                        {
                            Console.WriteLine("Reintroduceti grupul din care face parte [1,4]:");
                            gr = Console.ReadLine();
                        }
                        int gru = Convert.ToInt32(gr);

                        adminPersoane.AddPersoana(persoane[nrPersoane++] = new PersoaneContact(num, prenum, TE, em, gru));
                        //PersoaneContact p2 = new PersoaneContact(num, prenum, nrTEl, em, gru);

                        Console.WriteLine("\nIntroduceti data nasterii in ordine: anul,luna,ziua:YYYY-XZ-HH");
                        DateTime dataN = Convert.ToDateTime(Console.ReadLine());

                        //Console.WriteLine(dataN.nastere());


                        break;

                    case "A":
                        AfisareAgenda(persoane, nrPersoane);
                       /* Console.WriteLine("Aveti un numar de " + nrPersoane + " in agenda.");
                        for (int i = 0; i < nrPersoane; i++)
                        {
                            Console.WriteLine("ID-ul " + (i + 1) + "\n" + persoane[i].ConversieLaSir());
                        }*/
                        break;

                    case "B":
                        Console.WriteLine("Introduceti id-ul persoanei pentru comparare:");
                        string ID = Console.ReadLine();
                        while (Convert.ToInt32(ID) < nrPersoane || Convert.ToInt32(ID) > (nrPersoane + 1)) //&& Convert.ToInt32(ID) == numarPersoane)
                        {
                            Console.WriteLine("Reintroduceti id- ul persoanei nu mai mare de " + nrPersoane);
                            ID = Console.ReadLine();
                        }
                        int id = Convert.ToInt32(ID);
                        bool ok = false;
                        for (int i = 0; i < nrPersoane; i++)
                        {
                            if (persoane[id - 1].Compara(persoane[i]))
                            {
                                Console.WriteLine("Persoana: " + persoane[i + 1].NumeleComplet + " are numele mai lung decat " + persoane[i].NumeleComplet);
                                ok = true;
                            }
                            if (ok == false)
                            {
                                Console.WriteLine(".");
                            }


                        }
                        break;

                    case "Z":
                        Console.WriteLine("Introdu numele persoanei cautate:");
                        string nump = Console.ReadLine();
                        Console.WriteLine("Introdu prenumele persoanei cautate:");
                        string pnump = Console.ReadLine();
                        for (int i = 0; i < nrPersoane; i++)
                        {
                            if (persoane[i].NumeleComplet == nump + " " + pnump)
                            {
                                Console.WriteLine("Presoana cautata:");
                                Console.WriteLine(persoane[i].ConversieLaSir());
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Nu s-a gasit pesoana introdusa de dumneavoastra!");
                            }
                        }

                        break;

                    case "Y":
                        bool k = false;
                        Console.WriteLine("Ce grup doriti sa afisati:\n" +
                                          "1 - Familie\n" +
                                          "2 - Prieteni\n" +
                                          "3 - Serviciu\n" +
                                          "4 - Necunoscut");
                        string gru1 = Console.ReadLine();
                        while (Convert.ToInt32(gru1) < 1 || Convert.ToInt32(gru1) > 4)
                        {
                            Console.WriteLine("Reintroduceti grupul cautat [1,4]:");
                            gru1 = Console.ReadLine();
                        }
                        int gru2 = Convert.ToInt32(gru1);

                        for (int i = 0; i < nrPersoane; i++)
                        {
                            if (persoane[i].GRUP == (Grup)gru2)
                            {
                                Console.WriteLine(persoane[i].ConversieLaSir());
                                k = true;
                            }

                        }
                        if (k == false)
                        {
                            Console.WriteLine("Grupul ales nu contine nici o persoana.");
                        }
                        break;

                    case "R":
                        bool altaPers = true;
                        Console.WriteLine("Introduceti ID- ul persoanei pe care doriti sa o modificati: ");
                        int idd = Convert.ToInt32(Console.ReadLine()) - 1;
                        while (altaPers)
                        {
                            Console.WriteLine("1 - Nume\n" +
                                              "2 - Prenume\n" +
                                              "3 - Numar Telefon\n" +
                                              "4 - Adresa e-mail\n" +
                                              "5 - Grupul");
                            int optiune = Convert.ToInt32(Console.ReadLine());

                            if (optiune == 1)
                            {
                                Console.WriteLine("Reintroduceti numele:");
                                persoane[idd].Nume = Console.ReadLine();
                            }
                            if (optiune == 2)
                            {
                                Console.WriteLine("Reintroduceti prenume:");
                                persoane[idd].Prenume = Console.ReadLine();
                            }
                            if (optiune == 3)
                            {
                                Console.WriteLine("Reintroduceti Numar de telefon:");
                                persoane[idd].NumarTelefon = Console.ReadLine();
                            }
                            if (optiune == 4)
                            {
                                Console.WriteLine("Reintroduceti adresa de e-mail:");
                                persoane[idd].AdresaEmail = Console.ReadLine();
                            }
                            if (optiune == 5)
                            {
                                Console.WriteLine("Reintroduceti grupul:\n" +
                                                  "1 - Familie\n" +
                                                  "2 - Prieteni\n" +
                                                  "3 - Serviciu\n" +
                                                  "4 - Necunoscut");
                                int GRUP = Convert.ToInt32(Console.ReadLine());
                                persoane[idd].GRUP = (Grup)GRUP;
                            }
                            Console.WriteLine("Doriti alte modificari? ~ Y/N ~");
                            string danu = Console.ReadLine().ToUpper();
                            if (danu == "Y")
                            {
                                altaPers = true;
                            }
                            else
                            {
                                altaPers = false;
                                File.WriteAllText(ConfigurationManager.AppSettings["NumeFisierPersoane"] + ".txt", string.Empty);
                                for (int i = 0; i < nrPersoane; i++)
                                {
                                    adminPersoane.AddPersoana(persoane[i]);
                                }
                            }
                        }

                        break;

                    case "I":
                        Console.WriteLine("~~~~~Baciu Emanuel-Ionut~~~~~");
                        break;

                    case "X":
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta:");
                        break;

                }
            } while (true);

        }
        public static void AfisareAgenda(PersoaneContact[] persoane, int nrPersoane)
        {
            Console.WriteLine("Datele de contact sunt:");
            for (int i = 0; i < nrPersoane; i++)
            {
                Console.WriteLine(persoane[i].ConversieLaSir());
            }
        }
    }   }

    
    
