namespace DebtusTestTask.Utils;

public class ServiceResult<T>
{
    public bool IsSuccessfull { get; set; }
    public ICollection<string>? Messages { get; set; }
    public T? Result { get; set; }
}
