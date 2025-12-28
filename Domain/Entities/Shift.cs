using Domain.Interfaces;

namespace Domain.Entities;

    public class Shift : IHasGuid
    {

    private Guid _guid;
    private Guid _employeeGuid;
    private Guid _workdayGuid;
    private Guid _shiftCodeGuid;
    private Guid _shiftRemarkGuid;
    private Guid _filePathGuid;

    public string Id
    {
        get => _guid.ToString();
        set => _guid = Guid.Parse(value);
    }
    public Guid Guid
    {
        get => _guid;
        set =>  _guid = value;
    }

    public string EmployeeId
    {
        get => _employeeGuid.ToString();
        set => _employeeGuid = Guid.Parse(value);
    }
    public Guid EmployeeGuid
    {
        get => _employeeGuid;
        set => _employeeGuid = value;
    }

    public string WorkdayId
    {
        get => _workdayGuid.ToString();
        set => _workdayGuid = Guid.Parse(value);
    }
    public Guid WorkdayGuid
    {
        get => _workdayGuid;
        set => _workdayGuid = value;
    }

    public string ShiftCodeId
    {
        get => _shiftCodeGuid.ToString();
        set => _shiftCodeGuid = Guid.Parse(value);
    }
    public Guid ShiftCodeGuid
    {
        get => _shiftCodeGuid;
        set => _shiftCodeGuid = value;
    }

    public string ShiftCodeRemarkId
    {
        get => _shiftRemarkGuid.ToString();
        set => _shiftRemarkGuid = Guid.Parse(value);
    }
    public Guid ShiftRemarkGuid
    {
        get => _shiftRemarkGuid;
        set => _shiftRemarkGuid = value;
    }

    public string FilePathId
    {
        get => _filePathGuid.ToString();
        set => _filePathGuid = Guid.Parse(value);
    }
    public Guid FilePathGuid
    {
        get => _filePathGuid;
        set => _filePathGuid = value;
    }
    
    public string Time { get; set; }
    public DateTime CreatedAt { get; set; } 
}

