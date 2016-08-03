public class ViewExistingStudentsScenario
{
	public ViewExistingStudentsScenario()
	{
		_fixture = new DatabaseFixture();
		_controller = new StudentController(new StudentRepository(_fixture.Context));
	}

	public void GivenExistingStudents()
	{
		var expectedStudents = new List<Student>
		{
			new Student("Joe", "Bloggs"),
			new Student("Jane", "Smith")			
		};
		_existingStudents = _fixture.SeedContext.Save(expectedStudents);
	}
	
	public void WhenUserViewsStudents()
	{
		_actionResult = _controller.Index();
	}
	
	public void ThenUserShouldSeeStudentsOrderedByName()
	{
		List<StudentViewModel> viewModel;
		_actionResult.ShouldRenderDefaultView()
			.WithModel<List<StudentViewModel>>(vm => viewModel = vm);
			
		viewModel.Select(s => s.Name).ShouldBe(
			_existingStudents.OrderBy(s => s.FullName).Select(s => s.FullName))
	}

	[Test]
	public void ExecuteScenario()
	{
		this.Bddfy();
	}
	
	private List<Student> _existingStudents;
	private ActionResult _actionResult;
	private DatabaseFixture _fixture;
}