using System;
using System.Collections.Generic;

public abstract class Vozilo
{
    public string Tip { get; set; }
    public int BrojVrata { get; set; }
    public int BrojTočkova { get; set; }
    public int BrojSedišta { get; set; }
    public int CenaPranja { get; set; }
    public int CenaUsisavanja { get; set; }
    public int PotrebnoVreme { get; set; }
    public bool Oprano { get; private set; }

    public Vozilo(string tip, int vrata, int tockovi, int sedista, int cenaPranja, int cenaUsisavanja, int potrebnoVreme)
    {
        Tip = tip;
        BrojVrata = vrata;
        BrojTočkova = tockovi;
        BrojSedišta = sedista;
        CenaPranja = cenaPranja;
        CenaUsisavanja = cenaUsisavanja;
        PotrebnoVreme = potrebnoVreme;
        Oprano = false;
    }

    public void OznaciKaoOprano()
    {
        Oprano = true;
    }

    public abstract int ObracunajCenu();
}

public class Motocikl : Vozilo
{
    public Motocikl() : base("Motocikl", 0, 2, 1, 200, 100, 30) { }

    public override int ObracunajCenu()
    {
        return CenaPranja;
    }
}

public class MaliAutomobil : Vozilo
{
    public MaliAutomobil() : base("Mali automobil", 4, 4, 5, 300, 150, 45) { }

    public override int ObracunajCenu()
    {
        return CenaPranja;
    }
}

public class SUV : Vozilo
{
    public SUV() : base("SUV", 4, 4, 5, 400, 200, 60) { }

    public override int ObracunajCenu()
    {
        return CenaPranja;
    }
}

public class Kombi : Vozilo
{
    public Kombi() : base("Kombi", 6, 4, 7, 600, 300, 90) { }

    public override int ObracunajCenu()
    {
        return CenaPranja;
    }
}

public class Autobus : Vozilo
{
    public Autobus() : base("Autobus", 4, 22, 32, 1000, 500, 120) { }

    public override int ObracunajCenu()
    {
        return CenaPranja;
    }
}

public abstract class Tepisi
{
    public string TipTepiha { get; set; }
    public int CenaPranjaTepiha { get; set; }
    public int CenaUsisavanjaTepiha { get; set; }
    public bool OpranoTepih { get; private set; }

    public Tepisi(string tipTepiha, int cenaPranjaTepiha, int cenaUsisavanjaTepiha)
    {
        TipTepiha = tipTepiha;
        CenaPranjaTepiha = cenaPranjaTepiha;
        CenaUsisavanjaTepiha = cenaUsisavanjaTepiha;
        OpranoTepih = false;
    }

    public void OznaciKaoOpranoTepih()
    {
        OpranoTepih = true;
    }

    public abstract int ObracunajCenuPranja();
    public abstract int ObracunajCenuUsisavanja();

}

public class MaliTepih : Tepisi
{
    public MaliTepih() : base("Mali tepih", 300, 150) { }

    public override int ObracunajCenuPranja()
    {
        return CenaPranjaTepiha;
    }

    public override int ObracunajCenuUsisavanja()
    {
        return CenaUsisavanjaTepiha;
    }
}

public class SrednjiTepih : Tepisi
{
    public SrednjiTepih() : base("Srednji tepih", 500, 250) { }

    public override int ObracunajCenuPranja()
    {
        return CenaPranjaTepiha;
    }

    public override int ObracunajCenuUsisavanja()
    {
        return CenaUsisavanjaTepiha;
    }
}

public class VelikiTepih : Tepisi
{
    public VelikiTepih() : base("Veliki tepih", 1000, 500) { }

    public override int ObracunajCenuPranja()
    {
        return CenaPranjaTepiha;
    }

    public override int ObracunajCenuUsisavanja()
    {
        return CenaUsisavanjaTepiha;
    }
}

public class Servis
{
    public string Ime { get; set; }

    public virtual void DodajVoziloNaRed(Vozilo vozilo)
    {
        // Implementacija po potrebi u nasleđenim klasama
    }
}

public class PerionicaServis : Servis
{
    UsisivacServis usisivac;
    int SumaUkupnogVremena = 0;

    public PerionicaServis(string name, StreamWriter writer)
    {
        Ime = name;
        VozilaNaRedu = new Queue<Vozilo>();
        Writer = writer;
        usisivac = new UsisivacServis("Usisivac OOP", writer);
    }

    public StreamWriter Writer { get; private set; }
    public Queue<Vozilo> VozilaNaRedu { get; private set; }

