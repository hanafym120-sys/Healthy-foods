# HealthyBites API - Developer Documentation

## 1. Purpose

This document explains:

- what has already been implemented in the project
- what still needs to be completed
- the recommended next steps in a clear execution order

It is intended to help future development continue without confusion.

---

## 2. Current Project Status

The project was updated to align the existing codebase with the required HealthyBites API scope while preserving the healthy parts of the original implementation.

The code currently:

- builds successfully
- has updated authentication logic
- has updated meals catalog logic
- has restaurant and review APIs
- has the weight dynamics endpoint aligned with the documentation
- has JWT authentication infrastructure enabled

The database side still needs migration/update work before full runtime testing.

---

## 3. What Was Implemented

### 3.1 User and Authentication

The authentication module was reworked to support the requested smart nutrition flow.

Implemented:

- updated `User` model with:
  - `Id`
  - `Name`
  - `Email`
  - `PasswordHash`
  - `Age`
  - `Weight`
  - `Height`
  - `Gender`
  - `ActivityLevel`
  - `Goal`
  - `DailyCalories`
  - `CreatedAt`
- added password reset OTP fields:
  - `ResetPasswordOtp`
  - `ResetPasswordOtpExpiresAt`
- preserved weight-dynamics-related fields from the old project:
  - `SmokingStatus`
  - `SleepHours`
  - `StressLevel`
  - `HasType2Diabetes`

Created/updated:

- `Services/IAuthService.cs`
- `Services/AuthService.cs`
- `Controllers/AuthController.cs`
- `Services/ITokenService.cs`
- `Services/TokenService.cs`

Added/updated DTOs:

- `DTOs/RegisterDto.cs`
- `DTOs/LoginDto.cs`
- `DTOs/ForgotPasswordDto.cs`
- `DTOs/ResetPasswordDto.cs`
- `DTOs/UpdateProfileDto.cs`
- `DTOs/UserProfileDto.cs`
- `DTOs/AuthResultDto.cs`

Supported endpoints:

- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`
- `POST /api/v1/auth/forgot-password`
- `POST /api/v1/auth/reset-password`
- `GET /api/v1/auth/profile/{userId}`
- `PUT /api/v1/auth/profile/{userId}`

Behavior implemented:

- registration creates user
- password is hashed using SHA256
- daily calories are calculated on register and profile update
- user diseases are linked during registration or update
- login returns JWT token
- forgot password generates a 6-digit OTP and sends it through `IEmailService`
- reset password validates OTP and updates password

---

### 3.2 Calories Calculation

Implemented inside `AuthService` using the requested logic:

- BMR using Mifflin-St Jeor equation
- TDEE using activity factor
- calories adjusted by goal:
  - `loss`
  - `maintain`
  - `gain`

The result is stored in:

- `User.DailyCalories`

---

### 3.3 Meals Module

The meals part was changed from old daily meal tracking behavior to a meal catalog structure that better matches the provided dataset and requested frontend behavior.

Implemented:

- `Models/Meal.cs`
- `Services/IMealService.cs`
- `Services/MealService.cs`
- `Controllers/MealController.cs`

Added DTOs:

- `DTOs/MealDto.cs`
- `DTOs/UpsertMealDto.cs`

Supported endpoints:

- `GET /api/v1/meals`
- `GET /api/v1/meals/{id}`
- `GET /api/v1/meals/for-user/{userId}`
- `POST /api/v1/meals`
- `PUT /api/v1/meals/{id}`
- `DELETE /api/v1/meals/{id}`

Current meal model supports:

- `Name`
- `ImageUrl`
- `Ingredients`
- `Calories`
- `MealType`
- `DiseaseTags`
- `IsActive`
- `CreatedAt`

Current filtering behavior:

- filter by meal type
- search by keyword
- filter meals for a user based on diseases

Important note:

- the current implementation is only an intermediate structure
- after reviewing the real meal dataset, the meal schema must be expanded further
- the disease filtering logic must also be tightened for multi-disease users

---

### 3.4 Restaurants Module

Implemented:

- updated `Models/Restaurant.cs`
- updated `Models/RestaurantReview.cs`
- `Services/IRestaurantService.cs`
- `Services/RestaurantService.cs`
- `Controllers/RestaurantsController.cs`

Added DTOs:

- `DTOs/RestaurantDto.cs`
- `DTOs/RestaurantReviewDto.cs`
- `DTOs/CreateRestaurantDto.cs`
- `DTOs/CreateRestaurantReviewDto.cs`

Supported endpoints:

- `GET /api/v1/restaurants`
- `GET /api/v1/restaurants/{id}`
- `POST /api/v1/restaurants`
- `POST /api/v1/restaurants/{id}/reviews`
- `GET /api/v1/restaurants/{id}/reviews`

Current restaurant model supports:

- `Name`
- `Phone`
- `Address`
- `GoogleMapsLink`
- `AverageRating`
- `CreatedAt`

Behavior implemented:

- create restaurant
- add restaurant review
- calculate average rating after review insertion
- return restaurant details with reviews

Important note:

- restaurant Google Maps support is conceptually prepared through `GoogleMapsLink`
- the real restaurant dataset contains more fields that should still be added

---

### 3.5 Weight Dynamics

Implemented:

- updated `Services/IWeightDynamicsService.cs`
- updated `Services/WeightDynamicsService.cs`
- updated `Controllers/WeightDynamicsController.cs`

Supported endpoint:

- `POST /api/v1/weight-dynamics/estimate`

Behavior:

- uses the physiology-based 30-day weight change formula
- uses:
  - BMR
  - TDEE
  - metabolic adaptation factor
  - lifestyle factor
- response format now follows the unified API pattern

---

### 3.6 DbContext and Program Setup

Implemented:

- updated `Data/AppDbContext.cs`
- updated `Program.cs`

Infrastructure changes:

- added `DbSet<User>`
- added `DbSet<Meal>`
- kept disease and restaurant entities wired up
- enabled JWT authentication
- added dependency injection registrations for:
  - `IAuthService`
  - `IMealService`
  - `IRestaurantService`
  - `IWeightDynamicsService`
  - existing services
- enabled:
  - `app.UseAuthentication()`
  - `app.UseAuthorization()`

---

## 4. Unified API Response Pattern

The updated controllers follow this shape:

```json
{
  "success": true,
  "data": {},
  "message": "optional message"
}
```

---

## 5. Dataset Findings and Their Impact

After reviewing the provided meal dataset, the following columns were found:

- `Meal_Name`
- `Cuisine`
- `Serving_Size_g`
- `Ingredients`
- `Preparation_Method`
- `Calories`
- `Carbohydrates_g`
- `Protein_g`
- `Total_Fat_g`
- `Saturated_Fat_g`
- `Sodium_mg`
- `Potassium_mg`
- `Diseases`
- `Suitable_For_Diabetes_And_Hypertension`
- `Medical_Benefit_Note`

The provided restaurant dataset contains:

- `restaurant_id`
- `restaurant_name`
- `branch`
- `city`
- `address`
- `phone`
- `rating`
- `category`
- `has_menu_online`

This means the frontend requirement is broader than the current `Meal` model.

The website is expected to show:

- meal name
- meal image
- preparation method
- meal medical benefit
- ingredients
- nutrition values as separate fields
- diseases supported by the meal

Nutrition values should be displayed as clearly separated fields, not as one block of raw text.

Recommended UI nutrition block:

- Calories
- Carbohydrates
- Protein
- Total Fat
- Saturated Fat
- Sodium
- Potassium

Disease presentation requirement:

- `Diabetes Type 1` must remain separate from `Diabetes Type 2`
- meals suitable for `Type 1` should not automatically appear for `Type 2`
- meals suitable for `Type 2` should not automatically appear for `Type 1`
- combined meals should appear only where their disease tags explicitly support the user condition

Important filtering rule from dataset understanding:

- if the user has multiple diseases, meals should match all of those diseases, not just any one of them

Example:

- if user diseases are `Diabetes Type 2` and `Obesity`
- only meals tagged for both should appear
- meals tagged for only one of them should not appear

This means:

- disease matching should be `ALL matching`
- disease matching should not be `ANY matching`

Meaning:

- wrong behavior:
  - user has `Type 2 + Obesity`
  - system returns meal for `Type 2` only
  - system returns meal for `Obesity` only
- correct behavior:
  - user has `Type 2 + Obesity`
  - system returns meals tagged with both `Type 2` and `Obesity`

Special dataset note:

- `Suitable_For_Diabetes_And_Hypertension` exists in the meal dataset
- this can be preserved as an extra display/filter field if needed

Dataset integration strategy:

- the Excel files should not be used directly by the frontend
- correct flow should be:
  - Excel dataset
  - import process
  - SQL Server database
  - API response
  - website frontend

This is the intended production data flow.

---

## 6. What Still Needs To Be Done

### 6.1 Database Migration and Update

Still required:

1. create a new EF migration
2. update the database

Suggested commands:

```powershell
Add-Migration AddAuthMealsRestaurantsModules
Update-Database
```

Or:

```powershell
dotnet ef migrations add AddAuthMealsRestaurantsModules
dotnet ef database update
```

Also ensure the correct SQL Server connection string is used.

Recommended connection string for the local server:

```json
"DefaultConnection": "Server=.;Database=HealthyBitesDb;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True"
```

---

### 6.2 Expand Meal Model to Match Real Dataset

The current meal model is still smaller than the real dataset.

Need to add:

- `PreparationMethod`
- `MedicalBenefitNote`
- `Carbohydrates`
- `Protein`
- `TotalFat`
- `SaturatedFat`
- `Sodium`
- `Potassium`
- `ServingSize`
- `Cuisine`
- `SuitableForDiabetesAndHypertension`

Still keep:

- `ImageUrl`

Because meal images are confirmed as part of the final website experience.

Need to update accordingly:

- `Models/Meal.cs`
- meal DTOs
- `MealService`
- import logic
- meal response sent to frontend

---

### 6.3 Fix Multi-Disease Filtering Logic

Current implementation should be revised.

Required rule:

- single disease user: return meals that support that disease
- multi-disease user: return only meals that support all the user diseases

This is because the real dataset contains:

- single-disease meals
- two-disease meals
- three-disease meals
- four-disease meals

The filtering logic must respect those exact combinations.

This should be updated inside:

- `Services/MealService.cs`

Recommended implementation idea:

- normalize disease names into tags
- compare user disease set with meal disease set
- return meal only if:
  - `userDiseaseTags.All(tag => mealDiseaseTags.Contains(tag))`

Example:

- user diseases:
  - `diabetes_type2`
  - `obesity`
- meal tags:
  - `diabetes_type2`
  - `obesity`
  - result: return meal

- meal tags:
  - `obesity`
  - result: do not return meal

- meal tags:
  - `diabetes_type2`
  - result: do not return meal

---

### 6.4 Meal Images Support

Requirement confirmed:

- meal images will exist and should be shown in the website

Need to do:

1. create a static files folder such as:
   - `wwwroot/images/meals`
2. enable static files in `Program.cs`
3. store image path or URL in `Meal.ImageUrl`
4. make API return image URL
5. map meals to their image files

Optional future improvement:

- create upload endpoint for meal images

Expected operational flow:

1. meal images are collected in a known folder
2. each meal is mapped to its image
3. `ImageUrl` is stored in database
4. API returns `ImageUrl`
5. frontend renders meal image directly

If image names match meal names closely, automatic mapping may be possible.
If not, a separate mapping sheet/file should be used.

---

### 6.5 Restaurant Dataset Alignment

The provided restaurant dataset contains additional fields:

- `branch`
- `city`
- `category`
- `has_menu_online`

Current restaurant model does not yet store all of them.

Recommended follow-up:

- add these fields to `Restaurant`
- update DTOs and API responses
- support Google Maps links in frontend display

Confirmed business need:

- restaurants should be connected to Google Maps
- website should be able to open restaurant location through `GoogleMapsLink`

If dataset does not already contain maps links, they can be added later manually or through a mapping/import step.

---

### 6.6 Authorization Rules

Authentication infrastructure exists, but authorization rules are not fully applied yet.

Still needed:

- protect sensitive endpoints with `[Authorize]`
- restrict admin-style operations such as:
  - create meal
  - update meal
  - delete meal
  - create restaurant
- ensure user can update only their own profile

Possible next step:

1. add `Role` field to `User`
2. include role in JWT token
3. use:

```csharp
[Authorize]
```

and where needed:

```csharp
[Authorize(Roles = "Admin")]
```

---

### 6.7 Data Import

The meal and restaurant data currently exist in Excel files.

Still needed:

- create import script or service to load Excel data into database
- map disease strings to normalized disease tags
- map meal images to meals
- optionally seed restaurants from Excel

Recommended handling strategy:

1. do not connect frontend directly to Excel files
2. use Excel only as source data
3. import into SQL Server
4. read from API afterward

Meal import should:

- parse nutrition values
- parse `Diseases`
- split disease combinations correctly
- store preparation method
- store medical benefit note
- store image URL

Restaurant import should:

- parse restaurant name
- parse branch and city
- parse phone and address
- parse rating and category
- optionally enrich with Google Maps link

Recommended order:

1. finalize models
2. create migration
3. update database
4. import meals
5. import restaurants
6. verify API output

---

### 6.8 Frontend Contract Finalization

Before frontend integration, finalize the exact meal response object.

Recommended meal response fields:

- `id`
- `name`
- `imageUrl`
- `ingredients`
- `preparationMethod`
- `medicalBenefitNote`
- `cuisine`
- `servingSize`
- `calories`
- `carbohydrates`
- `protein`
- `totalFat`
- `saturatedFat`
- `sodium`
- `potassium`
- `mealType`
- `diseaseTags`

This will allow the website to show:

- meal name
- meal image
- preparation steps
- benefit note
- nutrition section
- disease relevance

Recommended restaurant response fields:

- `id`
- `name`
- `branch`
- `city`
- `address`
- `phone`
- `averageRating`
- `category`
- `hasMenuOnline`
- `googleMapsLink`

---

## 7. Recommended Next Steps

Follow this order:

### Step 1 - Fix connection string

Update `appsettings.json` so SQL Server points to the correct local server:

```json
"DefaultConnection": "Server=.;Database=HealthyBitesDb;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True"
```

### Step 2 - Create migration

Run:

```powershell
Add-Migration AddAuthMealsRestaurantsModules
```

### Step 3 - Update database

Run:

```powershell
Update-Database
```

### Step 4 - Expand meal schema to match dataset

Add all meal nutrition and preparation fields.

### Step 5 - Update meal filtering logic

Implement strict multi-disease matching.

### Step 6 - Add static image support

- create meal image folder
- enable static files
- return image URLs in API

### Step 7 - Import meals from Excel

- parse Excel
- map disease tags
- map image names
- save into database

### Step 8 - Import restaurants from Excel

- parse restaurants dataset
- include google maps links if available
- extend fields if needed

### Step 9 - Align restaurant fields with maps and frontend needs

- add branch
- add city
- add category
- add hasMenuOnline
- finalize google maps link strategy

### Step 10 - Add authorization rules

- secure sensitive endpoints
- restrict admin-only operations

### Step 11 - Test with Swagger

Verify:

- register
- login
- forgot/reset password
- profile get/update
- meals listing
- meals for user
- restaurants
- reviews
- weight dynamics

### Step 12 - Frontend integration

After backend and import are stable:

- connect website to meals API
- show meal image
- show meal preparation method
- show medical benefit note
- show nutrition values in separated UI fields
- ensure Type 1 and Type 2 logic stays distinct
- ensure multi-disease users only see meals that match all their diseases

---

## 8. Files Most Important for Next Work

Core backend files:

- `Program.cs`
- `Data/AppDbContext.cs`
- `Models/User.cs`
- `Models/Meal.cs`
- `Models/Restaurant.cs`
- `Models/RestaurantReview.cs`
- `Services/AuthService.cs`
- `Services/MealService.cs`
- `Services/RestaurantService.cs`
- `Services/WeightDynamicsService.cs`
- `Controllers/AuthController.cs`
- `Controllers/MealController.cs`
- `Controllers/RestaurantsController.cs`
- `Controllers/WeightDynamicsController.cs`

---

## 9. Final Notes

The project is in a good intermediate state:

- compile is clean
- architecture is aligned better with the requested API
- major modules are in place

The next main phase is not rewriting core logic again.
The next phase is:

- finishing database migration
- aligning meals with the real dataset
- adding image support
- tightening authorization
- importing real data
