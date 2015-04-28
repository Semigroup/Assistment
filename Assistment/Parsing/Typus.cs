using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Parsing
{
    public class Generika
    {
    }
    public class Typus
    {
        public string name { get; private set; }
        public bool istHaupt;
        public bool generisch;
        public Generika schema;
        public Typus generus;
        public SortedDictionary<string, List<Methode>> methoden = new SortedDictionary<string, List<Methode>>();
        public SortedDictionary<Typus, Konversator> konversionen = new SortedDictionary<Typus, Konversator>();
        public SortedDictionary<string, Feld> felder = new SortedDictionary<string, Feld>();

        public Typus(string name)
        {
            this.name = name;
            this.generisch = false;
            this.istHaupt = false;
        }
        public Typus(string name, bool istHaupt)
        {
            this.name = name;
            this.generisch = false;
            this.istHaupt = istHaupt;
        }

        public void addFeld(string bezeichner, Typus typ, bool beschreibbar)
        {
            felder.Add(bezeichner, new Feld(this, bezeichner, typ, beschreibbar));
        }

        public bool hatFeld(string bezeichner, out Feld feld)
        {
            return felder.TryGetValue(bezeichner, out feld);
        }
        public bool getMethode(Signatur signatur, out Methode methode)
        {
            List<Methode> methoden;
            if (this.methoden.TryGetValue(signatur.bezeichner, out methoden))
                foreach (Methode item in methoden)
                    if (signatur.konvertierbarZu(item.signatur))
                    {
                        methode = item;
                        return true;
                    }

            methode = null;
            return false;
        }
        public bool hatMethode(string bezeichner)
        {
            return methoden.ContainsKey(bezeichner);
        }
        /// <summary>
        /// gibt an, ob dieser Typ spezieller ist als allgemeinerTyp
        /// </summary>
        /// <param name="allgemeinerTyp"></param>
        /// <returns></returns>
        public bool ist(Typus allgemeinerTyp)
        {
            if (this == allgemeinerTyp)
                return true;
            return konversionen.ContainsKey(allgemeinerTyp);
        }


        public static Typus Any = new Typus("Any");
        public static Typus Ganzzahl = new Typus("Int");
        public static Typus Fliesskommazahl = new Typus("Float");
        public static Typus Basis = new Typus("Base", true);
    }
    public struct Signatur
    {
        public string bezeichner;
        public Typus[] eingabeTypen;
        public int stelligkeit;

        /// <summary>
        /// gibt an, ob diese Signatur spezieller ist als die gegebene
        /// <para>ignoriert die bezeichner</para>
        /// </summary>
        /// <param name="signatur"></param>
        /// <returns></returns>
        public bool konvertierbarZu(Signatur signatur)
        {
            if (stelligkeit == signatur.stelligkeit)
            {
                for (int i = 0; i < stelligkeit; i++)
                    if (!eingabeTypen[i].ist(signatur.eingabeTypen[i]))
                        return false;
                return true;
            }
            else return false;
        }
    }
    public class Methode
    {
        public Signatur signatur;
        public Typus aufruferTyp;
        public Typus ruckgabeTyp;
    }
    public class Operator : Methode
    {
        TokenType token;
    }
    public class Vorzeichen : Methode
    {
        TokenType token;
    }
    public class Konversator : Methode
    {
    }
    public class Feld
    {
        public Typus aufruferTyp;
        public string bezeichner;
        public Typus feldTyp;
        public bool beschreibbar;

        public Feld(Typus aufruferTyp, string bezeichner, Typus feldTyp, bool beschreibbar)
        {

        }

        public bool hatFeld(string bezeichner)
        {
            return feldTyp.felder.ContainsKey(bezeichner);
        }
        public Feld getFeld(string bezeichner)
        {
            Feld f;
            if (feldTyp.hatFeld(bezeichner, out f))
                return f;
            else
                throw new NotImplementedException();
        }
        public bool hatMethode(string bezeichner)
        {
            return feldTyp.methoden.ContainsKey(bezeichner);
        }
        public Methode getMethode(Signatur signatur)
        {
            Methode m;
            if (feldTyp.getMethode(signatur, out m))
                return m;
            else
                throw new NotImplementedException();
        }
    }
}
