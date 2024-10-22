using DAL.Entities;
using DAL.Enums;

namespace BLL.Models;

public class WordModel
{
    public Guid Id { get; set; }
    public string WordName { get; set; } = null!;
}