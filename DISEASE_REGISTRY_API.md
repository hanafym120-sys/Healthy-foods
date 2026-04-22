# Disease Registry API Documentation

## Overview
The Disease Registry API provides a comprehensive system for managing medical conditions and diseases. It includes a pre-populated registry of 40+ common diseases across multiple medical categories and allows users to register their health conditions.

## Base URL
```
/api/v1/disease-registry
```

## Endpoints

### 1. Get All Diseases
**Endpoint:** `GET /diseases`

**Description:** Retrieve all available diseases in the registry, organized by category.

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "Type 2 Diabetes",
      "description": "A metabolic disorder characterized by high blood sugar levels",
      "category": "Metabolic",
      "isActive": true
    },
    ...
  ]
}
```

---

### 2. Get Diseases by Category
**Endpoint:** `GET /diseases/category/{category}`

**Parameters:**
- `category` (string): Disease category name (e.g., "Metabolic", "Cardiovascular", "Respiratory", "Neurological", "Renal", "Hepatic", "Mental Health", "Digestive", "Autoimmune", "Skeletal", "Oncological", "Hematological")

**Example:**
```
GET /api/v1/disease-registry/diseases/category/Cardiovascular
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "name": "Hypertension (High Blood Pressure)",
      "description": "Persistent elevated blood pressure that may lead to heart disease",
      "category": "Cardiovascular",
      "isActive": true
    },
    ...
  ]
}
```

---

### 3. Register Disease for User
**Endpoint:** `POST /register`

**Description:** Register a disease diagnosis for a specific user.

**Request Body:**
```json
{
  "userId": "550e8400-e29b-41d4-a716-446655440010",
  "diseaseId": "550e8400-e29b-41d4-a716-446655440001",
  "diagnosedDate": "2025-03-15T00:00:00Z",
  "notes": "Doctor recommended dietary changes"
}
```

**Parameters:**
- `userId` (GUID): The ID of the user registering the disease
- `diseaseId` (GUID): The ID of the disease from the registry
- `diagnosedDate` (DateTime): The date when the disease was diagnosed
- `notes` (string, optional): Additional clinical notes

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "id": "660e8400-e29b-41d4-a716-446655440020",
    "userId": "550e8400-e29b-41d4-a716-446655440010",
    "disease": {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "name": "Hypertension (High Blood Pressure)",
      "description": "Persistent elevated blood pressure that may lead to heart disease",
      "category": "Cardiovascular",
      "isActive": true
    },
    "diagnosedDate": "2025-03-15T00:00:00Z",
    "notes": "Doctor recommended dietary changes",
    "registeredAt": "2026-04-16T12:30:45.123Z"
  }
}
```

---

### 4. Get User's Registered Diseases
**Endpoint:** `GET /user/{userId}/diseases`

**Description:** Retrieve all diseases registered for a specific user.

**Parameters:**
- `userId` (GUID): The ID of the user

**Example:**
```
GET /api/v1/disease-registry/user/550e8400-e29b-41d4-a716-446655440010/diseases
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "660e8400-e29b-41d4-a716-446655440020",
      "userId": "550e8400-e29b-41d4-a716-446655440010",
      "disease": {
        "id": "550e8400-e29b-41d4-a716-446655440001",
        "name": "Hypertension (High Blood Pressure)",
        "description": "Persistent elevated blood pressure that may lead to heart disease",
        "category": "Cardiovascular",
        "isActive": true
      },
      "diagnosedDate": "2025-03-15T00:00:00Z",
      "notes": "Doctor recommended dietary changes",
      "registeredAt": "2026-04-16T12:30:45.123Z"
    },
    {
      "id": "660e8400-e29b-41d4-a716-446655440021",
      "userId": "550e8400-e29b-41d4-a716-446655440010",
      "disease": {
        "id": "550e8400-e29b-41d4-a716-446655440000",
        "name": "Type 2 Diabetes",
        "description": "A metabolic disorder characterized by high blood sugar levels",
        "category": "Metabolic",
        "isActive": true
      },
      "diagnosedDate": "2024-06-20T00:00:00Z",
      "notes": "Managed with medication and diet",
      "registeredAt": "2026-04-16T11:15:30.456Z"
    }
  ]
}
```

---

### 5. Check if User Has Disease
**Endpoint:** `GET /user/{userId}/has-disease/{diseaseId}`

**Description:** Check if a user has a specific disease registered.

**Parameters:**
- `userId` (GUID): The ID of the user
- `diseaseId` (GUID): The ID of the disease

**Example:**
```
GET /api/v1/disease-registry/user/550e8400-e29b-41d4-a716-446655440010/has-disease/550e8400-e29b-41d4-a716-446655440001
```

**Response:**
```json
{
  "success": true,
  "data": true
}
```

---

### 6. Remove Disease Registration
**Endpoint:** `DELETE /{userDiseaseId}`

**Description:** Remove a disease registration from a user's records.

**Parameters:**
- `userDiseaseId` (GUID): The ID of the user disease registration (returned from registration)