    public override void DodajVoziloNaRed(Vozilo vozilo)
    {
        if (!vozilo.Oprano)
        {
            VozilaNaRedu.Enqueue(vozilo);
            Console.WriteLine($"{vozilo.Tip} dodato na red za pranje. Potrebno vreme: {vozilo.PotrebnoVreme} min");
            Writer.Flush();
        }
        else
        {
            Console.WriteLine($"{vozilo.Tip} već oprano. Dodato direktno na red za usisavanje.");
            usisivac.DodajVoziloNaRed(vozilo); // Dodajemo direktno na red za usisavanje
        }
    }

    public int OperiVozilo()
    {
        int cenaPranja = 0;
        if (VozilaNaRedu.Count > 0)
        {
            Vozilo trenutnoVozilo = VozilaNaRedu.Dequeue();
            trenutnoVozilo.OznaciKaoOprano();
            int cena = trenutnoVozilo.ObracunajCenu();
            cenaPranja = cena;
            SumaUkupnogVremena += trenutnoVozilo.PotrebnoVreme; // Dodajemo vreme pranja vozila u ukupno vreme
            Console.WriteLine($"{trenutnoVozilo.Tip} oprano. Cena pranja: {cena} rsd");
            Writer.WriteLine($"{trenutnoVozilo.Tip},pranje,{trenutnoVozilo.CenaPranja} rsd,{trenutnoVozilo.PotrebnoVreme} min");
            Writer.Flush();
        }
        else
        {
            Console.WriteLine("Nema vozila na redu za pranje.");
        }

        return cenaPranja;
    }

    public int KolikoJeVozilaUServisu()
    {
        return VozilaNaRedu.Count;
    }

    public int DohvatiSumuUkupnogVremena()
    {
        return SumaUkupnogVremena;
    }
}

public class UsisivacServis : Servis
{
    public UsisivacServis(string name, StreamWriter writer)
    {
        Ime = name;
        VozilaNaRedu = new Queue<Vozilo>();
        Writer = writer;
    }

    public StreamWriter Writer { get; private set; }
    public Queue<Vozilo> VozilaNaRedu { get; private set; }

    public override void DodajVoziloNaRed(Vozilo vozilo)
    {
        VozilaNaRedu.Enqueue(vozilo);
        Console.WriteLine($"{vozilo.Tip} dodato na red za usisavanje. Potrebno vreme: {vozilo.PotrebnoVreme} min");
    }

    public int UsisajVozilo()
    {
        int cenaUsisavanja = 0;
        if (VozilaNaRedu.Count > 0)
        {
            Vozilo trenutnoVozilo = VozilaNaRedu.Dequeue();
            int cena = trenutnoVozilo.CenaUsisavanja;
            cenaUsisavanja = cena;
            Console.WriteLine($"{trenutnoVozilo.Tip} usisano. Cena usisavanja: {cena} rsd");
            Writer.WriteLine($"{trenutnoVozilo.Tip},usisavanje,{trenutnoVozilo.CenaUsisavanja} rsd,{trenutnoVozilo.PotrebnoVreme} min");
            Writer.Flush();
        }
        else
        {
            Console.WriteLine("Nema vozila na redu za usisavanje.");
        }
        return cenaUsisavanja;
    }

    public int KolikoJeVozilaUServisu()
    {
        return VozilaNaRedu.Count;
    }
}

public class PerionicaTepihaServis : Servis
{
    public PerionicaTepihaServis(string name, StreamWriter writer)
    {
        Ime = name;
        TepisiNaRedu = new Queue<Tepisi>();
        Writer = writer;
    }

    public StreamWriter Writer { get; private set; }
    public Queue<Tepisi> TepisiNaRedu { get; private set; }

    public void DodajTepihNaRed(Tepisi tepih)
    {
        if (!tepih.OpranoTepih)
        {
            Console.WriteLine($"{tepih.TipTepiha} dodat na red za pranje.");
            TepisiNaRedu.Enqueue(tepih);
        }
        else
        {
            Console.WriteLine($"{tepih.TipTepiha} već opran. Ignorisano.");
        }
    }

    public int OperiTepih()
    {
        int cenaPranja = 0;
        if (TepisiNaRedu.Count > 0)
        {
            Tepisi trenutniTepih = TepisiNaRedu.Dequeue();
            trenutniTepih.OznaciKaoOpranoTepih();
            int cena = trenutniTepih.ObracunajCenuPranja();
            cenaPranja = cena;
            Console.WriteLine($"{trenutniTepih.TipTepiha} opran. Cena pranja: {cena} rsd");
            Writer.WriteLine(trenutniTepih.TipTepiha + ",pranje ," + trenutniTepih.CenaPranjaTepiha + " rsd");
            Writer.Flush();
        }
        else
        {
            Console.WriteLine("Nema tepiha na redu za pranje.");
        }

        return cenaPranja;
    }

