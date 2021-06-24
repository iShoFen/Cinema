using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Modele;

namespace Appli.Utils
{
    public static class CreerPersonne
    {
        private static Manager Man => (Application.Current as App)?.Man;

        private static (string prenom, string nom, string role) SplitNomPrenom(string group)
        {
            var tabLast = group.Split(" ", 2);
            if (tabLast is { Length: <= 1 })
                return (null, null, null);

            if (!tabLast[1].Contains(":"))
                return (tabLast[0].Trim().Replace(tabLast[0][0], char.ToUpper(tabLast[0][0])),
                        tabLast[1].Trim().Replace(tabLast[1][0], char.ToUpper(tabLast[1][0])), null);

            var tabNomRole = tabLast[1].Split(":", 2);
             return tabNomRole is {Length: <= 1}
                ? (tabLast[0].Trim().Replace(tabLast[0][0], char.ToUpper(tabLast[0][0])),
                    tabLast[1].Trim().Replace(tabLast[1][0], char.ToUpper(tabLast[1][0])), null)

                : (tabLast[0].Trim().Replace(tabLast[0][0], char.ToUpper(tabLast[0][0])),
                    tabNomRole[0].Trim().Replace(tabNomRole[0][0], char.ToUpper(tabLast[1][0])),
                    tabNomRole[1].Trim().Replace(tabNomRole[1][0], char.ToUpper(tabNomRole[1][0])));
        }

        public static bool ActeurUtil(MetroWindow window, string acteur, out Dictionary<Personne, string> personnes)
        {
            personnes = new Dictionary<Personne, string>();

            var list = acteur.Split("\n");

            foreach (var group in list)
            {
                var (prenom, nom, role) = SplitNomPrenom(group);

                if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(nom))
                {
                    window.ShowModalMessageExternal("Nouvel acteur incorrect",
                        "Vous devez rentrer (Prénom Nom : Rôle) !");
                    return false;
                }

                var testPersonne = Man.HasPersonne(prenom, nom, out var pers);
                if (testPersonne) personnes[pers] = role;
                else
                {
                     window.ShowModalMessageExternal("Acteur à créer", $"L'acteur : {prenom} {nom} doit être créé !");

                    Man.CurrentPersonne = null;
                    new AjouterPersonne().ShowDialog();

                    if (Man.CurrentPersonne is null ||
                        !Man.CurrentPersonne.Prenom.Equals(prenom) || !Man.CurrentPersonne.Nom.Equals(nom))
                    {
                        window.ShowModalMessageExternal("Nouvel acteur incorrect",
                            "Vous devez créer un acteur ayant les mêmes nom et prénom que ceux que vous avez choisi !");
                        return false;
                    }

                    if (!personnes.ContainsKey(Man.CurrentPersonne)) personnes[Man.CurrentPersonne] = role;
                }
            }

            return true;
        }

        public static bool RealUtil(MetroWindow window, string real, out Dictionary<Personne, string> personnes)
        {
            personnes = new Dictionary<Personne, string>();

            var list = real.Split("\n");

            foreach (var group in list)
            {
                var (prenom, nom, _) = SplitNomPrenom(group);

                if (string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(nom))
                {
                    window.ShowModalMessageExternal("Nouveau réalisateur incorrect",
                        "Vous devez rentrer (Prénom Nom) !");
                    return false;
                }

                var testPersonne = Man.HasPersonne(prenom, nom, out var pers);

                if (testPersonne) personnes[pers] = "";
                else
                {
                    window.ShowModalMessageExternal("Réalisateur à créer", $"Le réalisateur : {prenom} {nom} doit être créé !");

                    Man.CurrentPersonne = null;
                    new AjouterPersonne().ShowDialog();

                    if (Man.CurrentPersonne is null ||
                        !Man.CurrentPersonne.Prenom.Equals(prenom)|| !Man.CurrentPersonne.Nom.Equals(nom))
                    {
                        window.ShowModalMessageExternal("Nouveau réalisateur incorrect",
                            "Vous devez créer un réalisateur ayant les mêmes nom et prénom que ceux que vous avez choisi !");
                        return false;
                    }

                    if (!personnes.ContainsKey(Man.CurrentPersonne)) personnes[Man.CurrentPersonne] = "";
                }
            }

            return true;
        }
    }
}
