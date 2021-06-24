using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using static Modele.Constante;

namespace Modele
{
    /// <summary>
    /// représente une oeuvre cinématographique avec comme propriétés un Titre, une date de sortie, etc.
    /// </summary>
    public abstract class Oeuvre : INotifyPropertyChanged

    {
    public string Titre { get; private set; }

    public DateTime DateDeSortie { get; private set; }

    /// <summary>
    /// Note moyenne de l'Oeuvre calculé a partir de la moyenne des avis
    /// </summary>
    public float NoteMoyenne => ListeAvis.Count > LISTE_MIN ? ListeAvis.Average(kvp => kvp.Value.Note) : DEFAULT_VALUE;

    /// <summary>
    /// Contient le lien vers son image
    /// </summary>
    public string LienImage { get; private set; }

    public string Synopsis { get; private set; }

    public Themes Theme { get; private set; }

    public bool IsFamilleF { get; private set; }

    /// <summary>
    /// Contient une liste de personnes avec différents métiers (les métiers retenues seront Acteurs et réalisateurs)
    /// </summary>
    public ReadOnlyDictionary<string, Dictionary<Personne, string>> Personnes { get; }

    private readonly Dictionary<string, Dictionary<Personne, string>> _personnes = new();

    public ReadOnlyObservableCollection<KeyValuePair<User, Avis>> ListeAvis { get; }
    private readonly ObservableCollection<KeyValuePair<User, Avis>> _listeAvis = new();

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Initialise toutes les propriétés et créé le lien de toutes les ReadOnlyCollection vers leurs listes
    /// </summary>
    /// <param name="titre">Le titre de l'Oeuvre</param>
    /// <param name="dateDeSortie">La date de sortie de l'Oeuvre</param>
    /// <param name="lienImage">Le chemin de l'image de l'Oeuvre</param>
    /// <param name="synopsis">Le synopsis de l'Oeuvre</param>
    /// <param name="theme">Le theme de l'Oeuvre</param>
    /// <param name="isFamilleF">La valeur pour savoir si le film est pour la famille ou non</param>
    /// <seealso cref="Personne"/>
    /// <seealso cref="Avis"/>
    protected Oeuvre(string titre, DateTime dateDeSortie, string lienImage, string synopsis, Themes theme,
        bool isFamilleF)
    {
        Titre = titre;
        DateDeSortie = dateDeSortie;
        LienImage = lienImage;
        Synopsis = synopsis;
        Theme = theme;
        IsFamilleF = isFamilleF;

        Personnes = new ReadOnlyDictionary<string, Dictionary<Personne, string>>(_personnes);

        ListeAvis = new ReadOnlyObservableCollection<KeyValuePair<User, Avis>>(_listeAvis);
    }

    /// <summary>
    /// Permet de modifier une Oeuvre
    /// </summary>
    /// <param name="titre">Le nouveau titre de l'Oeuvre</param>
    /// <param name="dateDeSortie">La nouvelle date de sortie de l'Oeuvre</param>
    /// <param name="lienImage">Le nouveau lien de l'image de l'Oeuvre</param>
    /// <param name="synopsis">Le nouveau synopsis de l'Oeuvre</param>
    /// <param name="theme">Le nouveau Theme de l'Oeuvre</param>
    /// <param name="familleF">La nouvelle valeur du Family Friendly de l'Oeuvre</param>
    /// <param name="listePersonnes">La nouvelle liste de personne de l'Oeuvre</param>
    /// <seealso cref="Personne"/>
    internal void ModifierOeuvre(string titre, DateTime dateDeSortie, string lienImage, string synopsis,
        Themes theme, bool familleF,
        IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> listePersonnes)
    {
        Titre = titre;
        DateDeSortie = dateDeSortie;
        LienImage = lienImage;
        Synopsis = synopsis;
        Theme = theme;
        IsFamilleF = familleF;

        AjouterPersonnes(listePersonnes);
    }

    /// <summary>
    /// Permet d'ajouter une personne 
    /// </summary>
    /// <param name="personnes">La liste de personnes avec leurs métiers</param>
    /// <seealso cref="Personne"/>
    protected void AjouterPersonnes(
        IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<Personne, string>>>> personnes)
    {
        _personnes.Clear();
        foreach (var (key, value) in personnes)
        {
            var dic = value.ToDictionary(pair => pair.Key, pair => pair.Value);
            _personnes.Add(key, dic);
        }
    }

    /// <summary>
    /// Permet d'ajouter un avis 
    /// </summary>
    /// <param name="user">L'utilisateur qui ajoute l'avis</param>
    /// <param name="avis">L'avis laissé par l'utilisateur</param>
    /// <seealso cref="User"/>
    /// <seealso cref="Avis"/>
    internal void AjouterAvis(User user, Avis avis)
    {
        if (_listeAvis.ToDictionary(pair => pair.Key, pair => pair.Value).ContainsKey(user)) return;

        _listeAvis.Add(new KeyValuePair<User, Avis>(user, avis));
        OnPropertyChanged(nameof(NoteMoyenne));
    }

    /// <summary>
    /// Permet de retirer un avis
    /// </summary>
    /// <param name="user">L'utilisateur à qui l'avis est lié</param>
    /// <seealso cref="User"/>
    /// <seealso cref="Avis"/>
    internal void RetirerAvis(User user)
    {
        foreach (var kvp in _listeAvis.ToDictionary(pair => pair.Key, pair => pair.Value)
            .Where(kvp => kvp.Key.Equals(user)))
        {
            _listeAvis.Remove(kvp);
            OnPropertyChanged(nameof(NoteMoyenne));
        }
    }

    internal virtual void AjouterOeuvres(IEnumerable<Oeuvre> oeuvres)
    {
        throw new NotImplementedException();
    }

    internal virtual void RetirerOeuvre(Oeuvre oeuvre)
    {
        throw new NotImplementedException();
    }
    }
}