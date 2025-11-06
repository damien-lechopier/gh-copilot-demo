# Albums API - Documentation

Cette API REST permet de gérer une collection d'albums musicaux avec les opérations CRUD complètes et une fonctionnalité de recherche par année.

## Base URL
```
http://localhost:5000/api/Album
```

## Endpoints

### 1. Récupérer tous les albums
- **GET** `/api/Album`
- **Description** : Récupère la liste complète des albums
- **Réponse** : Array d'objets Album

**Exemple de réponse :**
```json
[
  {
    "id": 1,
    "title": "You, Me and an App Id",
    "artist": "Daprize",
    "price": 10.99,
    "image_url": "https://aka.ms/albums-daprlogo",
    "year": 2023
  }
]
```

### 2. Récupérer un album par ID
- **GET** `/api/Album/{id}`
- **Description** : Récupère un album spécifique par son ID
- **Paramètres** : 
  - `id` (int) : L'ID de l'album
- **Réponses** :
  - `200 OK` : Album trouvé
  - `404 Not Found` : Album non trouvé

### 3. Rechercher des albums par année
- **GET** `/api/Album/year/{year}`
- **Description** : Récupère tous les albums d'une année spécifique
- **Paramètres** :
  - `year` (int) : L'année de recherche
- **Réponse** : Array d'objets Album de l'année spécifiée

**Exemple :**
```
GET /api/Album/year/2023
```

### 4. Créer un nouvel album
- **POST** `/api/Album`
- **Description** : Crée un nouvel album
- **Body** : Objet Album (l'ID sera généré automatiquement)
- **Réponses** :
  - `201 Created` : Album créé avec succès
  - `400 Bad Request` : Données invalides

**Exemple de body :**
```json
{
  "title": "Nouvel Album",
  "artist": "Nouvel Artiste",
  "price": 14.99,
  "image_url": "https://example.com/image.jpg",
  "year": 2023
}
```

### 5. Mettre à jour un album
- **PUT** `/api/Album/{id}`
- **Description** : Met à jour un album existant
- **Paramètres** :
  - `id` (int) : L'ID de l'album à mettre à jour
- **Body** : Objet Album avec les nouvelles données
- **Réponses** :
  - `200 OK` : Album mis à jour avec succès
  - `404 Not Found` : Album non trouvé
  - `400 Bad Request` : Données invalides

### 6. Supprimer un album
- **DELETE** `/api/Album/{id}`
- **Description** : Supprime un album
- **Paramètres** :
  - `id` (int) : L'ID de l'album à supprimer
- **Réponses** :
  - `204 No Content` : Album supprimé avec succès
  - `404 Not Found` : Album non trouvé

## Modèle de données

### Album
```json
{
  "id": "int",
  "title": "string",
  "artist": "string", 
  "price": "double",
  "image_url": "string",
  "year": "int"
}
```

## Validation
- `title` et `artist` sont requis et ne peuvent pas être vides
- `price` doit être un nombre valide
- `year` a une valeur par défaut de 2023

## Exemples d'utilisation avec curl

### Récupérer tous les albums
```bash
curl -X GET http://localhost:5000/api/Album
```

### Récupérer un album par ID
```bash
curl -X GET http://localhost:5000/api/Album/1
```

### Rechercher par année
```bash
curl -X GET http://localhost:5000/api/Album/year/2023
```

### Créer un nouvel album
```bash
curl -X POST http://localhost:5000/api/Album \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Mon Album",
    "artist": "Mon Artiste",
    "price": 12.99,
    "image_url": "https://example.com/image.jpg",
    "year": 2024
  }'
```

### Mettre à jour un album
```bash
curl -X PUT http://localhost:5000/api/Album/1 \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Album Mis à Jour",
    "artist": "Artiste Mis à Jour", 
    "price": 15.99,
    "image_url": "https://example.com/updated.jpg",
    "year": 2024
  }'
```

### Supprimer un album
```bash
curl -X DELETE http://localhost:5000/api/Album/1
```