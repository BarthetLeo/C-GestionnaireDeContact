using System.Text.Json;
using GestionnaireDeContact;

public class Gestionnaire {

    private static string path = @"..\..\..\..\Gestionnaire\Contact.json";
    private static List<Contact> contacts = new List<Contact>();

    static void Main() {

        int resultat = 0;
        string? input;

        if (!Directory.Exists(@"..\..\..\..\Gestionnaire"))
            Directory.CreateDirectory(@"..\..\..\..\Gestionnaire");

        if (!File.Exists(path)) {
            using (StreamWriter sw = File.CreateText(path))
                sw.WriteLine("Contacts existants :\n\n");
        }

        ChargerContact();

        do {
            Console.Write("\n" + @"Bienvenue dans le gestionnaire de mot contact. Que souhaitez vous faire ? :
        1: Afficher la liste des contacts
        2: Ajouter un contact
        3: Supprimer un ou plusieurs contacts
        5: Quitter le programme" + "\n\n");

            input = Console.ReadLine();

            while (!Int32.TryParse(input, out resultat) || (resultat != 1 && resultat != 2 && resultat != 3 && resultat != 4 && resultat != 5)) {
                Console.WriteLine("Entrée invalide. Veuillez entrer un nombre entre 1 et 4 :");
                input = Console.ReadLine();
            }

            Console.Clear();

            switch (resultat) {
                case 1:
                    AfficherContact();
                    break;
                case 2:
                    CreerContacts();
                    break;
                case 3:
                    SupprimerContact();
                    break;
                case 4:
                    Console.WriteLine("Fermeture du programme...");
                    return;
                default:
                    Console.WriteLine("Option non reconnue.");
                    break;
            }
        } while (true);
    }

    public static void ChargerContact() {
        using (StreamReader sr = new StreamReader(path)) {
            string json = sr.ReadToEnd();
            contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        }
    }

    public static void AfficherContact() {
        if (contacts.Count == 0) {

            Console.WriteLine("Aucun contact a afficher");
            return;
        }

        Console.WriteLine("Liste des contacts :");

        for (int i = 0; i < contacts.Count; i++)
            Console.WriteLine( (i+1) + " : " + contacts[i]);

        Console.WriteLine("");
    }

    public static void CreerContacts() {

        string[] informations;

        Console.WriteLine("Information sur le contact à écrire comme suit : \n " +
            "Numero Prenom Nom Mel (seuls Numero et prenom sont obligatoire)\n");

        do {
            informations = Console.ReadLine().Split(' ');
        } while (!ValidationInformationContact(informations));

        string numero = informations[0];
        string prenom = informations[1];
        string nom = informations.Length > 2 ? informations[2] : " ";
        string mel = informations.Length > 3 ? informations[3] : " ";

        contacts.Add(new Contact(numero, prenom, nom, mel));

        Console.WriteLine("Ajout avec succès");

        SauvegarderContact();
    }

    public static void SupprimerContact() {

        if (contacts.Count == 0) {
            Console.WriteLine("Aucun mot de passe a supprimer"); return;
        }

        AfficherContact();

        Console.WriteLine("Quel mot de passe souhaitez vous supprimer ?\n");

        int index = 0;

        do {
            int.TryParse(Console.ReadLine(), out index);

            if (!(index > 0 && index <= contacts.Count))
                Console.WriteLine("Numéro invalide.");

        } while (!(index > 0 && index <= contacts.Count));

        contacts.RemoveAt(index - 1);

        SauvegarderContact();

        Console.WriteLine("Contact supprimé avec succès !");
    }

    public static void SauvegarderContact() {
        string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public static bool ValidationInformationContact(string[] informations) {

        if(informations.Length < 2 || informations.Length > 4)
            return false;

        if (!(informations[0].Length == 10 && informations[0].StartsWith('0'))) {
            Console.WriteLine("Numero invalide");
            return false;
        }

        if (informations.Length == 4 && !informations[3].Contains('@') &&
            !informations[3].StartsWith('@') && !informations[3].EndsWith('@')) {
            Console.WriteLine("mel invalide");
            return false;
        }

        return true;
    }
}