    public int KolikoJeTepihaUServisu()
    {
        return TepisiNaRedu.Count;
    }
}

public class UsisavacTepihaServis : Servis
{
    public UsisavacTepihaServis(string name, StreamWriter writer)
    {
        Ime = name;
        TepisiNaRedu = new Queue<Tepisi>();
        Writer = writer;
    }

    public StreamWriter Writer { get; private set; }
    public Queue<Tepisi> TepisiNaRedu { get; private set; }

    public void DodajTepihNaRed(Tepisi tepih)
    {
        if (!tepih.OpranoTepih)
        {
            Console.WriteLine($"{tepih.TipTepiha} dodat na red za usisavanje.");
            TepisiNaRedu.Enqueue(tepih);
        }
        else
        {
            Console.WriteLine($"{tepih.TipTepiha} već usisan. Ignorisano.");
        }
    }

    public int UsisajTepih()
    {
        int cenaUsisavanja = 0;
        if (TepisiNaRedu.Count > 0)
        {
            Tepisi trenutniTepih = TepisiNaRedu.Dequeue();
            trenutniTepih.OznaciKaoOpranoTepih();
            int cena = trenutniTepih.ObracunajCenuUsisavanja();
            cenaUsisavanja = cena;
            Console.WriteLine($"{trenutniTepih.TipTepiha} usisan. Cena usisavanja: {cena} rsd");
            Writer.WriteLine(trenutniTepih.TipTepiha + ",usisavanje ," + trenutniTepih.CenaUsisavanjaTepiha + " rsd");
            Writer.Flush();
        }
        else
        {
            Console.WriteLine("Nema tepiha na redu za usisavanje.");
        }

        return cenaUsisavanja;
    }

    public int KolikoJeTepihaUServisu()
    {
        return TepisiNaRedu.Count;
    }
}

class Program
{
    static void Main()
    {
        string putanjaDoFajla = ".\\informacije.csv";

        // Kreiramo StreamWriter
        using (StreamWriter sw = new StreamWriter(putanjaDoFajla))
        {
            sw.WriteLine("TIP VOZILA/TEPIHA, OPERACIJA, CENA, POTREBNO VREME");
            sw.Flush();

            Servis perionica = new PerionicaServis("Perionica OOP", sw);
            UsisivacServis usisivac = new UsisivacServis("Usisivac OOP", sw);
            PerionicaTepihaServis perionicaTepiha = new PerionicaTepihaServis("Perionica Tepiha OOP", sw);
            UsisavacTepihaServis usisavacTepiha = new UsisavacTepihaServis("Usisavac Tepiha OOP", sw);

            // Kreiranje vozila
            Vozilo motocikl = new Motocikl();
            Vozilo maliAutomobil = new MaliAutomobil();
            Vozilo suv = new SUV();
            Vozilo kombi = new Kombi();
            Vozilo autobus = new Autobus();

            // Kreiranje tepiha
            Tepisi maliTepih = new MaliTepih();
            Tepisi srednjiTepih = new SrednjiTepih();
            Tepisi velikiTepih = new VelikiTepih();

            Console.WriteLine(perionica.Ime);
            int sumaPranja = 0;
            int sumaUsisavanja = 0;
            int sumaPranjaTepiha = 0;
            int sumaUsisavanjaTepiha = 0;
            int sumaSvega = 0;

            int brojVozilaPranje = NasumicnoOperisiVozila(perionica, "pranje", 5, 10);

            Console.WriteLine("\nOperacije na perionici:");
            while (((PerionicaServis)perionica).VozilaNaRedu.Count > 0)
            {
                sumaPranja += ((PerionicaServis)perionica).OperiVozilo();
            }

            Console.WriteLine();

            Console.WriteLine(usisivac.Ime);

            int brojVozilaUsisavanje = NasumicnoOperisiVozila(usisivac, "usisavanje", 4, brojVozilaPranje);

            Console.WriteLine("\nOperacije na usisivaču:");
            while (((UsisivacServis)usisivac).VozilaNaRedu.Count > 0)
            {
                sumaUsisavanja += ((UsisivacServis)usisivac).UsisajVozilo();
            }

            Console.WriteLine();
            Console.WriteLine("Perionica Tepiha Servis");
            int brojTepihaPranje = NasumicnoOperisiTepihe(perionicaTepiha, "pranje", 3, 6);

            Console.WriteLine("\nOperacije na perionici tepiha:");
            while (perionicaTepiha.TepisiNaRedu.Count > 0)
            {
                sumaPranjaTepiha += perionicaTepiha.OperiTepih();
            }

            Console.WriteLine();
            Console.WriteLine("Usisavac Tepiha Servis");

            int brojTepihaUsisavanje = NasumicnoOperisiTepihe(usisavacTepiha, "usisavanje", 2, 5);

            Console.WriteLine("\nOperacije na usisivaču tepiha:");
            while (usisavacTepiha.TepisiNaRedu.Count > 0)
            {
                sumaUsisavanjaTepiha += usisavacTepiha.UsisajTepih();
            }

            Console.WriteLine();

            sumaSvega = sumaPranja + sumaUsisavanja + sumaPranjaTepiha + sumaUsisavanjaTepiha;

            int SumaUkupnogVremena;
            SumaUkupnogVremena = ((PerionicaServis)perionica).DohvatiSumuUkupnogVremena();


            sw.WriteLine();
            sw.WriteLine("Suma pranja vozila" + "," + "," + sumaPranja + " rsd");
            sw.WriteLine("Suma usisavanja vozila" + "," + "," + sumaUsisavanja + " rsd");
            sw.WriteLine("Suma pranja tepiha" + "," + "," + sumaPranjaTepiha + " rsd");
            sw.WriteLine("Suma usisavanja tepiha" + "," + "," + sumaUsisavanjaTepiha + " rsd");
            sw.WriteLine("Ukupna suma" + "," + "," + sumaSvega + " rsd");
            sw.WriteLine("Suma ukupnog vremena za vozila" + "," + "," + "," + SumaUkupnogVremena + " min");

            sw.WriteLine();
            sw.WriteLine("Broj opranih vozila" + "," + "," + brojVozilaPranje + " vozila");
            sw.WriteLine("Broj usisanih vozila" + "," + "," + brojVozilaUsisavanje + " vozila");
            sw.WriteLine("Broj opranih tepiha" + "," + "," + brojTepihaPranje + " tepiha");
            sw.WriteLine("Broj usisanih tepiha" + "," + "," + brojTepihaUsisavanje + " tepiha");

            Console.ReadLine();
        }
    }

