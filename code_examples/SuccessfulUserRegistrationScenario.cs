public class SuccessfulUserRegistrationScenario : SubcutaneousMvcTest<UserRegistrationController>
{
    public void GivenValidUserRegistrationData()
    {
        _viewModel = Builder<UserRegistrationViewModel>.CreateNew().Build();
    }

    public void WhenRegisteringTheUser()
    {
        ExecuteControllerAction(c => c.Index(_viewModel));

        _savedUser = VerifyDbContext.Users.SingleOrDefault();
    }

    public void ThenRedirectUserToSuccessPage()
    {
        ActionResult.ShouldRedirectTo(c => c.Success);
    }

    public void AndTheUserShouldBePersisted()
    {
        _savedUser.ShouldNotBe(null);
    }

    public void AndTheUserPersonalDetailsShouldBeCorrect()
    {
        _savedUser.ShouldSatisfyAllConditions(
            () => _savedUser.PersonalDetails.Title.ShouldBe(_viewModel.PersonalDetails.Title),
            () => _savedUser.PersonalDetails.FirstName.ShouldBe(_viewModel.PersonalDetails.FirstName),
            () => _savedUser.PersonalDetails.LastName.ShouldBe(_viewModel.PersonalDetails.LastName),
            () => _savedUser.PersonalDetails.Email.ShouldBe(_viewModel.PersonalDetails.Email),
            () => _savedUser.PersonalDetails.Telephone.ShouldBe(_viewModel.PersonalDetails.Telephone),
            () => _savedUser.PersonalDetails.Fax.ShouldBe(_viewModel.PersonalDetails.Fax)
        );
    }

    public void AndThePostalAddressShouldBeCorrect()
    {
        _savedUser.ShouldSatisfyAllConditions(
            () => _savedUser.PostalAddress.Line1.ShouldBe(_viewModel.PostalAddress.AddressLine1),
            () => _savedUser.PostalAddress.Line2.ShouldBe(_viewModel.PostalAddress.AddressLine2),
            () => _savedUser.PostalAddress.Line3.ShouldBe(_viewModel.PostalAddress.AddressLine3),
            () => _savedUser.PostalAddress.City.ShouldBe(_viewModel.PostalAddress.City),
            () => _savedUser.PostalAddress.State.ShouldBe(_viewModel.PostalAddress.State),
            () => _savedUser.PostalAddress.Postcode.ShouldBe(_viewModel.PostalAddress.Postcode),
            () => _savedUser.PostalAddress.Country.ShouldBe(_viewModel.PostalAddress.Country)
        );
    }

    public void AndThePasswordShouldBeCorrectlyHashedUsingBcrypt()
    {
        _savedUser.Password.Value.ShouldNotBe(_viewModel.Password);
        BCrypt.Net.BCrypt.Verify(_viewModel.Password, _savedUser.Password.Value);
    }

    public void AndTheCreatedAndModifiedDateShouldBeSetToNow()
    {
        _savedUser.CreatedDate.ShouldBe(Resolve<IDateTimeProvider>().Now());
        _savedUser.LastModifiedDate.ShouldBe(Resolve<IDateTimeProvider>().Now());
    }
    
    public void AndSendARegistrationEmailToTheUser()
    {
        var emailService = Resolve<MockEmailSendingService>();
        var sentEmail = emailService.SentEmails.Single();
        sentEmail.ShouldSatisfyAllConditions(
            () => sentEmail.To.Count.ShouldBe(1),
            () => sentEmail.To.First().EmailAddress.ShouldBe(_viewModel.PersonalDetails.Email),
            () => sentEmail.To.First().Name.ShouldBe($"{_viewModel.PersonalDetails.FirstName} {_viewModel.PersonalDetails.LastName}"),
            () => sentEmail.Subject.ShouldBe("Welcome to Site X!")
        );

        sentEmail.Body.ShouldMatchApproved();
    }
    
    private UserRegistrationViewModel _viewModel;
    private User _savedUser;
}
