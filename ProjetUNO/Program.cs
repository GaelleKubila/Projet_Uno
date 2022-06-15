using System;

namespace ProjetUNO
{
    internal class Program
    {

        public class carte
        {
            int numero;
            string couleur;

            public int Numero
            {
                get
                {
                    return numero;
                }
            }

            public string Couleur
            {
                get
                {
                    return couleur;
                }
            }


            //CONSTRUCTEURS

            public carte()
            {
                string[] types = { "Jaune", "Jaune", "Jaune", "Jaune", "Bleu", "Bleu", "Bleu", "Bleu", "Rouge", "Rouge", "Rouge", "Rouge", "Vert", "Vert", "Vert", "Vert", "Special" };
                int[] nombres1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                int[] nombres2 = { 11, 12 };
                int nombre_index;

                Random rand = new Random();

                int type_index = rand.Next(types.Length);

                if (types[type_index] != "Special")
                {
                    nombre_index = rand.Next(nombres1.Length);
                    numero = nombres1[nombre_index];
                }
                else
                {
                    nombre_index = rand.Next(nombres2.Length);
                    numero = nombres2[nombre_index];
                }


                couleur = types[type_index];

            }

            public carte(carte autre_carte)
            {
                numero = autre_carte.numero;
                couleur = autre_carte.couleur;

            }

            public carte(int n, string t)
            {
                numero = n;
                couleur = t;

            }



            //EGALITE

            public override bool Equals(object obj) => obj is carte c && Equals(c);

            public bool Equals(carte c) => ((couleur == c.couleur) && (numero == c.numero));

            public override int GetHashCode() => couleur.GetHashCode();

            public static bool operator ==(carte c1, carte c2)
            => ((c2.couleur == c1.couleur) && (c1.numero == c2.numero));

            public static bool operator !=(carte c1, carte c2)
            => ((c2.couleur != c1.couleur) && (c1.numero != c2.numero));



            public void affiche()
            {

                string[] special = { "+2", "+4", "Changement de couleur" };

                if (numero < 10)

                {

                    Console.WriteLine($"La carte est {couleur} et de numero {numero}");

                }

                else

                {
                    Console.WriteLine($"La carte est {couleur} et de type {special[numero - 10]}");
                }

            }

            public void regeneration()
            {
                carte remplacement = new carte();

                this.couleur = remplacement.couleur;
                this.numero = remplacement.numero;
            }

        }

        public class joueur

        {

            int ID;
            carte[] collectioncartes = new carte[100];
            int nbcartes;

            static int nbjoueurs = 0;

            public int Nbcartes
            {
                get 
                {
                    return nbcartes;
                }

                set
                {
                    if (value > 0)
                    {
                        nbcartes = value;
                    }
                    else
                    {
                        value = 0;
                    }
                }
            }

            public joueur()
            {
                ID = nbjoueurs + 1;

                nbcartes = 7;

                generation_debutdepartie(ref collectioncartes);

                Console.WriteLine($"Voici vos cartes, joueur {ID} :");

                for (int i = 0; i < nbcartes; i++)
                {
                     collectioncartes[i].affiche();
                }

                nbjoueurs++;
            }

            public void regardecollection()
            {
                Console.WriteLine($"Cartes du joueur {ID}");

                for (int i = 0; i < nbcartes; i++)
                {
                    collectioncartes[i].affiche();
                }
            }

            public void pioche()
            {

                bool choixpioche = pioche_ou_non();

                if (choixpioche)
                {

                    nbcartes++;
                    collectioncartes[nbcartes] = new carte();

                }
            }

            public carte choix_carte()
            {
                bool jeu_valide = false;
                int id_carte = 1;

                regardecollection();

                do
                {

                    Console.WriteLine("Selectionnez la carte à jouer");


                    try
                    {
                        id_carte = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    if (id_carte > nbcartes)
                    {
                        Console.WriteLine("La carte choisie n'existe pas!");
                    }
                    else
                    {
                        Console.WriteLine("Vous avez choisi une carte!");
                        jeu_valide = true;

                    }

                }
                while(!jeu_valide);

                return collectioncartes[id_carte - 1];
            }


        }

        // fonctions annexes

        public static void generation_debutdepartie(ref carte[] mescartes)
        {
            for (int i = 0; i < 7; i++)
            {
                carte temp = new carte();
                mescartes[i] = temp;
            }
        }

        public static void boucle_jeu(int nb_joueurs, joueur[] liste_joueurs)
        {
            int tour_id = 0;

            carte carte_dessus = new carte();

            switch (tour_id)
            {
                case 0: break;
                default: break;
            }



        }

        public static bool posecarte(ref carte dernierecarte, ref carte macarte)
        {

            if ((macarte.Couleur == "Special" && dernierecarte.Numero != 11) || (macarte.Numero == 11) || (macarte.Couleur != "Special" && macarte.Couleur == dernierecarte.Couleur) || (macarte.Numero == dernierecarte.Numero))
            {
                Console.WriteLine("Votre carte :");
                macarte.affiche();
                Console.WriteLine("La dernière carte en jeu :");
                dernierecarte.affiche();
                Console.WriteLine("Vous avez pu poser votre carte!");
                dernierecarte = macarte;
                return true;
            }

            else
            {
                Console.WriteLine("Votre carte :");
                macarte.affiche();
                Console.WriteLine("La dernière carte en jeu :");
                dernierecarte.affiche();
                Console.WriteLine("Votre carte ne correspond pas!");
                return false;

            }


        }

        public static bool pioche_ou_non()
        {
            string reponse;

            do
            {
                Console.WriteLine("Voulez vous piocher une nouvelle carte? (repondre Oui ou Non)");
                reponse = Console.ReadLine();
            }
            while (reponse != "Oui" && reponse != "Non");

            if (reponse == "Non")
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public static void Main(string[] args)
        {
            /*carte cartedeck = new carte();
            carte carteactuelle = new carte();

            bool res = posecarte(ref cartedeck, ref carteactuelle);
            bool rejoue;

            Console.WriteLine(res);

            if (res == false)
            {
                rejoue = pioche_ou_non();
                carteactuelle.regeneration();
                res = posecarte(ref cartedeck, ref carteactuelle);
            }*/


            joueur J1 = new joueur();
            joueur J2 = new joueur();
            joueur J3 = new joueur();
            joueur J4 = new joueur();

            joueur[] totaljoueurs = { J1, J2, J3, J4 };

            carte depart = new carte();

            carte carte_res = J1.choix_carte();

            carte_res.affiche();

            bool res = posecarte(ref depart, ref carte_res);

        }
    }
}
