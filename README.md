# MedilaboSolutions

## 📝 Description
Ce projet développé avec **.NET** implémente une solution en microservices pour la gestion de patients et calcul de risques pour le diabete

## 🚀 Technologies Utilisées
- **Framework** : .NET 8
- **Base de données** : SQL Server / MongoDb
- **Architecture** : Microservices avec Ocelot Gateway
- **Conteneurs** : Docker

#### 1. **Cloner le projet et éxecuter la solution**
- Docker doit etre installé. https://www.docker.com/
- Avant de pouvoir lancer le projet, il faut récupérer les fichiers sources en clonant le dépôt GitHub. Utilisez la commande suivante dans votre terminal :
- git clone https://github.com/Xcaliburns/MedilaboSolutions.git 
- se placer au niveau de la racine du projet : cd MedilaboSolutions
- git checkout dev (la branche dev est la branche fonctionnelle pour le moment)
- dotnet restore 
- effectuer la commande : docker-compose up --build
- http://localhost:5011/ depuis un navigateur
- login :
    - rôle organisateur : organisateur / Organisateur@123
    - rôle praticien    : praticien    / Praticien@123


## 🌱 Recommandations Green Code

### 🛠 Optimisation du Code à envisager
- Réduction de la complexité des algorithmes pour limiter les cycles CPU.
- Utilisation d'un système de cache (**MemoryCache**, **Redis**) pour minimiser les accès à la base de données.
- Chargement des données avec **lazy loading** pour éviter la surcharge mémoire, utile pour les relations rarement consultées.
- Utilisation du **eager loading** pour optimiser les performances en cas de relations fréquemment utilisées, en chargeant toutes les données nécessaires en une seule requête.

### 🛠 Optimisation du Code effectuées
- ajout d'une options de configuration de cache dans le gateway pour limiter les appels aux bases de données pour des requetes identiques
 "CacheOptions": { "TtlSeconds": 120 }
- Utilisation du **eager loading** (via `.Include()` dans Entity Framework) pour charger les données associées en une seule requête, évitant les N+1 requêtes et optimisant les performances.

### 🐳 Conteneurs Docker
- Utiliser des images Docker légères comme **mcr.microsoft.com/dotnet/runtime:8.0-alpine**(quand cela est possible)
- Nettoyer les conteneurs inutilisés régulièrement : 
  docker system prune -f (en creant un service dédié au nettoyage)
