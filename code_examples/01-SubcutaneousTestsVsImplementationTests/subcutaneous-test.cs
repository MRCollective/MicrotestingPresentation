public class StudentControllerTests
{
	[SetUp]
	public void Setup()
	{
		_fixture = new DatabaseFixture();
		_controller = new StudentController(new StudentRepository(_fixture.Context));
	}
	
	[Test]
	public void IndexAction_ShowsAllStudentsOrderedByName()
	{
		var expectedStudents = new List<Student>
		{
			new Student("Joe", "Bloggs"),
			new Student("Jane", "Smith")			
		};
		expectedStudents.ForEach(_fixture.SeedContext.Students.Add)
		_fixture.SeedContext.SaveChanges();
		
		List<StudentViewModel> viewModel;
		_controller.Index()
			.ShouldRenderDefaultView()
			.WithModel<List<StudentViewModel>>(vm => viewModel = vm);
			
		viewModel.Select(s => s.Name).ShouldBe(
			_existingStudents.OrderBy(s => s.FullName).Select(s => s.FullName))
	}
	
	private StudentController _controller;
	private DatabaseFixture _fixture;
}