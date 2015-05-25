public class ViewExistingStudentsScenario : SubcutaneousMvcScenario<StudentController>
{
	public void GivenExistingStudents()
	{
		_existingStudents = StudentBuilder.CreateListOfSize(3);
		SeedContext.Save(_existingStudents);
	}
	
	public void WhenUserViewsStudents()
	{
		ExecuteControllerAction(c => c.Index());
	}
	
	public void ThenUserShouldSeeStudentsOrderedByName()
	{
		List<StudentViewModel> viewModel;
		ActionResult.ShouldRenderDefaultView()
			.WithModel<List<StudentViewModel>>(vm => viewModel = vm);
			
		viewModel.Select(s => s.Name).ShouldBe(
			_existingStudents.OrderBy(s => s.FullName).Select(s => s.FullName))
	}
	
	private List<Student> _existingStudents;
}