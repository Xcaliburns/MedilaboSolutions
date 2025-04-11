# MedilaboSolutions

## 📝 Description
Ce projet développé avec **.NET** implémente une solution en microservices pour la gestion de patients et calcul de risques pour le diabete

## 🚀 Technologies Utilisées
- **Framework** : .NET 8
- **Base de données** : SQL Server / MongoDb
- **Architecture** : Microservices avec Ocelot Gateway
- **Conteneurs** : Docker

#### 1. **Cloner le projet et éxecuter la solution**
- Avant de pouvoir lancer le projet, il faut récupérer les fichiers sources en clonant le dépôt GitHub. Utilisez la commande suivante dans votre terminal :
- git clone https://github.com/Xcaliburns/MedilaboSolutions.git 
- se placer au niveau de la racine du projet : cd MedilaboSolutions
- git checkout dev (la branche dev est la branche fonctionnelle pour le moment)
- effectuer la commande : docker-compose up --build


## 🌱 Recommandations Green Code

### 🛠 Optimisation du Code  à envisager
- Réduction de la complexité des algorithmes pour limiter les cycles CPU.
- Utilisation d'un système de cache (**MemoryCache**, **Redis**) pour minimiser les accès à la base de données.
- Chargement des données avec **lazy loading** pour éviter la surcharge mémoire.

### 🛠 Optimisation du Code effectuées
- ajout d'une options de configuration de cache dans le gateway pour limiter les appels aux bases de données pour des requetes identiques
 "CacheOptions": { "TtlSeconds": 120 }  

### 🐳 Conteneurs Docker
- Utiliser des images Docker légères comme **mcr.microsoft.com/dotnet/runtime:8.0-alpine**(quand cela est possible)
- Nettoyer les conteneurs inutilisés régulièrement : 
  docker system prune -f
