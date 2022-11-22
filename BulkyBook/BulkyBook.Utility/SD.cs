using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
	public static class SD
	{
		public const string Role_User_Indi = "Individual";
		public const string Role_User_Comp = "Empresa";
		public const string Role_Admin = "Admin";
		public const string Role_Employee = "Funcionário";

		public const string StatusPending = "Pendente";
        public const string StatusApproved = "Aprovado";
        public const string StatusInProcess = "Processando";
        public const string StatusShipped = "Enviado";
        public const string StatusCancelled = "Cancelado";
        public const string StatusRefunded = "Ressarcido";

        public const string PaymentStatusPending = "Pendente";
        public const string PaymentStatusApproved = "Aprovado";
        public const string PaymentStatusDelayedPayment = "Aprovado para parcelamento";
        public const string PaymentStatusRejected = "Rejeitado";

        public const string SessionCart = "Seção de carrinho";

    }
}
