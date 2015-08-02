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

    Assert.That(applies, Is.EqualTo(state == State.WA && (age == 18 || age == 19)));
}

...

[Test]
public void GivenProductsWithVarietyOfStateRules_WhenQuerying_ThenOnlyReturnTheProductsThatApplyToTheMember()
{
    var member = ObjectMother.Members.WAMember.Build();
    var products = ProductBuilder.CreateListOfSize(3)
        .TheFirst(1).WithCampaign(c => c.ForAllMembers())
        .TheNext(1).WithCampaign(c => c.ForState(State.ACT))
        .TheNext(1).WithCampaign(c => c.ForState(member.State))
        .BuildList();
    Session.SaveAll(products);

    var result = Execute(new GetProductsForMember(member));

    var expectedIds = products.Where(p => p == Product.AllMembers || p.State == member.State).Select(p => p.Id).ToArray();
    result.Select(p => p.Id).ToArray()
        .ShouldBe(expectedIds);
}