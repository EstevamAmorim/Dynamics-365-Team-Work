using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using System;
using System.Activities;

namespace TeamWorkAction
{
    public class BuscarNivelDoCliente : ActionImplement
    {
        [Output("Sucesso")]
        public OutArgument<bool> Sucesso { get; set; }

        public override void ExecuteAction(CodeActivityContext context)
        {
            Guid oportunidadeId = this.WorkflowContext.PrimaryEntityId;

            Entity recuperaIdDaConta = this.Service.Retrieve("opportunity", oportunidadeId, new ColumnSet("parentaccountid"));

            Guid idDaConta = ((EntityReference)recuperaIdDaConta["parentaccountid"]).Id;

            Entity recuperaNivelDoCliente = this.Service.Retrieve("account", idDaConta, new ColumnSet("tw_niveldocliente"));

            Guid idDoNivelDoCliente = ((EntityReference)recuperaNivelDoCliente["tw_niveldocliente"]).Id;

            Entity recuperaValorDoDesconto = this.Service.Retrieve("tw_niveldocliente", idDoNivelDoCliente, new ColumnSet("tw_desconto"));

            decimal PorcentagemDoDesconto = (decimal)recuperaValorDoDesconto["tw_desconto"];

            Entity OportunidadeParaAtualizar = new Entity("opportunity", oportunidadeId);
            OportunidadeParaAtualizar["discountpercentage"] = PorcentagemDoDesconto;
            this.Service.Update(OportunidadeParaAtualizar);

            Sucesso.Set(context, true);
        }
    }
}