    static int NasumicnoOperisiVozila(Servis servis, string operacija, int lowerBound, int upperBound)
    {
        int brojVozila = new Random().Next(lowerBound, upperBound);

        for (int i = 0; i < brojVozila; i++)
        {
            int voziloIndex = new Random().Next(1, 6);
            DodajVoziloNaRed(servis, voziloIndex, operacija);
        }

        return brojVozila;
    }

    static void DodajVoziloNaRed(Servis servis, int voziloIndex, string operacija)
    {
        Vozilo vozilo = null;

        switch (voziloIndex)
        {
            case 1:
                vozilo = new Motocikl();
                break;
            case 2:
                vozilo = new MaliAutomobil();
                break;
            case 3:
                vozilo = new SUV();
                break;
            case 4:
                vozilo = new Kombi();
                break;
            case 5:
                vozilo = new Autobus();
                break;
        }

        if (operacija == "pranje" || operacija == "usisavanje")
        {
            servis.DodajVoziloNaRed(vozilo);
        }
    }

    static int NasumicnoOperisiTepihe(Servis servis, string operacija, int lowerBound, int upperBound)
    {
        int brojTepiha = new Random().Next(lowerBound, upperBound);

        for (int i = 0; i < brojTepiha; i++)
        {
            int tepihIndex = new Random().Next(1, 4);
            DodajTepihNaRed(servis, tepihIndex, operacija);
        }

        return brojTepiha;
    }

    static void DodajTepihNaRed(Servis servis, int tepihIndex, string operacija)
    {
        Tepisi tepih = null;

        switch (tepihIndex)
        {
            case 1:
                tepih = new MaliTepih();
                break;
            case 2:
                tepih = new SrednjiTepih();
                break;
            case 3:
                tepih = new VelikiTepih();
                break;
        }

        if (operacija == "pranje" || operacija == "usisavanje")
        {
            // Provera da li je servis instanca PerionicaTepihaServis pre dodavanja tepiha
            if (servis is PerionicaTepihaServis)
            {
                ((PerionicaTepihaServis)servis).DodajTepihNaRed(tepih);
            }
            // Provera da li je servis instanca UsisavacTepihServis pre dodavanja tepiha
            else if (servis is UsisavacTepihaServis)
            {
                ((UsisavacTepihaServis)servis).DodajTepihNaRed(tepih);
            }
            else
            {
                Console.WriteLine("Neispravan servis za pranje tepiha.");
            }
        }
    }
}