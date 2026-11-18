# Openpay Platform

RESTful API built with C# 14, .NET 10 and ASP.NET Core that supports the settlement and funds transfer operations of Openpay Peru.
It registers Disbursement Orders through `POST /api/v1/disbursement-orders`, stored in MySQL (`openpay_settlements` schema).

## Author Student

Geraldine Suarez Chinga - U20241C804 - NRC 8168

## Run

```bash
dotnet run --project PC28168.U20241C804.Openpay.Platform --launch-profile http
```

Extra:
API documentation: `http://localhost:5128/scalar`
