# Système de Gestion des Approvisionnements

Application web ASP.NET Core MVC pour gérer les approvisionnements d'une entreprise.

## 🚀 Fonctionnalités

- **Dashboard** : Vue d'ensemble avec statistiques (montant total, nombre d'approvisionnements, fournisseur principal)
- **Gestion des Approvisionnements** :
  - Liste complète avec filtres (date, fournisseur, article)
  - Création d'approvisionnements avec ajout dynamique d'articles
  - Visualisation détaillée
  - Suppression
- **Recherche en temps réel** dans la liste
- **Calcul automatique** des montants

## 🛠️ Technologies utilisées

- **Backend** : ASP.NET Core 9.0 MVC
- **ORM** : Entity Framework Core 9.0
- **Base de données** : MySQL 8.0 (via Pomelo.EntityFrameworkCore.MySql)
- **Frontend** : Bootstrap 5, Bootstrap Icons, jQuery
- **Architecture** : MVC avec pattern Repository/Service

## 📋 Prérequis

- .NET 9.0 SDK
- MySQL 8.0 (WAMP, XAMPP ou serveur MySQL)
- Visual Studio 2022 ou VS Code

## ⚙️ Installation

1. **Cloner le projet**
   ```bash
   git clone <votre-repo>
   cd WebGestionAppro
   ```

2. **Configurer la base de données**
   
   Modifiez la chaîne de connexion dans `appsettings.json` si nécessaire :
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Port=3306;Database=GesApproDB;User=root;Password=;"
   }
   ```

3. **Restaurer les packages**
   ```bash
   dotnet restore
   ```

4. **Créer la base de données**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   
   Ou lancez simplement l'application (la base sera créée automatiquement avec des données de test) :
   ```bash
   dotnet run
   ```

5. **Accéder à l'application**
   
   Ouvrez votre navigateur à : `https://localhost:5001`

## 📊 Données de test

L'application inclut des données de démonstration :
- 3 fournisseurs (Textiles Dakar SARL, Mercerie Centrale, Tissus Premium)
- 5 articles (tissus, fils, boutons, fermetures)

## 🗂️ Structure du projet

```
WebGestionAppro/
├── Controllers/          # Contrôleurs MVC
├── Models/              # Modèles de données et ViewModels
├── Views/               # Vues Razor
├── Services/            # Logique métier
├── Data/                # DbContext
└── wwwroot/            # Fichiers statiques (CSS, JS)
```

## 📝 Modèles de données

- **Fournisseur** : Informations des fournisseurs
- **Article** : Catalogue des articles
- **Approvisionnement** : En-têtes des approvisionnements
- **ApprovisionnementArticle** : Détails des articles par approvisionnement

## 🔧 Configuration

La base de données utilise MySQL. Assurez-vous que :
- MySQL est installé et démarré
- Le port 3306 est disponible
- L'utilisateur `root` a les permissions nécessaires

## 📄 Licence

Ce projet est développé à des fins éducatives.

## 👤 Auteur

Développé avec ASP.NET Core MVC
