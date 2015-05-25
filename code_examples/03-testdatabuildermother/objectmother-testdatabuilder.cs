[Test]
[Combinatorial]
public void GivenDemographicWithStateAndAgeRange_WhenCheckingIfTheDemographicAppliesToAMember_ThenReturnTrueOnlyIfTheMemberConformsToAllParameters
    ([Range(1, 25)] int age, [ValueSource("AllStates")] State state)
{
    var now = DateTime.UtcNow;
    var member = ObjectMother.Members.Fred
        .InState(state)
        .WithAge(age, now)
        .Build();
    var demographic = ObjectMother.Demographics.MembersInWA
        .WithMinimumAge(18)
        .WithMaximumAge(19)
        .Build();

    var applies = demographic.Contains(member, now);

    Assert.That(applies, Is.EqualTo(state == State.Wa && (age == 18 || age == 19)));
}

...

[Test]
public void GivenProductsWithCurrentCampaignWithSomeThatApplyToTheMember_WhenQuerying_ThenReturnTheProductsThatApplyToTheMember()
{
    var member = ObjectMother.Members.WAMember.WithAge(10, _now).Build();
    var products = Builder<ProductBuilder>.CreateListOfSize(3)
        .TheFirst(1).WithCampaign(_now,
            ObjectMother.Campaigns.Current(_now).ForAllMembers()
        )
        .TheNext(1).WithCampaign(_now,
            ObjectMother.Campaigns.Current(_now).ForState(State.Act)
        )
        .TheNext(1).WithCampaign(_now,
            ObjectMother.Campaigns.Current(_now)
                .ForState(State.Wa)
                .WithMinimumAge(9)
                .WithMaximumAge(11)
        )
        .BuildList();
    products.ToList().ForEach(p => Session.Save(p));

    var result = Execute(new GetProductsForMember(_now, member));

    Assert.That(result.Select(p => p.Name).ToArray(), Is.EqualTo(new[] { products[0].Name, products[2].Name }));
}