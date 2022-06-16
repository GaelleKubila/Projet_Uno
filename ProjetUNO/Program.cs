using System;

namespace ProjetUNO
{
    internal class Program
    {

        public static Random rand = new Random();

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

                    Console.WriteLine($"Carte {couleur} numero {numero}");

                }

                else

                {
                    Console.WriteLine($"Carte {couleur} type {special[numero - 10]}");
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
                    Console.Write($"N°{i + 1} - ");
                    collectioncartes[i].affiche();
                }
            }

            public bool pioche()
            {

                bool choixpioche = pioche_ou_non();

                if (choixpioche)
                {

                    nbcartes++;
                    collectioncartes[nbcartes] = new carte();

                    Console.WriteLine($"Vous avez désormais {nbcartes} cartes");
                    collectioncartes[nbcartes].affiche();

                }

                return choixpioche;
            }

            public void plus_deuxouquatre(int nb)
            {
                Console.WriteLine($"Joueur {ID} : vous venez de prendre un +{nb}!");
                Console.WriteLine("Voici vos cartes supplementaires");

                int i;

                for (i=0; i<nb; i++)
                {
                    nbcartes++;
                    collectioncartes[nbcartes-1] = new carte();
                    collectioncartes[nbcartes-1].affiche();

                }

                Console.WriteLine($"Vous avez désormais {nbcartes} cartes");

            }

            public void retire(carte a_retirer)
            {
                int i = 0;

                while (i < nbcartes && collectioncartes[i]!= a_retirer)
                {
                    if (collectioncartes[i] != a_retirer)
                    {
                        i++;
                    }

                }

                while(i < nbcartes-1)
                {
                    collectioncartes[i] = collectioncartes[i + 1];
                    i++;
                }

                nbcartes--;

            }

            public carte dernierecartedudeck()
            {
                return collectioncartes[nbcartes];
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
                        Console.Write("Vous avez choisi une carte : ");
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

            Console.Write("La première carte est la suivante : ");
            carte_dessus.affiche();

            while(true)
            {

                bool premiere_pioche = false;
                bool seconde_pioche = false;

                Console.Write("Derniere carte posee : ");
                carte_dessus.affiche();

                carte carte_choisie = liste_joueurs[tour_id].choix_carte();

                carte_choisie.affiche();

                premiere_pioche = posecarte(ref carte_dessus, ref carte_choisie);

                if (!premiere_pioche)
                {
                    bool piocherdeuxfois = liste_joueurs[tour_id].pioche();

                    if (piocherdeuxfois)
                    {
                        carte_choisie = liste_joueurs[tour_id].dernierecartedudeck();
                        seconde_pioche = posecarte(ref carte_dessus, ref carte_choisie);
                    }
                }

                if (premiere_pioche || seconde_pioche)
                {
                    liste_joueurs[tour_id].retire(carte_choisie);
                }

                if (liste_joueurs[tour_id].Nbcartes == 0)
                {
                    break;
                }

                if (liste_joueurs[tour_id].Nbcartes == 1)
                {
                    Console.WriteLine("Uno!");
                }

                if (carte_choisie.Numero == 12 || carte_choisie.Numero == 11)
                {
                    string couleur;

                    do
                    {


                        Console.WriteLine($"Joueur {tour_id+1}, quelle couleur voulez vous mettre?");

                        try
                        {
                            couleur = Console.ReadLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            couleur = "Rouge";
                        }
                    }
                    while (couleur != "Rouge" && couleur != "Vert" && couleur != "Bleu" && couleur != "Jaune");

                    carte changement_couleur = new carte(0, couleur);
                    carte_dessus = changement_couleur;

                }

                tour_id++;

                if (tour_id >= nb_joueurs)
                {
                    tour_id = 0;
                }

                Console.WriteLine("Tour terminé");
                Console.WriteLine("_____________________");

                if (carte_choisie.Numero == 10)
                {
                    liste_joueurs[tour_id].plus_deuxouquatre(2);
                }

                if (carte_choisie.Numero == 11)
                {
                    liste_joueurs[tour_id].plus_deuxouquatre(4);
                }


                    
            }

            Console.WriteLine("Bon la victoire n'est pas encore codée mais on s'est compris");
            Console.WriteLine($"Le joueur {tour_id+1} est le grand gagnant");


        }

        public static bool posecarte(ref carte dernierecarte, ref carte macarte)
        {

            if ((dernierecarte.Couleur == "Special") || (macarte.Couleur == "Special" && dernierecarte.Numero != 11) || (macarte.Numero == 11) || (macarte.Couleur != "Special" && macarte.Couleur == dernierecarte.Couleur) || (macarte.Numero == dernierecarte.Numero))
            {
                Console.WriteLine("Vous avez pu poser votre carte!");
                dernierecarte = macarte;
                return true;
            }

            else
            {

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

            joueur J1 = new joueur();
            joueur J2 = new joueur();
            joueur J3 = new joueur();
            joueur J4 = new joueur();

            joueur[] totaljoueurs = { J1, J2, J3, J4 };

            carte depart = new carte();

            boucle_jeu(totaljoueurs.Length, totaljoueurs);
        }
    }
}
