using NUnit.Framework;
using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigHub.Integration.Tests
{
    public class Isolated : Attribute, ITestAction //ITestAction is NUnit interface
    {
        private TransactionScope _transactionScope;
        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }

        public void AfterTest(TestDetails testDetails)
        {
            _transactionScope.Dispose(); // To rollback transaction
        }

        public void BeforeTest(TestDetails testDetails)
        {
            _transactionScope = new TransactionScope(); // set new transaction
        }
    }
}
