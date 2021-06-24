using System.Collections.Generic;
using System.Linq;

namespace Modele
{
    /// <summary>
    /// Partie concernant les Streaming du Manager
    /// <seealso cref="Streaming"/>
    /// </summary>
    public partial class Manager
    {
        /// <summary>
        /// Permet de créer des Streaming lors de la création d'une Oeuvre
        /// </summary>
        /// <param name="titre">Le titre du film auquel est lié se Streaming</param>
        /// <param name="d">Un dictionnaire ayant pour clef la Plateforme de Streaming et en valeur le lien</param>
        /// <returns>Rend tous les Streaming créer dans une liste</returns>
        /// <seealso cref="Streaming"/>
        /// <seealso cref="Oeuvre"/>
        public IEnumerable<Streaming>
            AjouterStreamLeaf(string titre, IEnumerable<KeyValuePair<Plateformes, string>> d) =>
            _factory.AjouterStreamLeaf(titre, d);
    }
}