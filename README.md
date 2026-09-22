# SchemeServe Technical Test

## How to Run
If you have Visual Studio installed, then you should have MS SQL Local DB installed too. If not you can install it from: https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver17.

Load the solution into Visual Studio. Set the TechTest project as your start-up project and click "Start Debugging" or press F5.

The solution will start-up and you should see the console window report on creating the DbUp database and tables. A separate web browser will open and should automatically take you to: https://localhost:7208/scalar/

You can test the 2 API endpoints using Scalar.

Please send me an email (phillips.adrian@gmail.com) if you have any issues.

## Features
- OpenAPI attributes on API endpoints.
- .NET controllers for API endpoints.
- Separate domain models and API models so both can evolve separately.
- Mapster to easily mapping objects between layers.
- DDD/Onion flavoured simple architecture.
- Client for handling calls to CQC.
- Custom exceptions for propagating CQC issues.
- Typed HttpClient to avoid repeatition of setting the base URI and APIM subscription key.
- Thin controllers, fat model.
- CQC client and repositories deal in domain models.
- Comments around provider caching details.
- Constant strings for things that shouldn't change.
- Config for things that might change.
- Flattened LastInspectionDate property to avoid adding complexity to model depth.
- RESTful API design.
- Expiry logic for provider moved inside the provider model.
- Repository abstraction for easier unit testing.
- DbUp used for simple database and schema creation.
- Microsoft SQL Server Local used for low/no install (should be already installed if you're running Visual Studio).
- Scalar for ease of testing APIs (https://localhost:7208/scalar/) (only visible when running in 'Development' environment).
- xUnit, Moq, AAA notation and SUT notation for unit testing.

## Notes
- The GetProviders API endpoint, takes zero parameters and works with the first page of CQC data only. I didn't add paging pass-through as it didn't seem to prove anything.
- Sensible data lengths and boundaries were given to CQC data types due to missing detail in the CQC OpenAPI schema.

## Alternative Paths Not Taken
- Used Redis and stored the provider data as JSON.
- Could use FastEndpoints (Vertical Slice Architecture might be a good fit for this kind of connector app).

## Options for Production
- Added more unit tests.
- Connection string and APIM subscription key value are in config, but in production would be stored as secrets somewhere secure like an Azure Key Vault.
- Moved the "1 month" expiry timespan into config.
- Used database transactions in places.
- Added logging and observability.
- For more complex APIs I would use FluentValidation to validate request models.
- OAuth2 or API key security around the new API endpoints.
