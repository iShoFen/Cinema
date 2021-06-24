using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modele
{
    public interface IPersistance
    {
        public (IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users) Charger();

        public void Sauvegarder(IEnumerable<Personne> personnes, IEnumerable<Oeuvre> oeuvres, IEnumerable<User> users);
    }
}
