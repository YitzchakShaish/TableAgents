using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DAL dal = new DAL();
             dal.CreateDatabase("CommandoDB");

            dal.CreateTable("Commandos", "CommandoDB");
            //dal.DropTable("commandos", "CommandoDB");
            //dal.SwitchDatabase("commandoDB");
            ModelsAgent agent = new ModelsAgent
            (
                "123321",
                "Dany Levi",
                "Ashdod",
                2,
                "Active"
            );
            ModelsAgent agent2 = new ModelsAgent
           (
               "aaaAAA",
               "Avrham Coen",
               "Ashdod",
               5,
               "Missing"
           );

            dal.AddAgent(agent, "Commandos");
            dal.AddAgent(agent2, "Commandos");


            List<ModelsAgent> listAgent= dal.getAgent();
            foreach (ModelsAgent age in listAgent)
               age.PrintDataList();
            dal.UpdateAgentLocation(1, "home");
            //dal.DeleteAgent(1);
            List<ModelsAgent> listAgent2 = dal.getAgent();
            foreach (ModelsAgent age in listAgent2)
                age.PrintDataList();

            //dal.closeConnection();

        }
    }
}
