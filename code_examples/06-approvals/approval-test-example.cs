public class NBNVoipAndInternationalCallsCheckoutScenario : SubcutaneousMvcTest<CheckoutController>
{
    public void GivenUserHasStartedASession()
    {
        _session = SeedContext.Sessions.Save(ObjectMother.Sessions.Default);
    }

    public void AndGivenUserHasAddedNBNVoipAndInternationalCallsBonusOption()
    {
        _session.AddToCart(ObjectMother.Products.NBN);
        _session.AddToCart(ObjectMother.Products.Voip);
        _session.AddToCart(ObjectMother.Products.InternationalCallsBonusOption);
        SeedContext.Save();
    }

    public void AndGivenTheUserHasProvidedTheirCheckoutDetails()
    {
        _checkoutDetails = Builder<CheckoutViewModel>.CreateNew().Build();
    }

    public void WhenTheUserChecksOutTheCart()
    {
        ExecuteControllerAction(c => c.Index(_checkoutDetails))
    }

    public void ThenRegisterTheOrder()
    {
        var order = VerifyContext.Orders.Single();
        // todo: verify the details in the order match the details in _checkoutDetails
    }

    public void AndSendRequestToTheProvisioningSystem()
    {
        var request = Resolve<MockProvisioningSystem>().Requests.Single();
        request.ShouldMatchApproved();
    }
}