# Technical Stories

## TS001 - Create a disbursement order

As a developer, I want to register a disbursement order through `POST /api/v1/disbursement-orders`, so that Openpay can settle and transfer funds to an affiliate store.

The following is presented here: 

1.- **Given** valid data,
  **when** the request is sent,
  **then** the API returns `201 Created`.
  
2.- **Given** an existing order tracking code,
  **when** the request is sent,
  **then** the API returns `409 Conflict`.
  
3.- **Given** data that breaks a business rule,
  **when** the request is sent,
  **then** the API returns `400 Bad Request`.
