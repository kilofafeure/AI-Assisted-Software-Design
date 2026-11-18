namespace Domain;

public class TaskItem
{
    private const int MaxNameLength = 50;

    private Guid _id;
    private string _name = string.Empty;
    private string _data = string.Empty;
    private TaskType _type;

    public required Guid Id
    {
        get => _id;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Id no puede estar vacío.", nameof(Id));
            }

            _id = value;
        }
    }

    public required string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name no puede estar vacío.", nameof(Name));
            }

            if (value.Length > MaxNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(Name),
                    $"Name no puede exceder los {MaxNameLength} caracteres.");
            }

            _name = value;
        }
    }

    public bool Status { get; set; }

    public required string Data
    {
        get => _data;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Data no puede estar vacío.", nameof(Data));
            }

            _data = value;
        }
    }

    public required TaskType Type
    {
        get => _type;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new ArgumentOutOfRangeException(nameof(Type), "Type no es un tipo de tarea válido.");
            }

            _type = value;
        }
    }
}
