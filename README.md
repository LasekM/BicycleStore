# Sklep Motorowy
BicycleStore to aplikacja stworzona przy użyciu ASP.NET Core, która zarządza sklepem rowerowym, w tym klientami, zamówieniami, rowerami oraz dostawcami. Aplikacja zawiera również mechanizmy autoryzacji i uwierzytelniania za pomocą JWT.
## Spis treści
- [Wymagania](#wymagania)
- [Struktura bazy danych](#struktura-bazy-danych)
- [Instalacja](#instalacja)
- [Migracje bazy danych](#migracje-bazy-danych)
- [Uruchamianie aplikacji](#uruchamianie-aplikacji)
- [Struktura projektu](#struktura-projektu)
- [API Endpoints](#api-endpoints)
- [Autoryzacja](#autoryzacja)

## Wymagania
- .NET 8.0 SDK lub nowszy
- Visual Studio obsługujący .NET 8.0
  
## Struktura bazy danych
![image](https://github.com/LasekM/BicycleStore/assets/27893189/1cc66cec-34f9-4a48-8415-8497b8b9bdc7)

## Instalacja

1. Sklonuj repozytorium:
    ```sh
    git clone https://github.com/TwojeRepo/BicycleStoreAPI.git
    cd BicycleStoreAPI
    ```

2. Przygotuj plik konfiguracyjny:
    - Upewnij się, że plik `appsettings.json` zawiera poprawne ustawienia połączenia z bazą danych.

## Migracje bazy danych

Aby zastosować migracje bazy danych, uruchom następujące polecenie:

```sh
dotnet ef database update
```
*do uruchomienia projektu można wykorzystać baze dołączoną z projektem.

## Uruchamianie aplikacji
Aby uruchomić aplikację, użyj polecenia:
```sh
dotnet run
```
## Struktura projektu

```plaintext
BicycleStore/
│
├── BicycleStore/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── BikeController.cs
│   │   ├── OrderController.cs
│   │   ├── HomeController.cs
│   │   └── SupplierController.cs
│   ├── Models/
│   │   ├── Bike.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   └── Supplier.cs
│   ├── DbContext/
│   │   └── AppDbContext.cs
│   └── Views/
│   
│
├── JwtAuthApi/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   ├── Models/
│   │   ├── LoginModel.cs
│   │   ├── RegisterModel.cs
│   │   └── User.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── BicycleStoreAPI/
│   ├── Controllers/
│   │   ├── BikeController.cs
│   │   ├── CustomerController.cs
│   │   ├── OrderController.cs
│   │   └── SupplierController.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   ├── Bike.cs
│   │   ├── Customer.cs
│   │   ├── ErrorViewModel.cs
│   │   ├── Order.cs
│   │   └── Supplier.cs
│   ├── Services/
│   │   ├── IBikeService.cs
│   │   ├── ICustomerService.cs
│   │   ├── IOrderService.cs
│   │   ├── ISupplierService.cs
│   │   ├── MemoryBikeServices.cs
│   │   ├── MemoryCustomerServices.cs
│   │   ├── MemoryOrderServices.cs
│   │   └── MemorySupplierServices.cs
│   └── Bike.db
```

## API Endpoints
### Auth
#### POST /api/auth/register - Rejestracja użytkownika
#### POST /api/auth/login - Logowanie użytkownika
### Bikes
#### GET /api/bike - Pobranie wszystkich rowerów
#### GET /api/bike/{id} - Pobranie roweru po ID
#### POST /api/bike - Dodanie nowego roweru
#### PUT /api/bike/{id} - Aktualizacja roweru
#### DELETE /api/bike/{id} - Usunięcie roweru
### Orders
#### GET /api/order - Pobranie wszystkich zamówień
#### GET /api/order/{id} - Pobranie zamówienia po ID
#### POST /api/order - Dodanie nowego zamówienia
#### PUT /api/order/{id} - Aktualizacja zamówienia
#### DELETE /api/order/{id} - Usunięcie zamówienia
### Suppliers
#### GET /api/supplier - Pobranie wszystkich dostawców
#### GET /api/supplier/{id} - Pobranie dostawcy po ID
#### POST /api/supplier - Dodanie nowego dostawcy
#### PUT /api/supplier/{id} - Aktualizacja dostawcy
#### DELETE /api/supplier/{id} - Usunięcie dostawcy

## Autoryzacja
Aplikacja wykorzystuje JWT (JSON Web Tokens) do autoryzacji. Aby uzyskać dostęp do chronionych zasobów, dodaj nagłówek Authorization: Bearer {token} do swoich żądań HTTP.
