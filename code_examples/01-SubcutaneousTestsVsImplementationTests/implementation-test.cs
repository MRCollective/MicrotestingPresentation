public class StudentControllerTests
{
	[SetUp]
	public void Setup()
	{
		_studentRepository = Substitute.For<IStudentRepository>();
		_controller = new StudentController(_studentRepository);
	}
	
	[Test]
	public void IndexAction_ShowsStudents()
	{
		var expectedStudents = new List<Student>
		{
			new Student("Joe", "Bloggs"),
			new Student("Jane", "Smith")			
		};
		_studentRepository.GetAll().Returns(expectedStudents);
		
		_controller.Index()
			.ShouldRenderDefaultAction()
			.WithModel<List<StudentViewModel>>(vm => {
				vm.Count.ShouldBe(expectedStudents.Count);
				vm[0].Name.ShouldBe(expectedStudents[0].FullName);
				vm[1].Name.ShouldBe(expectedStudents[1].FullName);
			});
	}
	
	private StudentController _controller;
	private IStudentRepository _studentRepository;
}
 
public class StudentRepositoryTests
{
	[SetUp]
	public void Setup()
	{
		_fixture = new DatabaseFixture();
		_repository = new StudentRepository(_fixture.WorkContext);	
	}
	
	[Test]
	public void GetAllMethod_ReturnsStudentsInDatabaseOrderedByName()
	{
		var expectedStudents = new List<Student>
		{
			new Student("Joe", "Bloggs"),
			new Student("Jane", "Smith")			
		};
		expectedStudents.ForEach(_fixture.SeedContext.Students.Add)
		_fixture.SeedContext.SaveChanges();
		
		var students = _repository.GetAll();
		
		students.Select(s => s.Id).ShouldBe(
			expectedStudents.OrderBy(s => s.FullName).Select(s => s.Id));
	}
	
	private StudentRepository _repository;
	private DatabaseFixture _fixture;
	
}