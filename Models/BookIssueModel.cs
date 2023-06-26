using System.ComponentModel.DataAnnotations.Schema;

public class BookIssueModel
{
    public int Id { get; set; }
    public BookModel BookId { get; set; }
    public ReaderModel ReaderId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
