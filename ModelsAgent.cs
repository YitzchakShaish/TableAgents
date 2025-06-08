using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_06
{
    internal class ModelsAgent
    {
        public ModelsAgent(int id, string codeName, string realName, string location, int missionsCompleted, string status)
        {
            Id = id;
            CodeName = codeName;
            RealName = realName;
            Location = location;
            MissionsCompleted = missionsCompleted;
            Status = status;
        }
        public ModelsAgent( string codeName, string realName, string location, int missionsCompleted, string status)
        {
            
            CodeName = codeName;
            RealName = realName;
            Location = location;
            MissionsCompleted = missionsCompleted;
            Status = status;
        }

        public  int Id { get; set; }
      public  string CodeName { get; set; }
      public string RealName { get; set; }
      public string Location { get; set; }
      public int MissionsCompleted { get; set; }
      public string Status { get; set; }



        public void PrintDataList()
        {
            Console.WriteLine($"Id: {Id}, CodeName: {CodeName}, RealName: {RealName}, Location: {Location}, MissionsCompleted: {MissionsCompleted}, Status: { Status}");
        }
    }
}
