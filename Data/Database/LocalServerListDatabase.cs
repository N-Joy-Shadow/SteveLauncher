using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SteveLauncher.Data.Database;

[Table("LocalServerList")]
public class LocalServerListDatabase {
 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string HostName { get; set; }
    public ushort Port { get; set; }
    
    public string SRVHostName { get; set; }
    public ushort SRVPort { get; set; }
}
