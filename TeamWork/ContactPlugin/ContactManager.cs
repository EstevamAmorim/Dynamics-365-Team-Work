using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Messages;

namespace ContactPlugin
{
    public class ContactManager : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(executionContext.UserId);

            Entity contact = (Entity)executionContext.InputParameters["Target"];

            if (contact.Contains("tw_cpf"))
            {
                string cpf = contact["tw_cpf"].ToString();

                QueryExpression retrieveContactWithCpf = new QueryExpression("contact");
                retrieveContactWithCpf.Criteria.AddCondition("tw_cpf", ConditionOperator.Equal, cpf);
                EntityCollection contacts = service.RetrieveMultiple(retrieveContactWithCpf);

                if (contacts.Entities.Count() > 0)
                {
                    throw new InvalidPluginExecutionException("Já existe um contato com este CPF");
                }
            }
        }
    }
}
