What is this?
-------------

This is the notes for the presenter of Test Driven Development, Part
2.

The course is divided into three different excerises, or labs.

# Lab 1

Checkout branch `lab/1`.

The first is a demonstration, where you - the presenter - will code
live in front of the audience to show how we create Level 1 tests.

The purpose of this demonstration is to show that we use exactly the
same tools for writing higher level tests that we do for unit tests.
Also, we hope to show that the tests that we develop here have a
slightly different purpose.  We intend to test the integration with
the database.

When done, you can let the group try this on their own.

Open the `ServiceTests` class at L1 and write the tests
`ShouldBeEmptyWhenCreated` and `ShouldAddProductToBasket`.  Note that
these tests to not run unless you fix `testsettings.json`, which gives
you an opportunity to talk about test configuration.

```c#
[Fact]
public async Task ShouldBeEmptyWhenCreated()
{
    var basket = await basketService.CreateBasket();

    Assert.Equal(0, basket.Count);
    Assert.Empty(basket.Products);
    Assert.Equal(Money.None, basket.Total);
    Assert.Equal(Money.None, basket.TotalWithVat);
}

[Fact]
public async Task ShouldAddProductToBasket()
{
    var created = await basketService.CreateBasket();

    await basketService.AddProductToBasket(created.Id, 42);

    var basket = await basketService.GetById(created.Id);

    Assert.Equal(1, basket.Count);
    Assert.Equal(new Money(13.76, SE), basket.Total);
    Assert.Equal(new Money(13.76, SE) * 1.25, basket.TotalWithVat);
    Assert.Equal("My Product", basket.Products.Single().Name);
}
```

# Lab 2

The purpose of this lab is to introduce tests that verify the hosted
solution.  This means that we have to get the API running, which is
probably the most important realization of this lab.  There is an
external product service that is mocked out.

In order to do this, you must first walk the group through the
intended function of the system.  This is another important
realization.  If we are to write good level 2 tests, we need to have
an understanding of the requirements.

Ask the students what level 2 tests they would like to write and give
them five minutes to think about it.� Emphasize that the purpose of
level 2 tests are to verify the function of the application.

Then, list the tests on a white board that the students have agreed
upon:

Happy Case: 

  * ShouldGetProducts()
  * ShouldCreateBasket()
  * ShouldGetBasket()
  * ShouldAddProductToBasket()
  * ShouldCheckout()

There is an error to find in this lab, and that is the fact that the
API allows the client to send in the total cost.

Sending cost might be OK, but only if it is validated on the backend.

# Lab 3

The purpose of this lab is to introduce the external product service,
which was mocked out for lab 2.  The issues that arise here is to
introduce the need to be able to run the API in different
configurations, with or without the product service.

The error to find is a misconfiguration of the service URL, or that we
have committed the URL to source code, or similar.

# Lab 4

This is not really a lab, but a throught experiment for the group.
Given the work we have done so far, what would it take for us to run
these set of tests against the production environment?  What would the
value be?  What would the costs be?

Products service
----------------

The `ProductService` class depends on an HTTP resource that will
return a product list as a JSON structure.  You can setup Azure
Functions to do this:

```json
{
    "$schema": "http://json.schemastore.org/proxies",
    "proxies": {
        "Products": {
            "matchCondition": {
                "route": "/products",
                "methods": [
                    "GET"
                ]
            },
            "responseOverrides": {
                "response.statusCode": "200",
                "response.body": "[{ \"id\": 1, \"name\": \"Apple\", \"cost\": { \"units\": 1376, \"decimalPlaces\": 2, \"currencyCode\": \"SEK\"}}, { \"id\": 2, \"name\": \"Banana\", \"cost\": { \"units\": 4455, \"decimalPlaces\": 2, \"currencyCode\": \"SEK\"}}]",
                "response.headers.Content-Type": "application/json"
            }
        },
        "ProductById": {
            "matchCondition": {
                "route": "/products/{id}",
                "methods": [
                    "GET"
                ]
            },
            "responseOverrides": {
                "response.statusCode": "200",
                "response.body": "{ \"id\": \"{id}\", \"name\": \"Apple\", \"cost\": { \"units\": 1376, \"decimalPlaces\": 2, \"currencyCode\": \"SEK\"}}",
                "response.headers.Content-Type": "application/json"
            }
        }
    }
}
```

See the documentation on Azure Functions for more information:

* https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-serverless-api

[1]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
[2]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments/
