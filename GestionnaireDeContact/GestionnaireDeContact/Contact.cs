namespace GestionnaireDeContact {
    internal class Contact {

        public string numero { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string mel { get; set; }

        public Contact(string numero, string prenom, string nom = "", string mel = "") {

            this.numero = numero;
            this.nom = nom;
            this.prenom = prenom;
            this.mel = mel;
        }

        public override string ToString() {
            return $"Téléphone: {numero}, Prénom: {prenom}, Nom: {nom}, Email: {mel}";
        }
    }
}