**Example:**
```
DELETE /api/v1/disease-registry/660e8400-e29b-41d4-a716-446655440020
```

**Response:**
```json
{
  "success": true,
  "message": "Disease registration removed successfully"
}
```

---

## Disease Categories

The system includes diseases from the following categories:

1. **Metabolic** - Type 2 Diabetes, Type 1 Diabetes, Obesity, Metabolic Syndrome
2. **Endocrine** - Thyroid Disorder
3. **Cardiovascular** - Hypertension, Coronary Artery Disease, Heart Failure, Atrial Fibrillation, High Cholesterol
4. **Respiratory** - Asthma, COPD, Sleep Apnea
5. **Neurological** - Stroke, Parkinson's Disease, Alzheimer's Disease, Epilepsy
6. **Renal** - Chronic Kidney Disease, Urinary Tract Infection
7. **Hepatic** - Fatty Liver Disease, Hepatitis, Cirrhosis
8. **Mental Health** - Depression, Anxiety Disorder
9. **Digestive** - IBS, IBD, GERD
10. **Autoimmune** - Rheumatoid Arthritis, SLE
11. **Skeletal** - Osteoporosis, Osteoarthritis
12. **Oncological** - Cancer
13. **Hematological** - Anemia

---

## Error Handling

### Common Error Responses

**400 Bad Request**
```json
{
  "success": false,
  "message": "Invalid user ID or disease ID"
}
```

**404 Not Found**
```json
{
  "success": false,
  "message": "User not found"
}
```

**409 Conflict** (Duplicate Registration)
```json
{
  "success": false,
  "message": "This user already has this disease registered"
}
```

**500 Internal Server Error**
```json
{
  "success": false,
  "message": "An unexpected error occurred"
}
```

---

## Usage Examples

### Register Multiple Diseases for a User

```bash
# Step 1: Get all diseases
curl -X GET "https://api.example.com/api/v1/disease-registry/diseases"

# Step 2: Register Type 2 Diabetes
curl -X POST "https://api.example.com/api/v1/disease-registry/register" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "550e8400-e29b-41d4-a716-446655440010",
    "diseaseId": "550e8400-e29b-41d4-a716-446655440000",
    "diagnosedDate": "2024-06-20",
    "notes": "Managed with medication and diet"
  }'

# Step 3: Register Hypertension
curl -X POST "https://api.example.com/api/v1/disease-registry/register" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "550e8400-e29b-41d4-a716-446655440010",
    "diseaseId": "550e8400-e29b-41d4-a716-446655440001",
    "diagnosedDate": "2025-03-15",
    "notes": "Doctor recommended dietary changes"
  }'

# Step 4: Get all registered diseases for the user
curl -X GET "https://api.example.com/api/v1/disease-registry/user/550e8400-e29b-41d4-a716-446655440010/diseases"
```

---

## Technical Details

### Database Schema

**Diseases Table:**
- `Id` (GUID, Primary Key)
- `Name` (string)
- `Description` (string)
- `Category` (string)
- `IsActive` (bool)
- `CreatedAt` (DateTime)

**UserDiseases Table:**
- `Id` (GUID, Primary Key)
- `UserId` (GUID, Foreign Key)
- `DiseaseId` (GUID, Foreign Key)
- `DiagnosedDate` (DateTime)
- `Notes` (string, nullable)
- `RegisteredAt` (DateTime)
- **Unique Constraint:** (UserId, DiseaseId) - prevents duplicate registrations

### Service Layer Architecture

The implementation follows senior-level best practices:

1. **Separation of Concerns** - Controller handles HTTP, Service handles business logic
2. **Dependency Injection** - Services are injected via constructor
3. **Entity Framework** - Database operations with async/await patterns
4. **Error Handling** - Comprehensive exception handling with meaningful messages
5. **SOLID Principles** - Interface-based design with single responsibility
6. **Query Optimization** - AsNoTracking for read operations, proper eager loading

---

## Frontend Integration Guide

### Display List of Diseases (Register Button)

```javascript
// Fetch all diseases
async function loadDiseaseRegistry() {
  const response = await fetch('/api/v1/disease-registry/diseases');
  const result = await response.json();
  
  // Group by category for better UI
  const diseasesByCategory = result.data.reduce((acc, disease) => {
    if (!acc[disease.category]) {
      acc[disease.category] = [];
    }
    acc[disease.category].push(disease);
    return acc;
  }, {});
  
  displayDiseaseRegistry(diseasesByCategory);
}

// Register disease for user
async function registerDisease(userId, diseaseId, diagnosedDate, notes) {
  const response = await fetch('/api/v1/disease-registry/register', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      userId,
      diseaseId,
      diagnosedDate,
      notes
    })
  });
  
  return await response.json();
}

// Get user's diseases
async function getUserDiseases(userId) {
  const response = await fetch(`/api/v1/disease-registry/user/${userId}/diseases`);
  return await response.json();
}
```

---

## Future Enhancements

- Add disease symptom tracking
- Implement medication history tracking
- Add disease severity levels
- Create analytics/reporting endpoints
- Add disease interaction warnings
- Implement disease timeline views
